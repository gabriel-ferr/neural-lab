using CSV, DSP, DataFrames, Statistics

x = randn(10, 2)

df = DataFrame(; t = x[:, 1], y = x[:, 2])

CSV.write(ARGS[1] * "/" * "teste.temp", df)