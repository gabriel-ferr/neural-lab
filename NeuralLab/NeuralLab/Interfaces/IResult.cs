namespace NeuralLab.Interfaces;

/// <summary>
///     Interface para a construção de uma estrutura de resultado
///     que será inserida no sistema.
/// </summary>
/// <typeparam name="T">Estrutura de modelagem dos resultados.</typeparam>
public interface IResult<T>
{
    /// <summary>
    ///     Carrega o resultado retornando um conjunto de dados.
    /// </summary>
    /// <returns>Retorna um vetor de resultados.</returns>
    public T[,]? Load(Models.Result<T> result, params object[] args);
}