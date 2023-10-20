namespace NeuralLab.Exceptions;

/// <summary>
///     Exceção generalizada do servidor.
/// </summary>
public class ServerException : Exception
{
    //  - Código de erro.
    private readonly int code;

    /// <summary>
    ///     Pega o código de erro associado.
    /// </summary>
    public int Code { get => code; }

    /// <summary>
    ///     Instância uma nova exceção do servidor.
    /// </summary>
    /// <param name="code">Código de erro.</param>
    /// <param name="message">Mensagem de erro.</param>
    public ServerException (int code, string message) : base (message)
    {
        //  O erro deve ser um número negativo!
        this.code = code > 0 ? code * (-1) : code;
    }
}