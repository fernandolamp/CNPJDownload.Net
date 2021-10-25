using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Net;
using System;
using System.IO;
using System.Threading;

namespace CNPJ.Core
{
    public class PageService : IPageService
    {
        private const string DownloadsPath = "Downloads";
        private readonly IHttpClientFactory clientFactory;
        private Object consoleLock = new Object();
        public PageService(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public List<Task> DownloadFilesAsync(IEnumerable<string> listLinks)
        {
            var listTasks = new List<Task>();
            int position = 1;
            var dir = Directory.CreateDirectory(DownloadsPath);            
            foreach (var link in listLinks)
            {
                WebClient webClient = new WebClient();                
                var filename = String.Format("{0} - {1}", position, Path.GetFileName(link));
                webClient.Headers.Add("file", filename);
                webClient.Headers.Add("position", position.ToString());
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                
                listTasks.Add(Task.Run(async () =>
                {
                    await webClient.DownloadFileTaskAsync(link, dir.FullName + Path.DirectorySeparatorChar + filename);
                }));
                position++;
            }
            return listTasks;
        }

        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            string fileIdentifier = ((WebClient)(sender)).Headers["file"];            
            lock (consoleLock)
            {
                Console.SetCursorPosition(0, Int32.Parse(((WebClient)(sender)).Headers["position"]));
                Console.Write("\r {0} - Baixado {1:F3} of {2:F3} Megabytes. {3} % completo...",
                fileIdentifier,
                ConvertBytesToMegabytes(e.BytesReceived),
                ConvertBytesToMegabytes(e.TotalBytesToReceive),
                e.ProgressPercentage);
            }
        }

        private double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        public IEnumerable<string> GetLinks(string pageContent)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(pageContent);
            var links = doc.DocumentNode.SelectNodes("//a[@href]");

            foreach (var link in links)
            {
                var linkValue = link.Attributes.AttributesWithName("href").GetEnumerator();
                linkValue.MoveNext();
                if (linkValue.Current.Value.Contains("zip"))
                {
                    yield return linkValue.Current.Value;
                }
            }
        }

        public async Task<string> GetPageAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                "https://www.gov.br/receitafederal/pt-br/assuntos/orientacao-tributaria/cadastros/consultas/dados-publicos-cnpj");
            var client = clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return $"StatusCode: {response.StatusCode}";
            }
        }
    }
}
