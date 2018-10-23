using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Scraper.Domain.Commands;
using Scraper.Domain.Contracts;
using Scraper.Domain.Dto;

namespace ScraperInjectorService
{
    public class Scraper
    {
        private readonly IShowCommandRepository showCommandRepository;
        private readonly ITVShowDataProvider dataProvider;
        private readonly ILogger logger;
        public Scraper(IShowCommandRepository showCommandRepository, ITVShowDataProvider dataProvider, ILogger logger)
        {
            this.showCommandRepository = showCommandRepository;
            this.dataProvider = dataProvider;
            this.logger = logger;
        }

        public async Task PerformScraping(CancellationToken cancellationToken)
        {
            bool hasMoreData;
            int page = 0;
            do
            {
                logger.LogInformation($"Fetching page {page}");
                ShowIndexDto showIndexPage = await SafeExecute(() => dataProvider.GetShowIndexAsync(page));

                if (showIndexPage != null && showIndexPage.HasData)
                {
                    await ProcessPage(showIndexPage, cancellationToken);
                }

                hasMoreData = showIndexPage == null || showIndexPage.HasData;
                ++page;
            } while (hasMoreData);
        }

        private async Task ProcessPage(ShowIndexDto showIndexPage, CancellationToken cancellationToken)
        {
            foreach (var showId in showIndexPage.ShowIds)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
                ShowDto show = await SafeExecute(() => dataProvider.GetShowCastAsync(showId));
                if (show != null)
                {
                    try
                    {
                        await AddShow(show);
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e.ToString());
                    }
                }
                else
                {
                    logger.LogWarning($"could not load show {showId}");
                }
            }
        }

        private async Task<T> SafeExecute<T>(Func<Task<T>> func) where T : class
        {
            try
            {
                return await func();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
            }

            return null;
        }

        private async Task AddShow(ShowDto show)
        {
            var addShowCommand = new AddShowCommand
            {
                Show = show
            };
            await showCommandRepository.AddShowAsync(addShowCommand);
        }
    }
}
