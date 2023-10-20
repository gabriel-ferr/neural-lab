namespace NeuralLab.Enums;

/// <summary>
///     Tipos de resultados previstos a serem emitidos pelos processos
///     do Julia.
/// </summary>
public enum ResultType : int
{
    TemporalData = 0,
    Classification = 1,
    ActivationIntensity = 2
}