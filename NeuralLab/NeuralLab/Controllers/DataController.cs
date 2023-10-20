using Microsoft.AspNetCore.Mvc;

namespace NeuralLab.Controllers;

/// <summary>
///     Controlador para o ambiente de dados.
/// </summary>
[ApiController]
[Route("data")]
[RequestSizeLimit(100_000_000)]
public class DataController : ControllerBase
{
    /// <summary>
    ///     Chamada POST que recebe um arquivo com os dados quantitativos.
    /// </summary>
    /// <returns>Retorna a resposta da requisição.</returns>
    [Route("import-quantitative")]
    [HttpPost]
    [RequestSizeLimit(100_000_000)]
    public IResult ImportQuantitative (IFormFile file)
    {
        try
        {
            //FileStream stream = new FileStream(Path.Combine(Program.PATH, "test.json"), FileMode.Create);
            //file.CopyTo(stream);

            //  - Cria um arquivo temporário.
            KeyValuePair<int, Models.TempFile> tempFile = Program.TempManager.Create();

            //  - Cria um Stream pro arquivo temporário criado.
            FileStream stream = new (tempFile.Value.Path, FileMode.Create);

            //  - Copia o conteúdo do arquivo recebido para o arquivo temporário.
            file.CopyTo(stream);

            //  - Chama o Parser de importação do Airton.
            Structs.DataGroup data = (new Functions.JSONImport.AirtonForm()).ImportFromTemp(tempFile.Key, "FP1,F4,F3,C4,FP2,O2,C3,O1");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Results.BadRequest();
        }
        return Results.Ok();
    }

    #region INTERN
    private readonly ILogger<DataController> _logger;
    public DataController(ILogger<DataController> logger) { _logger = logger; }
    #endregion
}