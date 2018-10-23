using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Scraper.Domain.Contracts;

namespace ScraperInjectorService
{
    internal sealed class ScraperInjectorService : StatelessService
    {
        private readonly ITVShowDataProvider dataProvider;
        private readonly IShowCommandRepository showCommandRepository;
        private readonly int pollIntervalSeconds;
        private readonly ILogger logger;

        public ScraperInjectorService(
            StatelessServiceContext context,
            ITVShowDataProvider tvShowDataProvider,
            IShowCommandRepository showCommandRepository,
            ILogger logger,
            ScraperConfiguration configuration)
            : base(context)
        {
            this.dataProvider = tvShowDataProvider;
            this.showCommandRepository = showCommandRepository;
            this.logger = logger;
            this.pollIntervalSeconds = configuration.PollIntervalSeconds;
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[0];
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            var scraper = new Scraper(showCommandRepository, dataProvider, logger);

            while (!cancellationToken.IsCancellationRequested)
            {
                await scraper.PerformScraping(cancellationToken);

                logger.LogInformation("Fetching complete");

                await Task.Delay(TimeSpan.FromSeconds(pollIntervalSeconds), cancellationToken);
            }
        }
    }
}
