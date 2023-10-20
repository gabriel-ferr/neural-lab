using Microsoft.AspNetCore.Http.Features;

namespace NeuralLab;

public class Program
{
    /// <summary>
    ///     Diretório para os elementos de funcionamento do sistema.
    /// </summary>
    public static readonly string PATH = Environment.GetEnvironmentVariable("NLPATH") ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "neural-lab");

    /// <summary>
    ///     Gerenciador dos arquivos temporários do sistema.
    /// </summary>
    public static readonly TempManager TempManager = new (600, 60);

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllersWithViews();
        builder.Services.Configure<FormOptions>(options => {
            options.ValueLengthLimit = int.MaxValue;
            options.KeyLengthLimit = int.MaxValue;
            options.MultipartBodyLengthLimit = int.MaxValue; // if don't set default value is: 128 MB
            options.MultipartHeadersLengthLimit = int.MaxValue;
        });
        builder.Services.AddMvc();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
        }

        app.UseStaticFiles();
        app.UseRouting();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}

