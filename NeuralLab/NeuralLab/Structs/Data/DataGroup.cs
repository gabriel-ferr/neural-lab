namespace NeuralLab.Structs;

/// <summary>
///     Estrutura para um arquivo de dados.
/// </summary>
public struct DataGroup
{
    /// <summary>
    ///     Identificação de referência para o indivíduo que realizou a pesquisa.
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    ///     Relação entre uma tarefa e o arquivo referente a ela.
    /// </summary>
    public Dictionary<int, int> Tasks { get; set; }

    /// <summary>
    ///     Diretório temporário onde os dados estão localizados.
    /// </summary>
    public int Directory { get; set; }
}