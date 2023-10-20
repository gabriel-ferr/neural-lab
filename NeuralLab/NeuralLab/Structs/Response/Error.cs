namespace NeuralLab.Structs.Response;

/// <summary>
///     Reposta de erro do servidor.
/// </summary>
public struct Error
{
    public int ID { get; set; }
    public string Message { get; set; }
}