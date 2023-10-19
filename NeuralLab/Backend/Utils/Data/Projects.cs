using System.IO;
using System.Text.Json;

namespace NeuralLab.Backend.Utils.Data;

public static class Projects
{
    //  Path to the 'models.json' file.
    private static readonly string path = Path.Combine(Program.PATH, "projects.json");

    /// <summary>
    ///     Save a new project.
    /// </summary>
    /// <param name="project">Project to save.</param>
    public static void New(Models.Project project)
    {
        Global.Projects.Add(project);

        //  Create the file if it didn't exist.
        if (!File.Exists(path)) File.Create(path).Close();

        //  Compute the JSON file.
        string json = JsonSerializer.Serialize(Global.Projects);

        //  Create the account file.
        File.Create(path).Close();

        //  Write the json content in the file.
        StreamWriter writer = new(path);
        writer.Write(json);
        writer.Flush();
        writer.Close();
    }

    /// <summary>
    ///     Load the network models from 'models.json' file.
    /// </summary>
    public static async Task Load()
    {
        //  If the file didn't exist, creates it.
        if (!File.Exists(path)) return;

        //  Read the file.
        StreamReader reader = new(path);
        string json = await reader.ReadToEndAsync();

        //  Close the stream.
        reader.Close();

        //  Parse the JSON.
        Global.Projects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.Project>>(json) ?? throw new ArgumentNullException(nameof(Global.Accounts), "Falha ao carregar os acessos.");
    }
}