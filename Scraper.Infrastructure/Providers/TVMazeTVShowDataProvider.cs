using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Scraper.Domain.Contracts;
using Scraper.Domain.Dto;
using Scraper.Infrastructure.Helpers;
using Scraper.Infrastructure.Providers.ApiModels;

namespace Scraper.Infrastructure.Providers
{
    public class TVMazeTVShowDataProvider : ITVShowDataProvider
    {
        private readonly Uri ApiBaseUrl;
        private readonly IApiClient apiClient;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public TVMazeTVShowDataProvider(IApiClient apiClient, ILogger logger, Uri baseUri, IMapper mapper)
        {
            this.apiClient = apiClient;
            this.logger = logger;
            this.ApiBaseUrl = baseUri;
            this.mapper = mapper;
        }
        public async Task<ShowDto> GetShowCastAsync(int showId)
        {
            try
            {
                var response = await apiClient.GetDataAsync<ShowModel>(
                    new Uri(ApiBaseUrl, $"shows/{showId}?embed=cast"));

                var show = mapper.Map<ShowModel, ShowDto>(response);
                show.Cast = show.Cast
                    .GroupBy(c => c.Id)
                    .Select(g => g.First()); //skip duplicate entries which sometimes occur in the API
                return show;

            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
                return null;
            }
        }

        public async Task<ShowIndexDto> GetShowIndexAsync(int page)
        {
            var response = await apiClient.GetDataAsync(new Uri(ApiBaseUrl, "shows?page=" + page));
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new ShowIndexDto
                {
                    HasData = false
                };
            }
            response.EnsureSuccessStatusCode();

            var showIndexModel = JsonConvert.DeserializeObject<IEnumerable<ShowIndexModel>>(await response.Content.ReadAsStringAsync());
            return new ShowIndexDto
            {
                HasData = true,
                Id = page,
                ShowIds = showIndexModel.Select(s => s.Id)
            };
        }
    }
}
