using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scraper.Domain.Contracts;
using Scraper.Domain.Dto;
using Scraper.Domain.Queries;
using Scraper.Infrastructure.Models;

namespace Scraper.Infrastructure.Repositories
{
    public class ShowQueryRepository: IShowQueryRepository
    {
        private readonly ScraperDBContext context;
        private readonly int pageSize = 10;
        private readonly IMapper mapper;
        public ShowQueryRepository(ScraperDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<ShowDto>> GetShowsAsync(ShowQuery showQuery)
        {
            return (await context.Shows
                .Include(e => e.ActorShow)
                .ThenInclude(e => e.Actor)
                .Skip(pageSize * (showQuery.Page - 1))
                .Take(pageSize)
                .ToListAsync())
                .Select(mapper.Map<Show, ShowDto>);
        }
    }
}
