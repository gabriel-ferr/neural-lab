using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace NeuralLab.Controllers;

public class CreateNetworkRequest
{
    public string? Name { get; set; }
    public int NetId { get; set; }
    public int Owner { get; set; }
}

public class LoadProjectResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public int OwnerId { get; set; }
    public string? OwnerName { get; set; }

    public string? NetworkName { get; set; }
    public float NetworkAccuracy { get; set; }

    public bool CanCreateProject { get; set; }
    public bool CanDeleteOthersProjects { get; set; }
}

[ApiController]
[Route("project")]
public class ProjectController : ControllerBase
{
    private readonly ILogger<ProjectController> _logger;

    public ProjectController(ILogger<ProjectController> logger) { _logger = logger; }

    [Route("create")]
    [HttpPost]
    public string Create(CreateNetworkRequest request)
    {
        if (request.Name == null) return JsonSerializer.Serialize(new RequestError() { Id = -2, Message = "O servidor identificou que o nome do projeto é nulo." });

        List<Backend.Models.Model> models = Backend.Global.NetworkModels.FindAll(x => x.Id == request.NetId);
        List<Backend.Models.User> accounts = Backend.Global.Accounts.FindAll(x => x.Id == request.Owner);
        List<Backend.Models.Project> projects = Backend.Global.Projects.FindAll(x => x.Name == request.Name);

        if (models.Count == 0) return JsonSerializer.Serialize(new RequestError() { Id = -2, Message = "A rede escolhida não foi identificada no sistema." });
        else if (models.Count > 1) return JsonSerializer.Serialize(new RequestError() { Id = -2, Message = "Uma multiplicidade da rede escolhida foi localizada e o projeto não pode ser criado com ela." });
        if (accounts.Count == 0) return JsonSerializer.Serialize(new RequestError() { Id = -2, Message = "O usuário não foi identificada no sistema." });
        else if (accounts.Count > 1) return JsonSerializer.Serialize(new RequestError() { Id = -2, Message = "Uma multiplicade de usuários de mesmo nome foi identificado, por favor, refaça o login." });
        if (projects.Count > 0) return JsonSerializer.Serialize(new RequestError() { Id = -2, Message = "Já existe um projeto com esse nome." });

        if (!accounts[0].Permissions.Contains(Backend.Enums.Permissions.CreateProject)) return JsonSerializer.Serialize(new RequestError() { Id = -2, Message = "Você não tem a autorização para criar um projeto." });

        Random rnd = new ();
        int id = 0;
        do
        {
            id = rnd.Next(1, int.MaxValue);
            int count = Backend.Global.Projects.FindAll(x => x.Id == id).Count;
            if (count == 0) break;
        } while (true);

        Backend.Models.Project project = new (id, request.Name, request.Owner, request.NetId);
        Backend.Utils.Data.Projects.New(project);

        accounts[0].AddProject(id);

        return JsonSerializer.Serialize(new LoadProjectResponse() { Id = id, Name = project.Name, NetworkName = models[0].Name, NetworkAccuracy = models[0].Accuracy, OwnerId = accounts[0].Id, OwnerName = accounts[0].Name });
    }
}