using System;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    // Permite acessar o gerenciador de qualquer script: QuestManager.Instance
    public static QuestManager Instance { get; private set; }

    [Header("UI do painel de missão")]
    [SerializeField] private GameObject painelMissao;
    [SerializeField] private TMP_Text textoTitulo;
    [SerializeField] private TMP_Text textoObjetivos;

    private Missao missaoAtual;
    private int[] progresso;

    public bool MissaoConcluida { get; private set; }

    // NOVO: true enquanto houver uma missão em andamento (usado para não aceitar outra por cima)
    public bool TemMissaoAtiva => missaoAtual != null;

    // NOVO: disparado uma única vez, no momento em que todos os objetivos ficam completos
    public event Action OnMissaoConcluida;

    private void Awake()
    {
        Instance = this;
        painelMissao.SetActive(false);
    }

    // Chamado pelo NPC depois do diálogo
    public void IniciarMissao(Missao missao)
    {
        missaoAtual = missao;
        progresso = new int[missao.objetivos.Length];
        MissaoConcluida = false;

        painelMissao.SetActive(true);
        AtualizarUI();
    }

    // Chamado pelos inimigos (e futuramente por itens, NPCs etc.)
    public void RegistrarProgresso(string idAlvo)
    {
        if (missaoAtual == null || MissaoConcluida)
            return;

        for (int i = 0; i < missaoAtual.objetivos.Length; i++)
        {
            Objetivo objetivo = missaoAtual.objetivos[i];

            if (objetivo.idAlvo == idAlvo && progresso[i] < objetivo.quantidadeNecessaria)
            {
                progresso[i]++;
            }
        }

        MissaoConcluida = VerificarConclusao();
        AtualizarUI();

        // NOVO: avisa quem estiver ouvindo (ex.: IntroBossController)
        if (MissaoConcluida)
        {
            OnMissaoConcluida?.Invoke();
        }
    }

    // Chamado pelo NPC quando o jogador entrega a missão concluída
    public void EncerrarMissao()
    {
        missaoAtual = null;
        progresso = null;
        MissaoConcluida = false;
        painelMissao.SetActive(false);
    }

    private bool VerificarConclusao()
    {
        for (int i = 0; i < missaoAtual.objetivos.Length; i++)
        {
            if (progresso[i] < missaoAtual.objetivos[i].quantidadeNecessaria)
                return false;
        }

        return true;
    }

    private void AtualizarUI()
    {
        textoTitulo.text = missaoAtual.titulo;

        string texto = "";

        for (int i = 0; i < missaoAtual.objetivos.Length; i++)
        {
            Objetivo objetivo = missaoAtual.objetivos[i];

            string linha = objetivo.descricao + " (" + progresso[i] + "/" + objetivo.quantidadeNecessaria + ")";

            // Objetivo completo fica verde
            if (progresso[i] >= objetivo.quantidadeNecessaria)
            {
                linha = "<color=#7CFC00>" + linha + "</color>";
            }

            texto += "- " + linha + "\n";
        }

        if (MissaoConcluida)
        {
            texto += "\nMissão concluída! Fale com o NPC.";
        }

        textoObjetivos.text = texto;
    }
}