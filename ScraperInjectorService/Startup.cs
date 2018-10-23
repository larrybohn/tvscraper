using System;
using System.Collections.Generic;
using System.Fabric;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scraper.Domain.Contracts;
using Scraper.Infrastructure;
using Scraper.Infrastructure.Helpers;
using Scraper.Infrastructure.Mapping;
using Scraper.Infrastructure.Providers;
using Scraper.Infrastructure.Repositories;

namespace ScraperInjectorService
{
    internal static class Startup
    {
        internal static IServiceProvider ConfigureServices(StatelessServiceContext context, IConfigurationRoot configurationRoot)
        {
            var scraperConfiguration = configurationRoot.GetSection("Scraper").Get<ScraperConfiguration>();

            var serviceProvider = new ServiceCollection();

            serviceProvider.AddSingleton<IMapper>(provider =>
            {
                var mapperConfiguration = new MapperConfiguration(cfg => {
                    cfg.AddProfile<DomainProfile>();
                });
                return mapperConfiguration.CreateMapper();
            });
            serviceProvider.AddTransient<IHttpClient, StandardHttpClient>();
            serviceProvider.AddSingleton<ILogger>(provider => (new LoggerFactory()).CreateLogger("Debug"));
            serviceProvider.AddSingleton<IDateTimeProvider, StandardDateTimeProvider>();
            serviceProvider.AddSingleton<IApiClient>(provider => new RateLimitedApiClient(
                provider.GetService<IDateTimeProvider>(),
                provider.GetService<IHttpClient>(),
                scraperConfiguration.MaxRequestCount,
                scraperConfiguration.MaxRequestsIntervalSeconds)
            );
            serviceProvider.AddSingleton<ITVShowDataProvider>(provider => new TVMazeTVShowDataProvider(
                provider.GetService<IApiClient>(),
                provider.GetService<ILogger>(),
                new Uri(scraperConfiguration.ApiBaseUri),
                provider.GetService<IMapper>())
            );
            serviceProvider.AddTransient<ScraperDBContext>(provider =>
                new ScraperDBContext(configurationRoot.GetConnectionString("DefaultConnection")));
            serviceProvider.AddSingleton<IShowCommandRepository, ShowCommandRepository>();
            serviceProvider.AddSingleton<ScraperConfiguration>(scraperConfiguration);
            serviceProvider.AddTransient<ScraperInjectorService>();
            serviceProvider.AddSingleton<StatelessServiceContext>(context);

            return serviceProvider.BuildServiceProvider();
        }
    }
}
