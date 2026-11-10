using UnityEngine;

public class Inimigos : MonoBehaviour
{
    [SerializeField] private int danoDeContato = 10;

    // Identificador usado pelas missões (ex.: "Slime"). Deve ser igual ao "Id Alvo" do objetivo.
    [SerializeField] private string idInimigo = "Inimigo";

    public void Morrer()
    {
        // Conta progresso na missão ativa (se houver)
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.RegistrarProgresso(idInimigo);
        }

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
