using System.Text;

namespace NeuralLab.Functions.JSONImport;

/// <summary>
///     Importa um arquivo JSON de saída do programa do Airton.
/// </summary>
public class AirtonForm
{
    /// <summary>
    ///     Importa os dados a partir de um arquivo temporário.
    /// </summary>
    /// <param name="fileId">ID do arquivo temporário.</param>
    /// <returns>Retorna uma lista de</returns>
    public Structs.DataGroup ImportFromTemp(int fileId, string labels)
    {
        //  - Pega o arquivo temporário.
        Models.TempFile file = Program.TempManager.GetFile(fileId);

        //  - Lê o arquivo.
        StreamReader reader = new(file.Path);
        string content = reader.ReadToEnd(); reader.Close();

        //  - Quebra o texto do arquivo para estruturar os blocos de dados.
        string[] rawBlocks = content.Replace("{", "&").Replace("}", "&").Replace("\'", "").Split("&");
        List<string> blocks = new();
        foreach (string blockContent in rawBlocks)
        {
            if (string.IsNullOrEmpty(blockContent) || string.IsNullOrWhiteSpace(blockContent)) continue;
            blocks.Add(blockContent);
        }

        //  - Pega a identificação do indivíduo que realizou a pesquisa.
        int id = Convert.ToInt32(blocks[0].Split(',')[0].Split(':')[1].Replace("\"", ""));

        //  - Cria a pasta temporária para salvar os dados.
        int directory = Program.TempManager.CreateDir(0).Key;

        //  - Cria um novo grupo de dados.
        Structs.DataGroup dataset = new()
        {
            ID = id,
            Directory = directory,
            Tasks = new Dictionary<int, int>()
        };

        //  - Pega os dados para cada tarefa.
        List<string> rawData = new();
        int index = 2;
        while (index < blocks.Count())
        {
            //  - Separa os conjuntos de dados para cada canal.
            string[] data = blocks[index].Split(':')[10].Replace("\",", "\"&").Split('&');
            Dictionary<int, List<string>> channels = new();
            int _rows = 1;
            int channelIndex = 1;
            foreach (string dataContent in data)
            {
                //  Pega o conteúdo do canal.
                List<string> channelContent = new();
                string[] _raw = dataContent.Replace("\"", "").Replace("[", "").Replace("]", "").Split('_');
                foreach (string _rawContent in _raw)
                    channelContent.Add(_rawContent.Replace("\n", "").Replace(" ", "").Replace(",", "."));
                _rows = channelContent.Count();

                //  Adiciona na lista de canais.
                channels.Add(channelIndex, channelContent);
                channelIndex++;
            }

            //  - Cria o arquivo onde os dados serão salvos.
            KeyValuePair<int, Models.TempFile> dataFile = Program.TempManager.Create(directory);
            File.Create(dataFile.Value.Path).Close();

            //  - Associa o arquivo a tarefa.
            dataset.Tasks.Add(index / 2, dataFile.Key);

            //  - Cria um streamer para colocar os dados no arquivo.
            StreamWriter writer = new StreamWriter(dataFile.Value.Path);

            //  Constroí o conteúdo do arquivo.
            //  ESTRUTURA PARA APENAS 8 CANAIS !! É NECESSÁRIO MUDAR ESSA STRING SE TIVER MAIS.
            StringBuilder fileContent = new();
            fileContent.Append($"{labels}\n");
            for (int i = 0; i < _rows; i++)
                fileContent.Append($"{channels[1][i]}&{channels[2][i]}&{channels[3][i]}&{channels[4][i]}&{channels[5][i]}&{channels[6][i]}&{channels[7][i]}&{channels[8][i]}\n");
            fileContent.Append("\n\n");

            writer.Write(fileContent.ToString().Replace("&", ","));
            writer.Flush();
            writer.Close();

            index += 2;
        }

        //  Retorna o grupo.
        return dataset;
    }
}