using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject painelGameOver;
    [SerializeField] private GameObject painelVitoria;

    // Evita mostrar Game Over e Vitória ao mesmo tempo
    private bool jogoEncerrado;

    private void Start()
    {
        // Os painéis começam escondidos
        if (painelGameOver != null)
        {
            painelGameOver.SetActive(false);
        }

        if (painelVitoria != null)
        {
            painelVitoria.SetActive(false);
        }
    }

    // Chamado pela Vida do jogador quando ele morre
    public void MostrarGameOver()
    {
        if (jogoEncerrado)
            return;

        jogoEncerrado = true;

        if (painelGameOver != null)
        {
            painelGameOver.SetActive(true);
        }

        // Pausa o jogo atrás do painel
        Time.timeScale = 0f;
    }

    // Chamado quando o jogador vence (ex.: boss derrotado)
    public void MostrarVitoria()
    {
        if (jogoEncerrado)
            return;

        jogoEncerrado = true;

        if (painelVitoria != null)
        {
            painelVitoria.SetActive(true);
        }

        // Pausa o jogo atrás do painel
        Time.timeScale = 0f;
    }

    // Espera um pouco (para a animação de morte do boss aparecer) e mostra a vitória
    public void MostrarVitoriaComAtraso(float segundos)
    {
        StartCoroutine(VitoriaComAtraso(segundos));
    }

    private System.Collections.IEnumerator VitoriaComAtraso(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        MostrarVitoria();
    }

    // Botão "Reiniciar"
    public void ReiniciarCena()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        // Garante que o jogo não fique pausado ao trocar de cena
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}