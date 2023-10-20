namespace NeuralLab.Structs;

/// <summary>
///     Série de dados nomeada com pares de valores.
/// </summary>
/// <typeparam name="X">Tipo do elemento x.</typeparam>
/// <typeparam name="Y">Tipo do elemento y.</typeparam>
public struct PairData<X, Y>
{
    public string Label { get; set; }
    public List<Pair<X, Y>> Dataset { get; set; }
}