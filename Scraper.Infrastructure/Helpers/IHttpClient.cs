using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scraper.Infrastructure.Helpers
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(
            Uri requestUri
        );
    }
}
