using UnityEngine;
using UnityEngine.InputSystem

public class NpcInteracao : MonoBehaviour
{
    public GameObject interactionUI;
    
    private bool podeInteragir = false

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
            
        }
    }

} 


