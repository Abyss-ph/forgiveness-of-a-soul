using UnityEngine;

public class Controlsplayerfight1 : MonoBehaviour
{
    [SerializeField] private Projetil projetilPrefab;
    [SerializeField] private Transform pontoDeDisparo; // opcional: se vazio, atira do centro do jogador
    [SerializeField] private float intervaloEntreTiros = 0.4f;

    private Camera cameraPrincipal;
    private float proximoTiro;

    void Awake()
    {
        cameraPrincipal = Camera.main;
    }

    void Update()
    {
        // Jogo pausado (ex.: tela de Game Over): não atira
        if (Time.timeScale == 0f) return;

        // Botão esquerdo do mouse (segurar para atirar continuamente)
        if (Input.GetMouseButton(0) && Time.time >= proximoTiro)
        {
            Atirar();
            proximoTiro = Time.time + intervaloEntreTiros;
        }

        if (dialogueManager.EmDialogo) return;
    }

    private void Atirar()
    {
        if (projetilPrefab == null || cameraPrincipal == null) return;

        // Posição do mouse no mundo do jogo
        Vector3 posicaoMouse = Input.mousePosition;
        posicaoMouse.z = Mathf.Abs(cameraPrincipal.transform.position.z);
        posicaoMouse = cameraPrincipal.ScreenToWorldPoint(posicaoMouse);

        Vector3 origem = pontoDeDisparo != null ? pontoDeDisparo.position : transform.position;

        Vector2 direcao = posicaoMouse - origem;
        if (direcao.sqrMagnitude < 0.01f) return; // mouse em cima do jogador

        Projetil novoProjetil = Instantiate(projetilPrefab, origem, Quaternion.identity);
        novoProjetil.Iniciar(direcao);
    }
}