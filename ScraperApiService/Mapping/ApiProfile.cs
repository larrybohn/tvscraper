using AutoMapper;
using Scraper.Domain.Dto;
using ScraperApiService.Models;

namespace ScraperApiService.Mapping
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            CreateMap<ShowDto, ShowModel>();

            CreateMap<ActorDto, ActorModel>()
                .ForMember(dest => dest.Birthday,
                    opts => opts.MapFrom(
                        src => src.Birthday.HasValue ? src.Birthday.Value.ToString("yyyy-MM-dd") : null));
        }
    }
}
