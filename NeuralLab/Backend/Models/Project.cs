namespace NeuralLab.Backend.Models;

/// <summary>
///     Model to the a Project.
/// </summary>
public struct Project
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public int Owner { get; private set; }

    public int Model { get; private set; }

    public Project (int Id, string Name, int Owner, int Model)
    {
        this.Id = Id;
        this.Name = Name;
        this.Owner = Owner;
        this.Model = Model;
    }
}