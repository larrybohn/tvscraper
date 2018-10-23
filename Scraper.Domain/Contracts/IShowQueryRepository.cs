using System.Collections.Generic;
using System.Threading.Tasks;
using Scraper.Domain.Dto;
using Scraper.Domain.Queries;

namespace Scraper.Domain.Contracts
{
    public interface IShowQueryRepository
    {
        Task<IEnumerable<ShowDto>> GetShowsAsync(ShowQuery showQuery);
    }
}
