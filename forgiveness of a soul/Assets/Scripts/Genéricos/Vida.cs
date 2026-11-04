using UnityEngine;

public class Vida : MonoBehaviour
{
    [SerializeField]public int vidaMaxima = 100;

    private Inimigos inimigos;

    private int vidaAtual;

    private bool estaMorto;

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
        }
    }
}
