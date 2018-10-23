using System.Collections.Generic;

namespace Scraper.Infrastructure.Models
{
    public class Show
    {
        public Show()
        {
            ActorShow = new HashSet<ActorShow>();
        }

        public int Id { get; set; }
        public int ApiId { get; set; }
        public string Title { get; set; }
        public ICollection<ActorShow> ActorShow { get; set; }
    }
}
