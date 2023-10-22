#
#   Script do Julia.
#   Por Gabriel Ferreira.
#
#   Essa é a estrutura para a importação de dados no modelo de uso da rede.
#   A partir disso a rede gera os arquivos de entrada que serão usados nela.
#   Também são aplicados alguns filtros no processo.
#
#   Executa com o comando:
#   
#       julia `import.jl` <ARQUIVO BRUTO> <PASTA ONDE OS ARQUIVOS SERÃO SALVOS COM NOME: [CANAL]_[ONDA].temp>

#   - Instala as referências necessárias.
#import Pkg;
#Pkg.add("DataFrames");
#Pkg.add("CSV");
#Pkg.add("DSP");
#Pkg.add("Statistics");

#   - Referencia as bibliotecas usadas.
using CSV, DSP, DataFrames, Statistics

#   - Tamanho dos blocos.
const BLOCKSIZE = 125

#   - Multiplicador do intervalo interquartil para a remoção dos outliers.
const IQR_MULTIPLIER = 2

#   - Função para remover os Outliers.
function RemoveOutliers(time, data)
    #   Calcula os valores estatísticos dos quadrantes.
    Q1 = quantile(data, 0.25)
    Q3 = quantile(data, 0.75)
    IQR = Q3 - Q1

    #   Vetor de construção dos resultados.
    result = []
    times = []

    #   Verifica os elementos dos dados e constroí o vetor de resultados.
    for i = 1:size(data)[1]
        if ((data[i] < (Q1 - (IQR_MULTIPLIER * IQR)) || (data[i] > (Q3 + (IQR_MULTIPLIER * IQR)))))
            continue
        end
        push!(result, data[i])
        push!(times, time[i])
    end

    #   Retorna o resultado.
    return (times, result)
end

#   - Filtro Notch para normalizar a rede.
notchFilter = Filters.iirnotch(65, 45; fs = 250.0)

#   - Filtros para selecionar as ondas Alpha.
alphaLowpass = Lowpass(12; fs = 250.0)
alphaHighpass = Highpass(8; fs = 250.0)

#   - Filtros para selecionar as ondas Beta.
betaLowpass = Lowpass(30; fs = 250.0)
betaHighpass = Highpass(12; fs = 250.0)

#   - Filtros para selecionar as ondas Gamma.
gammaLowpass = Lowpass(70; fs = 250.0)
gammaHighpass = Highpass(30; fs = 250.0)

#   - Método de filtragem.
filterMethod = Butterworth(4);

#   - Importa o arquivo CSV como um DataFrame.
rawData = CSV.read(ARGS[1], DataFrame)

#   - Processa o DataFrame por coluna, ou seja, pegando cada canal do EEG um por um.
for channel = 1:size(rawData)[2]

    #   - Separa um vetor com os valores do canal.
    raw = rawData[:, channel]

    #   - Calcula o tamanho do intervalo de dados.
    interval = range(0, size(raw)[1] / 250.0, size(raw)[1])

    #   - Aplica o filtro Notch.
    filtered = filtfilt(notchFilter, raw);

    #   - Separa o espectro alpha.
    alphaWave = filt(digitalfilter(alphaLowpass, filterMethod), filtered)
    alphaWave = filt(digitalfilter(alphaHighpass, filterMethod), alphaWave)

    #   - Separa o espectro beta.
    betaWave = filt(digitalfilter(betaLowpass, filterMethod), filtered)
    betaWave = filt(digitalfilter(betaHighpass, filterMethod), betaWave)

    #   - Separa o espectro gamma.
    gammaWave = filt(digitalfilter(gammaLowpass, filterMethod), filtered)
    gammaWave = filt(digitalfilter(gammaHighpass, filterMethod), gammaWave) 

    #   - Divide a onda em blocos de tamanho fixo.
    blocks = trunc(Int, size(filtered)[1]/BLOCKSIZE)

    #   - Cria vetores para armazenar os blocos de cada espectro.
    alpha = []
    beta = []
    gamma = []
        
    #   - Número de sobras.
    rest = size(filtered)[1] - (BLOCKSIZE * blocks)

    #   - Processa bloco por bloco.
    for block = 1:blocks
        #   - Remove os outliers.
        alphaBlock = RemoveOutliers(interval[1 + ((block - 1) * BLOCKSIZE):(block * BLOCKSIZE)], alphaWave[1 + ((block - 1) * BLOCKSIZE):(block * BLOCKSIZE)])
        betaBlock = RemoveOutliers(interval[1 + ((block - 1) * BLOCKSIZE):(block * BLOCKSIZE)], betaWave[1 + ((block - 1) * BLOCKSIZE):(block * BLOCKSIZE)])
        gammaBlock = RemoveOutliers(interval[1 + ((block - 1) * BLOCKSIZE):(block * BLOCKSIZE)], gammaWave[1 + ((block - 1) * BLOCKSIZE):(block * BLOCKSIZE)])
            
        #   - Coloca o bloco na array dele.
        push!(alpha, alphaBlock)
        push!(beta, betaBlock)
        push!(gamma, gammaBlock)
    end

    #   - Adiciona os elementos restantes no bloco.
    #   Remove os outliers.
    push!(alpha, RemoveOutliers(interval[((BLOCKSIZE * blocks) + 1):((BLOCKSIZE * blocks) + rest)], alphaWave[((BLOCKSIZE * blocks) + 1):((BLOCKSIZE * blocks) + rest)]))
    push!(beta, RemoveOutliers(interval[((BLOCKSIZE * blocks) + 1):((BLOCKSIZE * blocks) + rest)], betaWave[((BLOCKSIZE * blocks) + 1):((BLOCKSIZE * blocks) + rest)]))
    push!(gamma, RemoveOutliers(interval[((BLOCKSIZE * blocks) + 1):((BLOCKSIZE * blocks) + rest)], gammaWave[((BLOCKSIZE * blocks) + 1):((BLOCKSIZE * blocks) + rest)]))

    #   - Vetores para armazenar os valores de tempo e da informação para cada espectro.
    data = (alpha, beta, gamma)

    #   - Processa a saída de cada espectro individualmente.
    for w = 1:3
        #   - Cria vetores para armazenar os valores de tempo e da informação.
        time = []
        info = []

        #   - Pega o espectro desejado.
        wave = data[w]

        #   - Processa bloco por bloco dentro do espectro.
        for i = 1:(blocks + 1)
            #   - Pega os elementos um por um.
            for j = 1:size(wave[i][1])[1]
                push!(time, wave[i][1][j])
                push!(info, wave[i][2][j])
            end
        end

        #   - Gera o dataframe para exportar o arquivo CSV.
        df = DataFrame(;t = time, i = info)

        #   - Exporta o csv.
        CSV.write(ARGS[2] * "/" * string(channel) * "_" * string(w) * ".temp", df);
    end
end