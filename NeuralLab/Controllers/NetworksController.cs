using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace NeuralLab.Controllers;

public class ListPushReponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

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
        List<ListPushReponse> list = new();
        foreach (Backend.Models.Model model in Backend.Global.NetworkModels)
            list.Add(new ListPushReponse() { Id = model.Id, Name = model.Name });

        return JsonSerializer.Serialize(list);
    }
}