using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace NeuralLab.Controllers;

/// <summary>
///     Module to Login Request.
/// </summary>
public class LoginRequest
{
    public string? Account { get; set; }
    public string? Password { get; set; }
}

/// <summary>
///     Module to Login Response.
/// </summary>
public class LoginResponse
{
    public int Id { get; set; }
}

[ApiController]
[Route("login")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger) { _logger = logger; }

    [HttpPost]
    public string Post(LoginRequest request)
    {
        //  Verify the data.
        if (request.Account == null) return JsonSerializer.Serialize(new RequestError() { Id = -1, Message = "O servidor identificou que o acesso informado é nulo."});
        if (request.Password == null) return JsonSerializer.Serialize(new RequestError() { Id = -1, Message = "O servidor identificou que a senha informada é nula." });

        Console.WriteLine($"Received: {request.Account} ; {request.Password}");

        //  Search from the user access.
        List<Backend.Models.User> accounts = Backend.Global.Accounts.FindAll(x => x.IsIt(request.Account, request.Password));

        //  Verify.
        if (accounts.Count == 0) return JsonSerializer.Serialize(new RequestError() { Id = -1, Message = "O acesso não foi identificado no sistema." });
        else if (accounts.Count > 1) return JsonSerializer.Serialize(new RequestError() { Id = -1, Message = "Uma multiplicidade de acessos foi localizada e o login não pode ser efetuado." });

        //  Send the user ID.
        return JsonSerializer.Serialize(new LoginResponse() { Id = accounts[0].Id });
    }
}