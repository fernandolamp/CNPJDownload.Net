using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CNPJ.Core
{    public interface IPageService
    {
        Task<string> GetPageAsync();   
        IEnumerable<string> GetLinks(string pageContent);

        List<Task> DownloadFilesAsync(IEnumerable<string> listLinks);
    }    
}
