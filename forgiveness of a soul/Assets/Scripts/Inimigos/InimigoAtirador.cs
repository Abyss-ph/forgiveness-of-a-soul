using UnityEngine;

// Faz o inimigo atirar no jogador de tempos em tempos, quando ele está dentro do alcance.
// Funciona junto com o InimigoIA (que cuida de perseguir e desviar).
public class InimigoAtirador : MonoBehaviour
{
    [Header("Tiro")]
    [SerializeField] private ProjetilInimigo projetilPrefab;
    // Opcional: um objeto filho de onde o tiro sai. Se vazio, sai do centro do inimigo.
    [SerializeField] private Transform pontoDeTiro;

    [Header("Comportamento")]
    [SerializeField] private float alcance = 8f;
    [SerializeField] private float intervaloEntreTiros = 1.5f;
    // Tempo de espera antes do primeiro tiro (evita atirar no instante em que aparece)
    [SerializeField] private float atrasoInicial = 1f;
    // Erro de mira em graus (0 = mira perfeita)
    [SerializeField] private float imprecisao = 5f;

    private Transform alvo;
    private float proximoTiro;
    private float proximaBuscaAlvo;

    // OnEnable roda toda vez que o inimigo é ativado (ex.: quando o diálogo do boss termina)
    private void OnEnable()
    {
        proximoTiro = Time.time + atrasoInicial;
    }

    private void Update()
    {
        if (alvo == null)
        {
            if (Time.time >= proximaBuscaAlvo)
            {
                proximaBuscaAlvo = Time.time + 0.5f;
                GameObject jogador = GameObject.FindGameObjectWithTag("Player");
                alvo = jogador != null ? jogador.transform : null;
            }

            return;
        }

        if (Time.time < proximoTiro) return;

        Vector3 origem = pontoDeTiro != null ? pontoDeTiro.position : transform.position;
        Vector2 paraAlvo = (Vector2)alvo.position - (Vector2)origem;

        // Jogador fora do alcance
        if (paraAlvo.magnitude > alcance) return;

        Atirar(origem, paraAlvo);
        proximoTiro = Time.time + intervaloEntreTiros;
    }

    private void Atirar(Vector3 origem, Vector2 paraAlvo)
    {
        if (projetilPrefab == null)
        {
            Debug.LogWarning("InimigoAtirador sem projétil definido: " + name);
            return;
        }

        // Pequeno erro de mira para o tiro não ser sempre perfeito
        float erro = Random.Range(-imprecisao, imprecisao);
        Vector2 direcao = (Vector2)(Quaternion.Euler(0f, 0f, erro) * (Vector3)paraAlvo.normalized);

        ProjetilInimigo tiro = Instantiate(projetilPrefab, origem, Quaternion.identity);
        tiro.Iniciar(direcao);
    }
}
