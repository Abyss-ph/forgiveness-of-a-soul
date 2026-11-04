using UnityEngine;

public class TSTDAno : MonoBehaviour
{
 public Vida vida;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            vida.ReceberDano(20);
        }
    }
}
