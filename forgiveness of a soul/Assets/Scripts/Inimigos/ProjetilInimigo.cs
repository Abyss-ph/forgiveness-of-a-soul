using UnityEngine;

// Projétil disparado pelos inimigos. Causa dano no jogador e some ao bater em paredes.
public class ProjetilInimigo : MonoBehaviour
{
    [SerializeField] private float velocidade = 6f;
    [SerializeField] private int dano = 10;
    [SerializeField] private float tempoDeVida = 3f;

    private Vector3 direcao = Vector3.right;

    // Chamado pelo inimigo logo após criar o projétil
    public void Iniciar(Vector2 novaDirecao)
    {
        direcao = novaDirecao.normalized;

        // Gira o sprite para apontar na direção do tiro (considera que o sprite aponta para a direita)
        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angulo);
    }

    private void Start()
    {
        Destroy(gameObject, tempoDeVida);
    }

    private void Update()
    {
        transform.position += direcao * velocidade * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D outro)
    {
        // Ignora inimigos, outros projéteis inimigos e projéteis do jogador
        if (outro.GetComponentInParent<Inimigos>() != null) return;
        if (outro.GetComponentInParent<ProjetilInimigo>() != null) return;
        if (outro.GetComponentInParent<Projetil>() != null) return;

        Vida vida = outro.GetComponentInParent<Vida>();

        if (vida != null && vida.CompareTag("Player"))
        {
            vida.ReceberDano(dano);
            Destroy(gameObject);
            return;
        }

        // Some ao bater em algo sólido (parede). Triggers são ignorados.
        if (!outro.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
