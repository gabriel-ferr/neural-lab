using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace NeuralLab.Controllers;

[ApiController]
[Route("networks")]
public class NetworksController : ControllerBase
{
    private readonly ILogger<NetworksController> _logger;

    public NetworksController(ILogger<NetworksController> logger) { _logger = logger; }

    [Route("push")]
    [HttpGet]
    public string Push()
    {
        return JsonSerializer.Serialize(Backend.Global.NetworkModels);
    }
}