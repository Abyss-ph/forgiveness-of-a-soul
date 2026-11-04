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

    //inicia um novo dialogo
    public void IniciarDialogo(string[] falas)
    {
        //Impede de iniciar um dialogo sem falas
        if (falas == null || falas.Length == 0)
        {
            Debug.LogWarning("Tentativa de iniciar um diálogo sem falas.");
            return;
        }

        //guarda as falas recebidas
        dialogos = falas;

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
        //Desativa o dialogo
        dialoguePanel.SetActive(false);
        dialogoAtivo = false;
        indiceDialogo = 0;
        dialogos = null;
    }
  



   
}
