using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CNPJ.Core
{
    struct DadosDownload
    {
        public DadosDownload(string link, int posicao)
        {
            this.posicao = posicao;
            this.link = link;
        }
        public int posicao { get; }
        public string link { get; }
    };

    class Program
    {        
        static async Task<int> Main(string[] args)
        {
            Console.Clear();

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
                var pageService = services.GetRequiredService<IPageService>();
                var pageContent = await pageService.GetPageAsync();

                var listaLinks = pageService.GetLinks(pageContent);
                var listTasks = pageService.DownloadFilesAsync(listaLinks);
                
                Console.SetCursorPosition(0, listaLinks.Count() + 5);
                Console.WriteLine(String.Format("==== Download in progress ===== Start time: {0:d} at {0:t}", DateTime.Now));
                await Task.WhenAll(listTasks);
                Console.SetCursorPosition(0, listaLinks.Count() + 6);
                Console.WriteLine(String.Format("==== Finished: {0:d} at {0:t}", DateTime.Now));
            }            
            return 0;
        }
        
    }
}
