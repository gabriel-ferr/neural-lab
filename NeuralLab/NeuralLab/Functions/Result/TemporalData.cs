using NeuralLab.Structs;

namespace NeuralLab.Functions.Result;

/// <summary>
///     Estrutura associada a interface de resultado para as séries
///     de dados temporais.
/// </summary>
public class TemporalData : Interfaces.IResult<Structs.PairData<float, float>>
{
    /// <summary>
    ///     Carrega uma série temporal a partir dos arquivos CSV associados.
    /// </summary>
    /// <param name="result">Objeto de contexto do resultado.</param>
    /// <param name="x">Número de retornos x da matriz retornada.</param>
    /// <param name="y">Número de retornos y da matriz retornada.</param>
    /// <param name="args">
    ///     Argumentos passados para identificação do arquivo:
    ///     - args[0]: estrutura de nome, substitui {1}, {2} e {3}
    ///     - args[1]: identificação do indivíduo de teste.
    ///     - args[2]: identificação da tarefa.
    ///     - args[3]: complementos.
    /// </param>
    /// <returns>Retorna o conjunto de dados carregado.</returns>
    public Structs.PairData<float, float>[,]? Load (Models.Result<Structs.PairData<float, float>> result, params object[] args)
    {
        //  - Objeto de resultado.
        Structs.PairData<float, float>[,]? res = null;

        //  - Define o diretório onde os arquivos csv devem estar localizados.
        string dir = Path.Combine(Program.PATH, "projects", result.Project.ToString(), "results", result.ID.ToString(), "temporal");
        if (!Directory.Exists(dir)) throw new Exceptions.ServerException(201, "A estrutura de dados requisitada não foi identificada no projeto.");

        //  - Define o número de arquivos no diretório.
        string[] path;
        int files = 1;
        if (args.Length < 4)
        {
            path = new string[1];
            path[0] = ((string)args[0]).Replace("{1}", Convert.ToString(args[1])).Replace("{2}", Convert.ToString(args[2]));
        }
        else
        {
            string[] comps = (string[]) args[2];
            files = comps.Length;
            path = new string[files];
            for (int i = 0; i < files; i++)
                path[i] = ((string)args[0]).Replace("{1}", Convert.ToString(args[1])).Replace("{2}", Convert.ToString(args[2])).Replace("{3}", comps[i]);
        }

        //  - Cria uma matrix de listas de conjuntos PairData.
        List<PairData<float, float>>[] byFile = new List<PairData<float, float>>[files];

        //  - Número de elementos internos.
        int fileColumns = 0;

        //  - Abre os arquivos e lê o conteúdo deles, preenchendo a lista de valores.
        for (int file = 0; file < files; file++)
        {
            //  - Inicializa a lista.
            byFile[file] = new List<PairData<float, float>>();

            //  - Verifica se o arquivo existe, se não existir, apenas segue sem fazer nada.
            if (!File.Exists(path[file])) throw new Exceptions.ServerException(203, "Um arquivo desejado não está disponível no caminho indicado.");

            //  - Lê o arquivo.
            StreamReader reader = new StreamReader(path[file]);
            //  Como o padrão são arquivos CSV, quebra usando '\n' de referência.
            string[] content = reader.ReadToEnd().Split('\n');
            //  Fecha o streamer.
            reader.Close();

            //  - A primeira linha é o conjunto de labels, o cabeçalho da tabela.
            //  Então, usa ela de referência para construir a estrutura de retorno.
            string[] labels = content[0].Split(',');

            //  - Cria a estrutura de dados e aplica o respectivo nome a ela.
            for (int label = 1; label < labels.Length; label++)
                byFile[file].Add(new PairData<float, float>() { Label = labels[label], Dataset = new List<Pair<float, float>>() });

                //  - Adiciona os valores.
            for (int line = 1; line < content.Length; line++)
            {
                //  - Separa as colunas que compõem a linha.
                string[] columns = content[line].Split(',');

                //  - Determina o número de colunas, ou verifica o valor.
                if (fileColumns == 0) fileColumns = columns.Length - 1;
                else { if (fileColumns != (columns.Length - 1)) throw new Exceptions.ServerException(202, "A estrutura interna dos arquivos não coincide com o esperado."); }

                //  - Lista as colunas.
                for (int i = 1; i < columns.Length; i++)
                    byFile[file][i].Dataset.Add(new Pair<float, float>() { x = Convert.ToSingle(columns[0]), y = Convert.ToSingle(columns[i])});
            }
        }

        //  - Instância o objeto de retorno.
        res = new PairData<float, float>[fileColumns, files];

        //  - Preenche o objeto de retorno.
        for (int i = 0; i < fileColumns; i++)
        {
            for (int j = 0; j < files; j++)
            {
                res[i, j] = byFile[j][i];
            }
        }


        //  - Retorna o objeto construído.
        return res;
    }
}