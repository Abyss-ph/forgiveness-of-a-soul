using UnityEngine;
using UnityEngine.SceneManagement;

// Colocar em um objeto com Collider2D "Is Trigger" na entrada da caverna.
// O jogador só entra (troca de cena) se a marca exigida já foi gravada.
public class EntradaCaverna : MonoBehaviour
{
    [Header("Destino")]
    // Nome exato da cena (e ela precisa estar na lista de cenas do Build Settings)
    [SerializeField] private string nomeDaCena = "Caverna";

    [Header("Condição para entrar")]
    // Mesma marca configurada em "Marca Ao Aceitar" do NPC (ex.: "missaoSapo").
    // Se deixar vazio, a caverna fica sempre liberada.
    [SerializeField] private string marcaNecessaria = "missaoSapo";

    [Header("Aviso (opcional)")]
    // Objeto de UI mostrado enquanto o jogador está na entrada e a caverna ainda está bloqueada
    [SerializeField] private GameObject avisoBloqueado;

    private bool carregando;

    private void Start()
    {
        if (avisoBloqueado != null)
        {
            avisoBloqueado.SetActive(false);
        }
    }

    // Stay (e não Enter) para funcionar também se a marca for gravada com o jogador já parado na entrada
    private void OnTriggerStay2D(Collider2D outro)
    {
        if (carregando || !outro.CompareTag("Player")) return;

        if (ProgressoDoJogo.Tem(marcaNecessaria))
        {
            Entrar();
        }
        else if (avisoBloqueado != null)
        {
            avisoBloqueado.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D outro)
    {
        if (outro.CompareTag("Player") && avisoBloqueado != null)
        {
            avisoBloqueado.SetActive(false);
        }
    }

    private void Entrar()
    {
        carregando = true;

        // Garante que o jogo não fique pausado ao trocar de cena
        Time.timeScale = 1f;
        SceneManager.LoadScene(nomeDaCena);
    }
}
