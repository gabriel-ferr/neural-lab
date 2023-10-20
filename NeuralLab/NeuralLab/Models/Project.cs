namespace NeuralLab.Models;

/// <summary>
///     Modelo de um projeto.
///     O projeto é um conceito do sistema que engloba um processamento de dados.
/// </summary>
public class Project
{
    /// <summary>
    ///     Identificação do usuário.
    /// </summary>
    public int ID { get; private set; }

    /// <summary>
    ///     Nome do projeto.
    /// </summary>
    public int Name { get; private set; }

    /// <summary>
    ///     Responsável pelo projeto.
    /// </summary>
    public int Owner { get; private set; }

    /// <summary>
    ///     Conjunto de identificação de um grupo de dados e uma tag de identificação associada.
    /// </summary>
    public Dictionary<int, string> DataTags { get; private set; }

    //  - Conjunto de dados quantitativos associados ao projeto.
    private Dictionary<int, Dataset<Structs.QuantitativeData>> QuantitativeDataset { get; set; }

    //  - Conjunto de dados qualitativos associados ao projeto.
    private Dictionary<int, Dataset<Structs.QualitativeData>> QualitativeDataset { get; set; }
}