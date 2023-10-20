namespace NeuralLab;

/// <summary>
///     Estrutura para gerenciamento de arquivos temporários.
/// </summary>
public class TempManager
{
    // - Diretório principal dos arquivos temporários.
    private readonly string path;

    //  - Intervalo de verificação dos arquivos temporários.
    private readonly int interval;

    //  - Intervalo para deletar um arquivo não usado.
    private readonly int minutesToDelete;

    /// <summary>
    ///     Lista de arquivos temporários.
    /// </summary>
    private readonly Dictionary<int, Models.TempFile> files;

    /// <summary>
    ///     Lista de diretórios temporários.
    /// </summary>
    private readonly Dictionary<int, Models.TempDirectory> directories;

    /// <summary>
    ///     Apaga o conteúdo de um diretório.
    /// </summary>
    /// <param name="dir">Diretório alvo.</param>
    private void Delete(string dir)
    {
        //  - Pega as pastas do diretório.
        string[] folders = Directory.GetDirectories(dir);
        //  - Chama a função Delete para apagar os sub diretório.
        foreach (string subdir in folders) Delete(subdir);

        //  - Pega os arquivos do diretório.
        string[] files = Directory.GetFiles(dir);
        //  - Deleta os arquivos.
        foreach (string file in files) File.Delete(file);

        //  - Deleta o diretório em si.
        Directory.Delete(dir);
    }

    /// <summary>
    ///     Pega o arquivo temporário associado a uma identificação.
    /// </summary>
    /// <param name="id">ID do arquivo.</param>
    /// <returns>Retorna a estrutura do arquivo.</returns>
    public Models.TempFile GetFile(int id) => files[id];

    /// <summary>
    ///     Cria um novo diretório temporário.
    /// </summary>
    /// <param name="parent">Diretório pai. Use 0 para root.</param>
    /// <returns>Retorna um conjunto com o ID do arquivo e o modelo do arquivo temporário.</returns>
    public KeyValuePair<int, Models.TempDirectory> CreateDir(int parent)
    {
        //  - Chama o GarbageCollector.
        _ = GarbageCollector();

        //  - Gera uma identificação para a pasta.
        Random rnd = new();
        int id;
        do
        {
            id = rnd.Next(1, int.MaxValue);
            if (!files.ContainsKey(id)) break;
        } while (true);

        //  - Define o diretório pai.
        string _path = path;
        if (parent != 0) _path = Path.Combine(path, directories[parent].Path);

        //  - Cria o diretório.
        directories.Add(id, new Models.TempDirectory(Path.Combine(_path, id.ToString())));
        if (Directory.Exists(directories[id].Path)) Delete(directories[id].Path);
        Directory.CreateDirectory(directories[id].Path);

        //  - Retorna o resutlado.
        return new KeyValuePair<int, Models.TempDirectory>(id, directories[id]);
    }

    /// <summary>
    ///     Cria um novo arquivo temporário.
    /// </summary>
    /// <returns>Retorna um conjunto com o ID do arquivo e o modelo do arquivo temporário.</returns>
    public KeyValuePair<int, Models.TempFile> Create()
    {
        //  - Chama o GarbageCollector.
        _ = GarbageCollector();

        //  - Gera uma identificação para o arquivo.
        Random rnd = new();
        int id;
        do
        {
            id = rnd.Next(1, int.MaxValue);
            if (!files.ContainsKey(id)) break;
        } while (true);

        //  - Cria o arquivo.
        files.Add(id, new Models.TempFile(Path.Combine(path, $"{id}.temp")));
        return new KeyValuePair<int, Models.TempFile>(id, files[id]);
    }

    /// <summary>
    ///     Cria um novo arquivo temporário.
    /// </summary>
    /// <param name="dirId">ID do diretório temporário.</param>
    /// <returns>Retorna um conjunto com o ID do arquivo e o modelo do arquivo temporário.</returns>
    public KeyValuePair<int, Models.TempFile> Create(int dirId)
    {
        //  - Chama o GarbageCollector.
        _ = GarbageCollector();

        //  - Gera uma identificação para o arquivo.
        Random rnd = new();
        int id;
        do
        {
            id = rnd.Next(1, int.MaxValue);
            if (!files.ContainsKey(id)) break;
        } while (true);

        //  - Cria o arquivo.
        files.Add(id, new Models.TempFile(Path.Combine(directories[dirId].Path, $"{id}.temp")));
        return new KeyValuePair<int, Models.TempFile>(id, files[id]);
    }

    /// <summary>
    ///     Remove arquivos não utilizados.
    /// </summary>
    /// <returns>Retorna uma tarefa não finalizável, em outras palavras, a função é um Callback infinito.</returns>
    public Task GarbageCollector ()
    {
        //  - Momento atual.
        DateTime now = DateTime.Now.ToUniversalTime();

        //  - Lista os arquivos.
        foreach (KeyValuePair<int, Models.TempFile> file in files)
        {
            //  - Verifica se o arquivo foi usado no limite do tempo estabelecido ou não.
            if (file.Value.LastUse.AddMinutes(minutesToDelete).Ticks > now.Ticks)
            {
                try
                {
                    //  - Deleta o arquivo.
                    files[file.Key].Delete();
                    //  - Remove o arquivo da lista de acesso.
                    files.Remove(file.Key);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Falha no Garbage Collector: ", e.Message);
                    continue;
                }
            }
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Intância um novo gerenciador.
    /// </summary>
    /// <param name="interval">Intervalo entre verificações do tempo de inatividade dos arquivos. Valor em segundos.</param>
    /// <param name="minutesToDelete">Quantidade de tempo para ele deletar um arquivo não usado. Valor em minutos.</param>
    public TempManager (int interval, int minutesToDelete)
    {
        //  - Define o intervalo de verificação dos arquivos.

        //  - Inicializa a lista de arquivos temporários.
        files = new Dictionary<int, Models.TempFile>();
        directories = new Dictionary<int, Models.TempDirectory>();
        //  - Define o diretório.
        path = Path.Combine(Program.PATH, "temp");

        //  - Verifica se a pasta temporária não existe.
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        else
        {
            //  - Se o diretório existir, apaga ele.
            Delete(path);
            //  - Cria um novo diretório.
            Directory.CreateDirectory(path);
        }
    }
}