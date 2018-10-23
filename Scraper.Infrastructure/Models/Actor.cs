using System;
using System.Collections.Generic;

namespace Scraper.Infrastructure.Models
{
    public class Actor
    {
        public Actor()
        {
            ActorShow = new HashSet<ActorShow>();
        }

        public int Id { get; set; }
        public int ApiId { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }

        public ICollection<ActorShow> ActorShow { get; set; }
    }
}
