using System.Collections.Generic;

namespace Scraper.Domain.Dto
{
    public class ShowIndexDto: DtoBase
    {
        public bool HasData { get; set; }
        public IEnumerable<int> ShowIds { get; set; }
    }
}
