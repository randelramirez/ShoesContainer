using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductCatalogApi.Data;

namespace ProductCatalogApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                .Build();

            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ==
                                Environments.Development;

            var isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Production;

            if (isDevelopment || isProduction)
            {
                using var scope = host.Services.CreateScope();
                var services = scope.ServiceProvider;
                try
                {
                    Console.WriteLine("Seeding data");
                    var context = services.GetRequiredService<CatalogContext>();
                    Console.WriteLine($"connection string: {context.Database.GetDbConnection()}");
                    await CatalogSeed.SeedAsync(context);
                }
                catch (Exception e)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e,
                        "An error occured while seeding the database");
                    Console.WriteLine(e);
                    throw;
                }
            }

            Console.WriteLine(
                $" Environment.GetEnvironmentVariable(\"ASPNETCORE_URLS\"):{Environment.GetEnvironmentVariable("ASPNETCORE_URLS")}");
            Console.WriteLine($"isDevelopment: {isDevelopment.ToString()}");
            Console.WriteLine($"isProduction: {isProduction.ToString()}");
            Console.WriteLine(
                $"Environment.GetEnvironmentVariable(\"ASPNETCORE_ENVIRONMENT\"):{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");
            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    // webBuilder.UseKestrel(options => { options.Listen(IPAddress.Any, 5000); });
                    webBuilder.UseKestrel().UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS"));
                });
    }
}