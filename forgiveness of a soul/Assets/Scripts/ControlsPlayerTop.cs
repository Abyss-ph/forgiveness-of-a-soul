using UnityEngine;

public class ControlsPlayerTop : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    
    void Update()
    {
        bool leftPressed= Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool upPressed = Input.GetKey(KeyCode.W);
        bool downPressed = Input.GetKey(KeyCode.S);

        float horizontal;        


        if (leftPressed && rightPressed)
        {
            horizontal = 0f;
        }
        else if (leftPressed)
        {
            horizontal = -1f;
        }
        else if (rightPressed)
        {
            horizontal = 1f;
        }
        else
        {
            horizontal = 0f;
        }

        float vertical;
       
       if (upPressed && downPressed)
        {
            vertical = 0f;
        }
        else if (upPressed)
        {
            vertical = 1f;
        }
        else if (downPressed)
        {
            vertical = -1f;
        }
        else
        {
            vertical = 0f;
        }

        // Combinando eixo vertical e horizontal para criar um vetor de movimento
        Vector3 direction = new Vector3(horizontal, vertical, 0f);

        direction = direction.normalized; // Normaliza o vetor para evitar movimento mais rápido na diagonal

        // aplicar
        transform.position += direction * speed * Time.deltaTime;
    }
    
    
}
