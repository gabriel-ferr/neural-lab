using Microsoft.AspNetCore.Mvc;
using NeuralLab.Structs;
using System.Diagnostics;

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
    public string ImportQuantitative (IFormFile file)
    {
        try
        {
            //  - Cria um arquivo temporário.
            KeyValuePair<int, Models.TempFile> tempFile = Program.TempManager.Create();

            //  - Cria um Stream pro arquivo temporário criado.
            FileStream stream = new (tempFile.Value.Path, FileMode.Create);

            //  - Copia o conteúdo do arquivo recebido para o arquivo temporário.
            file.CopyTo(stream);

            //  - Chama o Parser de importação do Airton.
            Structs.DataGroup data = (new Functions.JSONImport.AirtonForm()).ImportFromTemp(tempFile.Key, "FP1,F4,F3,C4,FP2,O2,C3,O1");

            //  - Deleta o arquivo recebido.
            Program.TempManager.DeleteFile(tempFile.Key);

            //  - Processa os dados tarefa por tarefa.
            //  Key: Tarefa aplicada.
            //  Value: Arquivo temporário associado.
            foreach (KeyValuePair<int, int> task in data.Tasks)
            {
                //  - Ignora o basal.
                if (task.Key == 1) continue;

                //  - Cria uma pasta temporária para os arquivos temporários processados.
                KeyValuePair<int, Models.TempDirectory> temp = Program.TempManager.CreateDir(data.Directory);

                //  - Abre um processo do Julia para filtrar os dados da tarefa.
                Process process = Process.Start("julia", $"{Path.Combine(Program.PATH, "networks", "0", "import.jl")} {Program.TempManager.GetFile(task.Value).Path} {temp.Value.Path}");
                process.WaitForExit();

                //  - Pega os arquivos da pasta.
                string[] generatedFiles = Directory.GetFiles(temp.Value.Path);

                Dictionary<int, List<Structs.Dataset>> channels = new ();

                Console.WriteLine($"Working with task {task.Key} in {temp.Value.Path} ");

                //  - Processa os arquivos gerados.
                foreach (string f in generatedFiles)
                {
                    //  - Pega o nome do arquivo para extrair a informação sobre canal e espectro de onda.
                    string[] info = Path.GetFileName(f).Replace(".temp", "").Split('_');

                    //  Número do canal.
                    int channel = Convert.ToInt32(info[0]);
                    //  ID da onda (1 -> Alpha; 2 -> Beta; 3 -> Gamma)
                    int waveId = Convert.ToInt32(info[1]);

                    //  - Cria o conjunto de dados.
                    Structs.Dataset dataset = new Structs.Dataset();
                    dataset.data = new List<Pair<float, float>>();

                    if (waveId == 1) { dataset.id = "Alpha"; dataset.color = "#FF003C"; }
                    else if (waveId == 2) { dataset.id = "Beta"; dataset.color = "#FABE28"; }
                    else { dataset.id = "Gamma"; dataset.color = "#00C176"; }

                    //  - Registra o arquivo como um arquivo temporário.
                    int fileId = Program.TempManager.Import(f);

                    //  - Se o canal ainda não tiver sido registrado.
                    if (!channels.ContainsKey(channel))
                        //  - Instância o canal.
                        channels.Add(channel, new List<Structs.Dataset>());

                    //  - Lê o conteúdo do arquivo.
                    StreamReader reader = new (f);
                    string[] content = reader.ReadToEnd().Split('\n');
                    reader.Close();

                    //  - Pega os dados.
                    for (int i = 1; i < content.Length; i++)
                    {
                        if (string.IsNullOrEmpty(content[i]) || string.IsNullOrWhiteSpace(content[i])) continue;
                        string[] lineInfo = content[i].Split(',');
                        dataset.data.Add(new Pair<float, float>() { x = Convert.ToSingle(lineInfo[0].Trim().Replace(".", ",")), y = Convert.ToSingle(lineInfo[1].Trim().Replace(".", ",")) });
                    }

                    Console.WriteLine($"Allocing {dataset.id} from channel {channel} in list index {channels[channel].Count} ");

                    //  - Adiciona o dataset.
                    channels[channel].Add(dataset);

                    //  - Deleta o arquivo temporário.
                    //Program.TempManager.DeleteFile(fileId);
                }

                //  - Deleta o arquivo de origem.
                //Program.TempManager.DeleteFile(task.Value);

                //  - Salva o JSON para validação.
                string ffi = Path.Combine(Program.PATH, "test.json");

                if (!System.IO.File.Exists(ffi)) System.IO.File.Create(ffi).Close();
                StreamWriter w = new StreamWriter(ffi);
                w.Write(System.Text.Json.JsonSerializer.Serialize(channels[1]));
                w.Flush();
                w.Close();

                //  - Retorna o JSON de um canal.
                return System.Text.Json.JsonSerializer.Serialize(channels[1]);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            //return Results.BadRequest();
        }
        return System.Text.Json.JsonSerializer.Serialize(new Structs.Response.Error() { ID = 0, Message = "Sei lá" });
    }

    #region INTERN
    private readonly ILogger<DataController> _logger;
    public DataController(ILogger<DataController> logger) { _logger = logger; }
    #endregion
}