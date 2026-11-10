using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject painelGameOver;

    private void Start()
    {
        // O painel de Game Over começa escondido
        if (painelGameOver != null)
        {
            painelGameOver.SetActive(false);
        }
    }

    // Chamado pela Vida do jogador quando ele morre
    public void MostrarGameOver()
    {
        if (painelGameOver != null)
        {
            painelGameOver.SetActive(true);
        }

        // Pausa o jogo atrás do painel
        Time.timeScale = 0f;
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