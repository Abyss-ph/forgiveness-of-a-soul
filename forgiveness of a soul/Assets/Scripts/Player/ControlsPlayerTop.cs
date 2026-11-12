using UnityEngine;

public class ControlsPlayerTop : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Durante um diálogo o jogador fica parado (e a animação de andar para)
        if (dialogueManager.EmDialogo)
        {
            animator.SetBool("andando", false);
            return;
        }

        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool upPressed = Input.GetKey(KeyCode.W);
        bool downPressed = Input.GetKey(KeyCode.S);

        float horizontal;

        if (leftPressed && rightPressed)
        {
            horizontal = 0f;
        }
        else if (leftPressed)
        {
            horizontal = -1f;
        }
        else if (rightPressed)
        {
            horizontal = 1f;
        }
        else
        {
            horizontal = 0f;
        }

        float vertical;

        if (upPressed && downPressed)
        {
            vertical = 0f;
        }
        else if (upPressed)
        {
            vertical = 1f;
        }
        else if (downPressed)
        {
            vertical = -1f;
        }
        else
        {
            vertical = 0f;
        }

        // Combinando eixo vertical e horizontal para criar um vetor de movimento
        Vector3 direction = new Vector3(horizontal, vertical, 0f);

        direction = direction.normalized; // Normaliza o vetor para evitar movimento mais rápido na diagonal

        bool andando = direction != Vector3.zero;

        // --- ANIMAÇÃO ---
        animator.SetBool("andando", andando);

        // Só atualiza a direção enquanto está andando.
        // Assim, ao parar, o Blend Tree não recebe (0,0) e não dá glitch na transição.
        if (andando)
        {
            float animX = 0f;
            float animY = 0f;

            // Prioridade para os lados: na diagonal toca a animação lateral.
            // Isso evita valores "no meio" (ex: 0.7, 0.7), que fariam o Blend Tree
            // ficar trocando de sprite sem saber qual escolher.
            if (horizontal != 0f)
            {
                animX = horizontal;
            }
            else
            {
                animY = vertical;
            }

            animator.SetFloat("moveX", animX);
            animator.SetFloat("moveY", animY);
        }

        // A animação WalkR olha para a direita. Para a esquerda, espelhamos o sprite.
        // (Se o seu WalkR olha para a esquerda, troque para: horizontal > 0f)
        spriteRenderer.flipX = horizontal < 0f;

        // aplicar
        transform.position += direction * speed * Time.deltaTime;
    }
}
