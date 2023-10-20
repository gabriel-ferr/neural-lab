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
    public DateTime LastUse { get; private set; }

    /// <summary>
    ///     Deleta o arquivo.
    /// </summary>
    public void Delete ()
    {
        if (!File.Exists(Path)) throw new Exceptions.ServerException(204, "O arquivo não existe.");
        File.Delete(Path);
    }

    /// <summary>
    ///     Instância um novo arquivo temporário.
    /// </summary>
    /// <param name="path">Diretório do arquivo.</param>
    public TempFile(string path)
    {
        Path = path;
        LastUse = DateTime.Now.ToUniversalTime();
    }

}