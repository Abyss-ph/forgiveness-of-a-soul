using UnityEngine;
using TMPro;


public class dialogueManager : MonoBehaviour
{

   //Referências aos elementos da UI
   public GameObject dialoguePanel;
   public TMP_Text dialogueText;

    //guarda todas as falas do dialogo
   private string[] dialogos;

   //Indice qual fala do dialogo está sendo exibida
   private int indiceDialogo = 0;

    // Informa se existe um dialogo acontecendo
   private bool dialogoAtivo = false;

   //Ação opcional executada quando o dialogo termina (ex.: entregar uma missão)
   private System.Action aoFinalizarDialogo;

   //Permite que outros scripts saibam se há um dialogo acontecendo
   public bool DialogoAtivo => dialogoAtivo;


    private void Start()
    {
        //Inicia o painel de dialogo desativado
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        //Verifica se o dialogo está ativo e se o jogador apertou a tecla de avançar
        if (dialogoAtivo && Input.GetKeyDown(KeyCode.Space))
        {
            ProximaFala();
        }
    }

    //inicia um novo dialogo (aoFinalizar é opcional)
    public void IniciarDialogo(string[] falas, System.Action aoFinalizar = null)
    {
        //Impede de iniciar um dialogo sem falas
        if (falas == null || falas.Length == 0)
        {
            Debug.LogWarning("Tentativa de iniciar um diálogo sem falas.");
            return;
        }

        //guarda as falas recebidas
        dialogos = falas;
        aoFinalizarDialogo = aoFinalizar;

        //começa do inicio do dialogo
        indiceDialogo = 0;

        //Ativa o dialogo
        dialogoAtivo = true;
        dialoguePanel.SetActive(true);

        //Exibe a primeira fala
        dialogueText.text = dialogos[indiceDialogo];
    }

    //Exibe a próxima fala do dialogo
    public void ProximaFala()
    {
        //Passa para a próxima fala
        indiceDialogo++;

        //Verifica se ainda existem falas
        if (indiceDialogo < dialogos.Length)
        {
            dialogueText.text = dialogos[indiceDialogo];
        }
        else
        {
            FinalizarDialogo();
        }
    }

    //Finaliza o dialogo
    private void FinalizarDialogo()
    {
        //Guarda a ação antes de limpar o estado
        System.Action acao = aoFinalizarDialogo;

        //Desativa o dialogo
        dialoguePanel.SetActive(false);
        dialogoAtivo = false;
        indiceDialogo = 0;
        dialogos = null;
        aoFinalizarDialogo = null;

        //Executa a ação (ex.: aceitar a missão)
        if (acao != null)
        {
            acao.Invoke();
        }
    }
}