using UnityEngine;

public class MainGame : MonoBehaviour
{
  // fazer eu escolher o nome do personagem, no text chamado EscolhaSeuNome.

    namePlayer = GetNomeDoPersonagem();
  public void EscolherNome(string nome)
  {
    PlayerPrefs.SetString("NomeDoPersonagem", nome);
  }

  public string GetNomeDoPersonagem()
  {
    return PlayerPrefs.GetString("NomeDoPersonagem", "Jogador");
  }
}
