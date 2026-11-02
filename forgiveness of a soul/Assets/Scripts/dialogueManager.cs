using UnityEngine;
using TMPro;


public class dialogueManager : MonoBehaviour
{
   public GameObject dialoguePanel;
   public TMP_Text dialogueText;

   private string[] dialogos;
   private int indiceDialogo = 0;

   private bool dialogoAtivo;

   public void IniciarDialogo(string falas)
   {
    indiceDialogo++
    if(indiceDialogo< dialogos.Lenght)
    {
    dialogueText.text = dialogos[indiceDialogo];
    }
    else
    {
        dialoguePanel.SetActive(false);
        dialogoAtivo = false;
    }
   }
    private void Update()
    {
        if(dialogoAtivo && Input.GetKeyDown(KeyCode.Space))
        {
            ProximaFala();
        }
    }

    private void IniciarDialogo(string[]falas)
    {
        dialogos = falas;
        indiceDialogo = 0;
        dialoguePanel.SetActive(true);
        dialogoAtivo = true;
        
        dialogueText.text = falas[0];
    }

    public void ProximaFala()
    {
        indiceDialogo++;
        if(indiceDialogo<dialogos.Lenght)
        {
            dialogueText.text = dialogos[indiceDialogo];
        }
        else
        {
            dialoguePanel.SetActive(false);
            dialogoAtivo = false;
        }
    }




   
}
