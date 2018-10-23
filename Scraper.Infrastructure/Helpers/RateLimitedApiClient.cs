using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Scraper.Infrastructure.Providers;

namespace Scraper.Infrastructure.Helpers
{
    public class RateLimitedApiClient: IApiClient
    {
        private readonly int maxRetryAttempts = 5;
        private readonly IHttpClient httpClient;
        private DateTime? lastRequestDateTime = null;
        private readonly TimeSpan maxRequestRate;
        private readonly IDateTimeProvider dateTimeProvider;

        public RateLimitedApiClient(IDateTimeProvider dateTimeProvider, IHttpClient httpClient, int maxRequests = 20, int perIntervalSeconds = 10)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.httpClient = httpClient;
            maxRequestRate = TimeSpan.FromMilliseconds(perIntervalSeconds * 1000 / maxRequests);
        }

        public async Task<HttpResponseMessage> GetDataAsync(Uri address)
        {
            return await RateLimitedGetAsync(address);
        }

        public async Task<T> GetDataAsync<T>(Uri address)
        {
            var response = await RateLimitedGetAsync(address);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        private async Task<HttpResponseMessage> RateLimitedGetAsync(Uri address)
        {
            DateTime now = dateTimeProvider.GetNow();
            if (lastRequestDateTime.HasValue)
            {
                var timeElapsed = now.Subtract(lastRequestDateTime.Value);
                if (timeElapsed < maxRequestRate)
                {
                    await Task.Delay(maxRequestRate.Subtract(timeElapsed));
                }
            }

            lastRequestDateTime = now;

            var response = await httpClient.GetAsync(address);

            int retryAttempt = 0;
            while (response.StatusCode == HttpStatusCode.TooManyRequests && retryAttempt < maxRetryAttempts)
            {
                await Task.Delay(maxRequestRate.Subtract(maxRequestRate));
                lastRequestDateTime = now;
                response = await httpClient.GetAsync(address);
                ++retryAttempt;
            }
            return response;
        }
    }
}
