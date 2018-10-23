using System.Collections.Generic;

namespace Scraper.Domain.Dto
{
    public class ShowDto: DtoBase
    {
        public string Name { get; set; }
        public IEnumerable<ActorDto> Cast { get; set; }
    }
}
