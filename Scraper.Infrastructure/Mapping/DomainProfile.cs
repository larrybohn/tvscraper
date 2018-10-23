using System;
using System.Linq;
using AutoMapper;
using Scraper.Domain.Dto;
using Scraper.Infrastructure.Models;
using Scraper.Infrastructure.Providers.ApiModels;

namespace Scraper.Infrastructure.Mapping
{
    public class DomainProfile: Profile
    {
        public DomainProfile()
        {
            CreateMap<Show, ShowDto>()
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Title))
                .ForMember(dest => dest.Cast, opts => opts.MapFrom(src => src.ActorShow.Select(e => e.Actor)));

            CreateMap<ShowDto, Show>()
                .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.ApiId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opts => opts.Ignore());

            CreateMap<Actor, ActorDto>();

            CreateMap<ActorDto, Actor>()
                .ForMember(dest => dest.ApiId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opts => opts.Ignore());

            CreateMap<ShowModel, ShowDto>()
                .ForMember(dest => dest.Cast, opts => opts.MapFrom(src => src.Embedded.Cast.Select(c => c.Person)));

            CreateMap<ActorModel, ActorDto>()
                .ForMember(dest => dest.Birthday, opts => opts.ResolveUsing(src =>
                {
                    if (src.Birthday != null)
                    {
                        DateTime birthday;
                        bool parsed = DateTime.TryParse(src.Birthday, out birthday);
                        if (parsed)
                        {
                            return birthday;
                        }
                    }
                    return (DateTime?)null;
                }));
        }
    }
}
