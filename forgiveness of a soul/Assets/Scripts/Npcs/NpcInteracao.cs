using UnityEngine;

public class NpcInteracao : MonoBehaviour
{
    [Header("Diálogo")]
    public string[] falas;
    public GameObject interactionUI;
    // Referência ao DialogueManager
    public dialogueManager dialogueManager;

    [Header("Missão (opcional)")]
    // Se desligado, o NPC funciona como antes: só fala as "falas" acima
    [SerializeField] private bool entregaMissao = false;
    [SerializeField] private Missao missao;
    [SerializeField, TextArea(2, 4)] private string[] falasEmAndamento;   // missão aceita, ainda não terminou
    [SerializeField, TextArea(2, 4)] private string[] falasConcluida;     // missão terminada

    private enum EstadoMissao { SemMissao, EmAndamento, Entregue }
    private EstadoMissao estadoMissao = EstadoMissao.SemMissao;

    private bool podeInteragir = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            podeInteragir = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            podeInteragir = false;
        }
    }

    private void Update()
    {
        // Só dá para conversar se o jogador está perto e nenhum diálogo está aberto
        bool disponivel = podeInteragir && !dialogueManager.DialogoAtivo;

        // O aviso de interação aparece só quando dá para conversar
        if (interactionUI != null)
        {
            interactionUI.SetActive(disponivel);
        }

        if (disponivel && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Iniciando diálogo com NPC");
            Interagir();
        }
    }

    private void Interagir()
    {
        // NPC comum, sem missão
        if (!entregaMissao)
        {
            dialogueManager.IniciarDialogo(falas);
            return;
        }

        switch (estadoMissao)
        {
            case EstadoMissao.SemMissao:
                // A missão só começa depois que o diálogo termina
                dialogueManager.IniciarDialogo(falas, AceitarMissao);
                break;

            case EstadoMissao.EmAndamento:
                if (QuestManager.Instance.MissaoConcluida)
                {
                    dialogueManager.IniciarDialogo(falasConcluida, EntregarMissao);
                }
                else
                {
                    dialogueManager.IniciarDialogo(falasEmAndamento);
                }
                break;

            case EstadoMissao.Entregue:
                dialogueManager.IniciarDialogo(falasConcluida);
                break;
        }
    }

    private void AceitarMissao()
    {
        QuestManager.Instance.IniciarMissao(missao);
        estadoMissao = EstadoMissao.EmAndamento;
    }

    private void EntregarMissao()
    {
        QuestManager.Instance.EncerrarMissao();
        estadoMissao = EstadoMissao.Entregue;
    }
}


