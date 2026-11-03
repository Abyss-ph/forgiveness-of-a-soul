using UnityEngine;
using UnityEngine.InputSystem;

public class NpcInteracao : MonoBehaviour
{
    public string[] falas;
    public GameObject interactionUI;
    // Referência ao DialogueManager
    public dialogueManager dialogueManager;
    private bool podeInteragir = false;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
        podeInteragir = true;
        interactionUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
        podeInteragir = false;
        interactionUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (podeInteragir && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Iniciando diálogo com NPC");
            dialogueManager.IniciarDialogo(falas);
            interactionUI.SetActive(false); // Desativa a UI de interação quando o diálogo começa
        }
    }

} 


