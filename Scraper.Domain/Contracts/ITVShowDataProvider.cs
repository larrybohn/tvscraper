using System.Threading.Tasks;
using Scraper.Domain.Dto;

namespace Scraper.Domain.Contracts
{
    public interface ITVShowDataProvider
    {
        Task<ShowIndexDto> GetShowIndexAsync(int page);
        Task<ShowDto> GetShowCastAsync(int showId);
    }
}
