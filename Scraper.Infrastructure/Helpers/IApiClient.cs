using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Scraper.Infrastructure.Helpers
{
    public interface IApiClient
    {
        Task<T> GetDataAsync<T>(Uri address);
        Task<HttpResponseMessage> GetDataAsync(Uri address);
    }
}
