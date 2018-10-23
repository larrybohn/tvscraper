using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scraper.Infrastructure.Helpers
{
    public class StandardHttpClient: IHttpClient
    {
        private readonly HttpClient httpClient = new HttpClient();

        public Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return httpClient.GetAsync(requestUri);
        }
    }
}
