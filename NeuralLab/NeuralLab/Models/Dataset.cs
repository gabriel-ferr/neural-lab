namespace NeuralLab.Models;

/// <summary>
///     Estrutura de um conjunto de dados em trânsito no sistema.
/// </summary>
public class Dataset<T>
{
    /// <summary>
    ///     Identificação do conjunto de dados no sistema.
    /// </summary>
    public int ID { get; private set; }

    /// <summary>
    ///     Identificação do projeto.
    /// </summary>
    public int Project { get; private set; }

    /// <summary>
    ///     Estrutura dos dados inseridos dentro do dataset.
    /// </summary>
    public Interfaces.IData Data { get; private set; }

    /// <summary>
    ///     Estrutura de dado.
    /// </summary>
    public T Struct { get; private set; }
}