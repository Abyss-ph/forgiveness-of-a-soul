using UnityEngine;

public class Vida : MonoBehaviour
{
    [SerializeField]public int vidaMaxima = 100;

    // Tempo sem receber dano após levar um golpe (use ~1 no jogador, 0 nos inimigos)
    [SerializeField] private float tempoInvulneravel = 0f;

    // Preencher só no jogador: é quem mostra a tela de Game Over
    [SerializeField] private GameManager gameManager;

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