using UnityEngine;

public class Inimigos : MonoBehaviour
{
    [SerializeField] private int danoDeContato = 10;

    public void Morrer()
    {
        Destroy(gameObject);
    }

    // Enquanto o jogador estiver dentro da hitbox do inimigo, tenta causar dano.
    // O intervalo entre danos é controlado pelo tempoInvulneravel da Vida do jogador.
    void OnTriggerStay2D(Collider2D outro)
    {
        Vida vida = outro.GetComponentInParent<Vida>();

        if (vida != null && vida.CompareTag("Player"))
        {
            vida.ReceberDano(danoDeContato);
        }
    }
}
