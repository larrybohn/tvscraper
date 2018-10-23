using Newtonsoft.Json;

namespace Scraper.Infrastructure.Providers.ApiModels
{
    public class ShowModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonProperty("_Embedded")]
        public EmbeddedModel Embedded { get; set; }
    }
}
