using System.Collections.Generic;
using System.Threading.Tasks;

namespace CNPJ.Core
{    public interface IPageService
    {
        Task<string> GetPage();   
        IEnumerable<string> GetLinks(string pageContent);
    }    
}
