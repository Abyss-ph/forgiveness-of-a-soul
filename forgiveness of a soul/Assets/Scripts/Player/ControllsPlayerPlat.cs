 
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ControllsPlayerPlat : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Detecção de chão")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float horizontal;
    private bool jumpPressed;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Durante um diálogo o jogador fica parado (a gravidade continua agindo)
        if (dialogueManager.EmDialogo)
        {
            horizontal = 0f;
            jumpPressed = false;
            animator.SetBool("andando", false);
            animator.SetBool("noChao", isGrounded);
            animator.SetFloat("velocidadeY", rb.linearVelocity.y);
            return;
        }

        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);

        // Se apertar os dois ao mesmo tempo, o resultado é 0
        horizontal = (rightPressed ? 1f : 0f) - (leftPressed ? 1f : 0f);

        // Pulo (Espaço ou W). Fica guardado até o FixedUpdate processar
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
            jumpPressed = true;

        // --- ANIMAÇÃO ---
        animator.SetBool("andando", horizontal != 0f);
        animator.SetBool("noChao", isGrounded);
        animator.SetFloat("velocidadeY", rb.linearVelocity.y);

        // Só muda o lado quando há input. Ao parar, o sprite continua virado
        // para o último lado, então o idle funciona para esquerda e direita.
        // (Se o seu sprite original olha para a esquerda, troque para: horizontal > 0f)
        if (horizontal != 0f)
            spriteRenderer.flipX = horizontal < 0f;
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        // Move só no eixo X; o Y fica por conta da gravidade
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);

        if (jumpPressed && isGrounded)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        jumpPressed = false;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}