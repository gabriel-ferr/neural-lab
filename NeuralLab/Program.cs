
namespace NeuralLab;

public class Program
{
    public static readonly string PATH = Environment.GetEnvironmentVariable("NLPATH") ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "neural-lab");

    public static void Main(string[] args)
    {
        //  Creathe the root path.
        if (!Directory.Exists(PATH)) Directory.CreateDirectory(PATH);

        //  Load the Accounts.
        Backend.Utils.Data.Accounts.Load().Wait();
        Backend.Utils.Data.NetworkModels.Load().Wait();
        Backend.Utils.Data.Projects.Load().Wait();

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllersWithViews();

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

