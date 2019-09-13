using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CNPJ.Core
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var builder = new HostBuilder()
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddHttpClient();
                        services.AddTransient<IPageService, PageService>();
                    }).UseConsoleLifetime();

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var myService = services.GetRequiredService<IPageService>();
                    var pageContent = await myService.GetPage();

                    foreach (var link in myService.GetLinks(pageContent))
                    {
                       Console.WriteLine(link);
                    }                    
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred.");
                }
            }

            return 0;
        }
    }
}
