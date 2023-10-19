namespace NeuralLab.Backend;

/// <summary>
///     Static and global elements from Backend.
/// </summary>
public static class Global
{
    /// <summary>
    ///     List with the system access accounts.
    /// </summary>
    public static List<Models.User> Accounts = new();

    /// <summary>
    ///     List with the network models.
    /// </summary>
    public static List<Models.Model> NetworkModels = new();

    /// <summary>
    ///     List with the projects.
    /// </summary>
    public static List<Models.Project> Projects = new();
}