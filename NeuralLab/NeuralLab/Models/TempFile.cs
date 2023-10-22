namespace NeuralLab.Models;

/// <summary>
///     Arquivo temporário do sistema.
/// </summary>
public class TempFile
{
    /// <summary>
    ///     Caminho até o arquivo.
    /// </summary>
    public string Path { get; private set; }

    /// <summary>
    ///     Última vez que o arquivo foi acessado.
    /// </summary>
    public DateTime CreateTime { get; private set; }

    /// <summary>
    ///     Instância um novo arquivo temporário.
    /// </summary>
    /// <param name="path">Diretório do arquivo.</param>
    public TempFile(string path)
    {
        Path = path;
        CreateTime = DateTime.Now.ToUniversalTime();
    }

}