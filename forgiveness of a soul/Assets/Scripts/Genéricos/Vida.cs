using UnityEngine;

public class Vida : MonoBehaviour
{
    [SerializeField]public int vidaMaxima = 100;

    // Tempo sem receber dano após levar um golpe (use ~1 no jogador, 0 nos inimigos)
    [SerializeField] private float tempoInvulneravel = 0f;

    // Preencher no jogador (Game Over) e no boss (Vitória)
    [SerializeField] private GameManager gameManager;

    // Marcar só no boss: quando ele morre, mostra o painel de vitória
    [SerializeField] private bool ehBoss = false;

    // Tempo de espera antes do painel de vitória (para a animação de morte aparecer)
    [SerializeField] private float atrasoVitoria = 1.5f;

    private Inimigos inimigos;

    private int vidaAtual;

    private bool estaMorto;

    private float invulneravelAte;

    public float PorcentagemVida => vidaMaxima > 0 ? (float)vidaAtual / vidaMaxima : 0f;

    private void Start()
    {
        vidaAtual = vidaMaxima;
        estaMorto = false;
        inimigos = GetComponent<Inimigos>();
    }

    public void ReceberDano(int dano)
    {
        if(estaMorto)
        return;

        if(Time.time < invulneravelAte)
        return;

        invulneravelAte = Time.time + tempoInvulneravel;

        vidaAtual = vidaAtual - dano;

        if(vidaAtual < 0)
        {
            vidaAtual = 0;
        }

        if(vidaAtual == 0)
        {
            Morrer();
        }
    }

    private void Morrer()
    {
        estaMorto = true;
       
       if(inimigos != null)
        {
            inimigos.Morrer();

            // Se for o boss, mostra o painel de vitória depois do atraso
            if(ehBoss && gameManager != null)
            {
                gameManager.MostrarVitoriaComAtraso(atrasoVitoria);
            }
        }
        else
        {
            Debug.Log("Jogador Morreu");

            if(gameManager != null)
            {
                gameManager.MostrarGameOver();
            }
        }
    }
}