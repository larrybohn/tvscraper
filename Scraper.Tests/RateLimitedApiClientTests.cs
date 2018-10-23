using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Scraper.Infrastructure.Helpers;
using Scraper.Infrastructure.Providers;

namespace Scraper.Tests
{
    [TestClass]
    public class RateLimitedApiClientTests
    {
        [TestMethod]
        public void RateLimitedApiClient_PerformsRateLimiting()
        {

        }

        [TestMethod]
        public void RateLimitedApiClient_RetriesWhenTooManyRequests()
        {

        }

        [TestMethod]
        public void RateLimitedApiClient_DoesNotRetryOnOtherErrors()
        {

        }
    }
}
