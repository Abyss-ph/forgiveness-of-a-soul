using UnityEngine;

public class Câmera : MonoBehaviour
{
    // câmera do jogador
    [SerializeField] private Transform jogador;
    
    void LateUpdate()
    {
        if (jogador !=null)
        {
            Vector3 posicaoCamera = new Vector3(jogador.position.x, jogador.position.y, transform.position.z);
            transform.position = posicaoCamera;

        }
        
    }
}
