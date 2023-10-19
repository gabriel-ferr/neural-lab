using System.Text.Json;

namespace NeuralLab.Backend.Utils.Data;

/// <summary>
///     Manage the accounts.
/// </summary>
public static class Accounts
{
    //  Path to the 'accounts.json' file.
    private static readonly string path = Path.Combine(Program.PATH, "accounts.json");


    //  - Creates a default 'accounts.json' file.
    private static Task Create()
    {
        //  Create the user.
        List<Models.User> accounts = new();

        Models.User user = new(0, "admin", "admin", "admin", new List<Enums.Permissions>() {
            Enums.Permissions.CreateProject,
            Enums.Permissions.DeleteOthersProjects
        }, null);

        user.Compute();
        accounts.Add(user);

        //  Compute the JSON file.
        string json = JsonSerializer.Serialize(accounts);

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
    ///     Load the accounts from 'accounts.json' file.
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
        Global.Accounts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.User>>(json) ?? throw new ArgumentNullException(nameof(Global.Accounts), "Falha ao carregar os acessos.");
    }
}