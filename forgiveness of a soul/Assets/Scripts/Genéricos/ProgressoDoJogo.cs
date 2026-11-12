using System.Collections.Generic;

// Guarda "marcas" do que o jogador já fez (ex.: "missaoSapo").
// É estática: não precisa colocar em nenhum objeto e sobrevive à troca de cena.
public static class ProgressoDoJogo
{
    private static readonly HashSet<string> marcas = new HashSet<string>();

    // Anota que algo aconteceu. Nome vazio é ignorado.
    public static void Marcar(string marca)
    {
        if (!string.IsNullOrEmpty(marca))
        {
            marcas.Add(marca);
        }
    }

    // Verifica se algo já aconteceu. Se nenhum nome for exigido (vazio), retorna true.
    public static bool Tem(string marca)
    {
        return string.IsNullOrEmpty(marca) || marcas.Contains(marca);
    }

    // Apaga tudo (use ao começar um jogo novo)
    public static void Resetar()
    {
        marcas.Clear();
    }
}
