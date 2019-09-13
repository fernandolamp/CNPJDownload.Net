using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace CNPJ.Core
{
    public class PageService : IPageService
    {
        private readonly IHttpClientFactory clientFactory;

        public PageService(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
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

        public async Task<string> GetPage()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                "http://receita.economia.gov.br/orientacao/tributaria/cadastros/cadastro-nacional-de-pessoas-juridicas-cnpj/dados-publicos-cnpj/");
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
