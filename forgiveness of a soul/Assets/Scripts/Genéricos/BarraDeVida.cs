using UnityEngine;

public class BarraDeVida : MonoBehaviour
{
    // Vida de quem carrega a barra (jogador ou inimigo). Se ficar vazio, procura no objeto "pai".
    [SerializeField] private Vida vida;

    // Parte colorida da barra (o sprite que encolhe)
    [SerializeField] private Transform preenchimento;

    private float escalaInicialX;
    private float bordaEsquerdaX;
    private float larguraSprite;

    private void Awake()
    {
        if (vida == null)
        {
            vida = GetComponentInParent<Vida>();
        }

        SpriteRenderer sprite = preenchimento != null ? preenchimento.GetComponent<SpriteRenderer>() : null;

        if (vida == null || sprite == null || sprite.sprite == null)
        {
            Debug.LogError("BarraDeVida: faltam referências (Vida, Preenchimento ou sprite do Preenchimento).", this);
            enabled = false;
            return;
        }

        // Guarda as medidas da barra cheia
        larguraSprite = sprite.sprite.bounds.size.x;
        escalaInicialX = preenchimento.localScale.x;

        // Borda esquerda da barra: ela fica parada enquanto o lado direito encolhe
        float larguraTotal = escalaInicialX * larguraSprite;
        bordaEsquerdaX = preenchimento.localPosition.x - larguraTotal / 2f;
    }

    private void LateUpdate()
    {
        float porcentagem = Mathf.Clamp01(vida.PorcentagemVida);

        float novaEscalaX = escalaInicialX * porcentagem;
        Vector3 escala = preenchimento.localScale;
        escala.x = novaEscalaX;
        preenchimento.localScale = escala;

        // Reposiciona para a borda esquerda continuar no mesmo lugar
        float novaLargura = novaEscalaX * larguraSprite;
        Vector3 posicao = preenchimento.localPosition;
        posicao.x = bordaEsquerdaX + novaLargura / 2f;
        preenchimento.localPosition = posicao;
    }
}
