namespace NeuralLab.Structs;

public struct Dataset
{
    public string id { get; set; }
    public string color { get; set; }

    public List<Pair<float, float>> data { get; set; }
}