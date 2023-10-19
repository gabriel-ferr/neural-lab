#nullable enable
namespace NeuralLab.Backend.Models;

/// <summary>
///     Modelo para um usuário do sistema.
/// </summary>
public struct User
{
    public int Id { get; private set; }

    public string Account { get; private set; }
    public string Password { get; private set; }

    public string Name { get; private set; }

    public List<Enums.Permissions> Permissions { get; private set; }
    public List<int>? Projects { get; private set; }

    /// <summary>
    ///     Verify the info.
    /// </summary>
    /// <param name="account">Account to compare.</param>
    /// <param name="password">Password to compare.</param>
    /// <returns>Return comparate result.</returns>
    public readonly bool IsIt(string account, string password)
    {
        //  Compute the info.
        string _account = Utils.Cryptography.ComputeHash(account);
        string _password = Utils.Cryptography.ComputeHash(password);

        //  Verify.
        Console.WriteLine($"A: {(Account == _account)}\nP: {(Password == _password)}");

        //  Verify.
        return (Account == _account) && (Password == _password);
    }


    public void AddProject (int id)
    {
        Projects ??= new List<int>();
        Projects.Add(id);
    }

    public void Compute()
    {
        Account = Utils.Cryptography.ComputeHash(Account);
        Password = Utils.Cryptography.ComputeHash(Password);
    }

    public User(int Id, string Account, string Password, string Name, List<Enums.Permissions> Permissions, List<int>? Projects)
    {
        this.Id = Id;
        this.Account = Account;
        this.Password = Password;
        this.Name = Name;
        this.Permissions = Permissions;
        this.Projects = Projects;
    }
}