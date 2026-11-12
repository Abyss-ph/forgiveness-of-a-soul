using System.Collections.Generic;
using UnityEngine;

// IA simples para os inimigos:
// - persegue o jogador (o dano é causado pelo script Inimigos, por contato)
// - desvia de projéteis que estão vindo na direção dele
// - se afasta um pouco dos outros inimigos para não ficarem todos empilhados
public class InimigoIA : MonoBehaviour
{
    [Header("Perseguição")]
    [SerializeField] private float velocidade = 2.5f;
    // Só persegue se o jogador estiver dentro desse raio (valor alto = persegue de qualquer lugar)
    [SerializeField] private float raioPerseguicao = 30f;
    // Para de andar quando chegar bem perto (evita tremedeira em cima do jogador)
    [SerializeField] private float distanciaMinima = 0.2f;

    [Header("Esquiva de projéteis")]
    // Distância em que o inimigo "enxerga" projéteis
    [SerializeField] private float raioDeteccaoTiro = 4f;
    // Se o tiro passaria a menos que isso do centro do inimigo, ele considera que vai acertar
    [SerializeField] private float margemDeAcerto = 0.7f;
    // 1 = sempre desvia, 0 = nunca desvia
    [SerializeField, Range(0f, 1f)] private float chanceDeEsquivar = 0.7f;
    [SerializeField] private float multiplicadorVelocidadeEsquiva = 2f;
    [SerializeField] private float duracaoEsquiva = 0.3f;
    // Tempo em que ele não consegue esquivar de novo (senão ficaria impossível acertá-lo)
    [SerializeField] private float recargaEsquiva = 0.8f;

    [Header("Separação entre inimigos")]
    [SerializeField] private float distanciaEntreInimigos = 0.8f;

    private Transform alvo;
    private Rigidbody2D rb;

    private Vector2 direcaoMovimento;
    private float velocidadeAtual;

    private Vector2 direcaoEsquiva;
    private float fimEsquiva;
    private float proximaEsquivaPermitida;
    private Projetil ultimoTiroAvaliado;

    private float proximaChecagemTiros;
    private float proximaBuscaAlvo;

    private readonly Collider2D[] buffer = new Collider2D[16];
    private ContactFilter2D filtro;

    // Lista de todos os inimigos ativos (usada para a separação)
    private static readonly List<InimigoIA> todos = new List<InimigoIA>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        filtro = ContactFilter2D.noFilter;
    }

    private void OnEnable()
    {
        todos.Add(this);
    }

    private void OnDisable()
    {
        todos.Remove(this);
    }

    private void Start()
    {
        BuscarAlvo();
    }

    private void Update()
    {
        // Sem alvo (jogador ainda não existe ou morreu): fica parado e tenta achar de novo
        if (alvo == null)
        {
            direcaoMovimento = Vector2.zero;

            if (Time.time >= proximaBuscaAlvo)
            {
                proximaBuscaAlvo = Time.time + 0.5f;
                BuscarAlvo();
            }

            return;
        }

        // Verifica projéteis algumas vezes por segundo (não precisa ser todo frame)
        if (Time.time >= proximaChecagemTiros)
        {
            proximaChecagemTiros = Time.time + 0.05f;
            ChecarTiros();
        }

        // Durante a esquiva, ignora a perseguição
        if (Time.time < fimEsquiva)
        {
            direcaoMovimento = direcaoEsquiva;
            velocidadeAtual = velocidade * multiplicadorVelocidadeEsquiva;
        }
        else
        {
            DecidirPerseguicao();
        }

        // Sem Rigidbody2D, move direto pelo transform
        if (rb == null)
        {
            transform.position += (Vector3)(direcaoMovimento * velocidadeAtual * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        // Com Rigidbody2D, move pela física (respeita colisões com paredes)
        if (rb != null)
        {
            Vector2 deslocamento = direcaoMovimento * velocidadeAtual * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + deslocamento);
        }
    }

    private void BuscarAlvo()
    {
        GameObject jogador = GameObject.FindGameObjectWithTag("Player");
        alvo = jogador != null ? jogador.transform : null;
    }

    private void DecidirPerseguicao()
    {
        Vector2 paraAlvo = (Vector2)alvo.position - (Vector2)transform.position;
        float distancia = paraAlvo.magnitude;

        if (distancia > raioPerseguicao || distancia <= distanciaMinima)
        {
            direcaoMovimento = Vector2.zero;
            return;
        }

        Vector2 direcao = paraAlvo / distancia;

        // Soma um empurrãozinho para longe dos outros inimigos
        direcao += CalcularSeparacao();

        direcaoMovimento = direcao.normalized;
        velocidadeAtual = velocidade;
    }

    private Vector2 CalcularSeparacao()
    {
        Vector2 empurrao = Vector2.zero;

        for (int i = 0; i < todos.Count; i++)
        {
            InimigoIA outro = todos[i];
            if (outro == this) continue;

            Vector2 deMimParaOutro = (Vector2)transform.position - (Vector2)outro.transform.position;
            float distancia = deMimParaOutro.magnitude;

            if (distancia > 0.001f && distancia < distanciaEntreInimigos)
            {
                // Quanto mais perto, mais forte o empurrão
                empurrao += (deMimParaOutro / distancia) * (1f - distancia / distanciaEntreInimigos);
            }
        }

        return empurrao;
    }

    private void ChecarTiros()
    {
        // Ainda em recarga da última esquiva
        if (Time.time < proximaEsquivaPermitida) return;

        int quantidade = Physics2D.OverlapCircle(transform.position, raioDeteccaoTiro, filtro, buffer);

        for (int i = 0; i < quantidade; i++)
        {
            Projetil tiro = buffer[i].GetComponentInParent<Projetil>();
            if (tiro == null) continue;

            // Cada tiro é avaliado uma vez só (senão a "chance" de esquivar seria sorteada várias vezes)
            if (tiro == ultimoTiroAvaliado) continue;

            // O projétil gira o sprite para a direção do tiro, então transform.right é a direção dele
            Vector2 direcaoTiro = tiro.transform.right;
            Vector2 tiroParaMim = (Vector2)transform.position - (Vector2)tiro.transform.position;

            // Tiro já passou por mim ou está indo para o lado oposto
            if (Vector2.Dot(direcaoTiro, tiroParaMim) <= 0f) continue;

            // Distância lateral entre a linha do tiro e o centro do inimigo (com sinal: lado esquerdo/direito)
            float lateral = direcaoTiro.x * tiroParaMim.y - direcaoTiro.y * tiroParaMim.x;

            // Vai passar longe, não precisa desviar
            if (Mathf.Abs(lateral) > margemDeAcerto) continue;

            ultimoTiroAvaliado = tiro;

            // Às vezes o inimigo "erra" a reação
            if (Random.value > chanceDeEsquivar) return;

            // Desvia para o lado em que já está (o caminho mais curto para sair da linha do tiro)
            float lado = Mathf.Sign(lateral);

            // Tiro vindo bem no centro: escolhe um lado aleatório
            if (Mathf.Abs(lateral) < 0.05f)
            {
                lado = Random.value < 0.5f ? -1f : 1f;
            }

            direcaoEsquiva = new Vector2(-direcaoTiro.y, direcaoTiro.x) * lado;
            fimEsquiva = Time.time + duracaoEsquiva;
            proximaEsquivaPermitida = fimEsquiva + recargaEsquiva;
            return;
        }
    }
}
