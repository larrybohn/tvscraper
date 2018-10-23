using System.Collections.Generic;

namespace Scraper.Infrastructure.Providers.ApiModels
{
    public class EmbeddedModel
    {
        public IEnumerable<CastModel> Cast { get; set; }
    }
}
