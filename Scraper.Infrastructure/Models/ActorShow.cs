namespace Scraper.Infrastructure.Models
{
    public class ActorShow
    {
        public int ActorId { get; set; }
        public int ShowId { get; set; }

        public Actor Actor { get; set; }
        public Show Show { get; set; }
    }
}
