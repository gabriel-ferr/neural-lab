namespace NeuralLab.Models;

/// <summary>
///     Diretório temporário do sistema.
/// </summary>
public class TempDirectory
{
    /// <summary>
    ///     Caminho até o arquivo.
    /// </summary>
    public string Path { get; private set; }

    /// <summary>
    ///     Última vez que o arquivo foi acessado.
    /// </summary>
    public DateTime LastUse { get; private set; }

    /// <summary>
    ///     Instância um novo arquivo temporário.
    /// </summary>
    /// <param name="path">Diretório do arquivo.</param>
    public TempDirectory(string path)
    {
        Path = path;

        LastUse = DateTime.Now.ToUniversalTime();
    }

}