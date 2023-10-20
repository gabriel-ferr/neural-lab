namespace NeuralLab.Models;

/// <summary>
///     Estrutura para gerenciamento dos resultados dos processos externos.
/// </summary>
/// <typeparam name="T">Estrutura de modelagem dos resultados.</typeparam>
public class Result<T>
{
    /// <summary>
    ///     Identificação do resultado no sistema.
    /// </summary>
    public int ID { get; private set; }

    /// <summary>
    ///     Projeto associado ao resultado.
    /// </summary>
    public int Project { get; private set; }

    /// <summary>
    ///     Tipo de arquivo de resultado emitido pela estrutura do Julia.
    /// </summary>
    public Enums.ResultType Type { get; private set; }

    /// <summary>
    ///     Interface de estruturação dos resultados.
    /// </summary>
    public Interfaces.IResult<T> Struct { get; private set; }

    /// <summary>
    ///     Instância um novo resultado.
    /// </summary>
    /// <param name="id">Identificação do resultado.</param>
    /// <param name="project">Identificação do projeto ao qual o resultado está associado.</param>
    /// <param name="type">Tipo de projeto.</param>
    public Result(int id, int project, Enums.ResultType type)
    {
        ID = id;
        Project = project;
        Type = type;

        switch (type)
        {
            case Enums.ResultType.TemporalData: break;
            default: throw new Exceptions.ServerException(101, "Não há uma estrutura definida para o tipo de resultado requisitado.");
        }
    }
}