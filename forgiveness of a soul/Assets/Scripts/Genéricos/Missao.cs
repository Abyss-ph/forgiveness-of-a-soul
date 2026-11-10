// Dados de uma missão. São classes simples que aparecem no Inspector do NPC.

[System.Serializable]
public class Objetivo
{
    public string idAlvo;                 // ex.: "Slime" (igual ao "Id Inimigo" do inimigo)
    public string descricao;              // ex.: "Derrotar slimes"
    public int quantidadeNecessaria = 1;  // ex.: 5
}

[System.Serializable]
public class Missao
{
    public string titulo;
    public Objetivo[] objetivos;
}