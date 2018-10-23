using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scraper.Domain.Commands;
using Scraper.Domain.Contracts;
using Scraper.Domain.Dto;
using Scraper.Infrastructure.Models;

namespace Scraper.Infrastructure.Repositories
{
    public class ShowCommandRepository: IShowCommandRepository
    {
        private readonly ScraperDBContext context;
        private readonly IMapper mapper;

        public ShowCommandRepository(ScraperDBContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task AddShowAsync(AddShowCommand addShowCommand)
        {
            bool exists = await context.Shows.AnyAsync(s => s.ApiId == addShowCommand.Show.Id);
            if (!exists)
            {
                var show = mapper.Map<ShowDto, Show>(addShowCommand.Show);
                await context.Shows.AddAsync(show);

                foreach(var actor in addShowCommand.Show.Cast)
                {
                    var dbActor = await context.Actors.SingleOrDefaultAsync(a => a.ApiId == actor.Id);
                    if (dbActor == null)
                    {
                        dbActor = mapper.Map<ActorDto, Actor>(actor);
                        await context.Actors.AddAsync(dbActor);
                    }
                    await context.ActorShow.AddAsync(new ActorShow {
                        Actor = dbActor,
                        Show = show
                    });
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
