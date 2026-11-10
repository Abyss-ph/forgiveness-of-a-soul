using UnityEngine;

public class Projetil : MonoBehaviour
{
    [SerializeField] private float velocidade = 10f;
    [SerializeField] private int dano = 25;
    [SerializeField] private float tempoDeVida = 2f;

    private Vector3 direcao = Vector3.right;

    // Chamado pelo jogador logo após criar o projétil
    public void Iniciar(Vector2 novaDirecao)
    {
        direcao = novaDirecao.normalized;

        // Gira o sprite para apontar na direção do tiro (considera que o sprite aponta para a direita)
        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angulo);
    }

    void Start()
    {
        Destroy(gameObject, tempoDeVida);
    }

    void Update()
    {
        transform.position += direcao * velocidade * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        Inimigos inimigo = outro.GetComponentInParent<Inimigos>();

        if (inimigo == null) return; // ignora jogador, paredes etc.

        Vida vida = inimigo.GetComponent<Vida>();

        if (vida != null)
        {
            vida.ReceberDano(dano);
        }

        Destroy(gameObject);
    }
}
