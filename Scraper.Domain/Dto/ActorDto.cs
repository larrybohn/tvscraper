using System;

namespace Scraper.Domain.Dto
{
    public class ActorDto: DtoBase
    {
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
