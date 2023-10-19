namespace NeuralLab.Backend.Models;

/// <summary>
///     Model to the a Network Model.
/// </summary>
public struct Model
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public float Accuracy { get; private set; }

    public DateTime LastTrain { get; private set; }

    public Model (int Id, string Name, float Accuracy, DateTime LastTrain)
    {
        this.Id = Id;
        this.Name = Name;
        this.Accuracy = Accuracy;
        this.LastTrain = LastTrain;
    }
}