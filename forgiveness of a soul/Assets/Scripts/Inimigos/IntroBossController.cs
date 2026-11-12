using System.Collections;
using TMPro;
using UnityEngine;

// Controla a sequência da cena:
// boss aparece -> diálogo (espaço) -> boss some -> inimigos aparecem + missão
// -> jogador mata todos -> mensagem "conseguiu a chave da masmorra"
public class IntroBossController : MonoBehaviour
{
    [Header("Referências da cena")]
    [SerializeField] private dialogueManager dialogueManager;
    [SerializeField] private GameObject boss;
    // Objeto pai que contém todos os inimigos (eles ficam desativados até o fim do diálogo)
    [SerializeField] private GameObject inimigos;

    [Header("Diálogo do boss")]
    [SerializeField, TextArea(2, 4)] private string[] falasBoss;

    [Header("Missão")]
    [SerializeField] private Missao missao;

    [Header("Mensagem da chave")]
    [SerializeField] private GameObject painelMensagem;
    [SerializeField] private TMP_Text textoMensagem;
    [SerializeField] private string mensagemChave = "Você conseguiu a chave da masmorra!";
    [SerializeField] private float tempoMensagem = 4f;

    // Outros scripts (ex.: porta da masmorra) podem consultar: IntroBossController.TemChaveMasmorra
    public static bool TemChaveMasmorra { get; private set; }

    // Start como corrotina: espera 1 frame para o dialogueManager.Start() rodar antes,
    // senão ele desativaria o painel logo depois de abrirmos o diálogo.
    private IEnumerator Start()
    {
        TemChaveMasmorra = false;

        boss.SetActive(true);
        inimigos.SetActive(false);
        painelMensagem.SetActive(false);

        yield return null;

        dialogueManager.IniciarDialogo(falasBoss, AoTerminarDialogo);
    }

    // Chamado pelo dialogueManager quando a última fala termina
    private void AoTerminarDialogo()
    {
        boss.SetActive(false);
        inimigos.SetActive(true);

        QuestManager.Instance.OnMissaoConcluida += AoConcluirMissao;
        QuestManager.Instance.IniciarMissao(missao);
    }

    // Chamado pelo QuestManager quando o último inimigo morre
    private void AoConcluirMissao()
    {
        QuestManager.Instance.OnMissaoConcluida -= AoConcluirMissao;
        QuestManager.Instance.EncerrarMissao();

        TemChaveMasmorra = true;
        StartCoroutine(MostrarMensagemChave());
    }

    private IEnumerator MostrarMensagemChave()
    {
        textoMensagem.text = mensagemChave;
        painelMensagem.SetActive(true);

        yield return new WaitForSeconds(tempoMensagem);

        painelMensagem.SetActive(false);
    }

    private void OnDestroy()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnMissaoConcluida -= AoConcluirMissao;
        }
    }
}