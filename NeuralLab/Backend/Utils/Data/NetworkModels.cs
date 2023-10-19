using System.IO;
using System.Text.Json;

namespace NeuralLab.Backend.Utils.Data;

public static class NetworkModels
{
    //  Path to the 'models.json' file.
    private static readonly string path = Path.Combine(Program.PATH, "models.json");

    //  - Creates a default 'models.json' file.
    private static Task Create()
    {
        //  Create the user.
        List<Models.Model> models = new();

        Models.Model model = new(1, "Julia CNN Test", 0, DateTime.MinValue);

        models.Add(model);

        //  Compute the JSON file.
        string json = JsonSerializer.Serialize(models);

        //  Create the account file.
        File.Create(path).Close();

        //  Write the json content in the file.
        StreamWriter writer = new(path);
        writer.Write(json);
        writer.Flush();
        writer.Close();

        //  Return.
        return Task.CompletedTask;
    }
    /// <summary>
    ///     Load the network models from 'models.json' file.
    /// </summary>
    public static async Task Load()
    {
        //  If the file didn't exist, creates it.
        if (!File.Exists(path)) await Create();

        //  Read the file.
        StreamReader reader = new(path);
        string json = await reader.ReadToEndAsync();

        //  Close the stream.
        reader.Close();

        //  Parse the JSON.
        Global.NetworkModels = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.Model>>(json) ?? throw new ArgumentNullException(nameof(Global.Accounts), "Falha ao carregar os acessos.");
    }
}