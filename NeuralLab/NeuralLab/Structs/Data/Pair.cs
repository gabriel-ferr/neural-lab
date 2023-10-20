namespace NeuralLab.Structs;

/// <summary>
///     Par de valores de tipo X e Y.
/// </summary>
/// <typeparam name="X">Tipo do elemento x.</typeparam>
/// <typeparam name="Y">Tipo do elemento y.</typeparam>
public struct Pair<X, Y>
{
    public X x { get; set; }
    public Y y { get; set; }
}