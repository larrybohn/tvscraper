namespace ScraperInjectorService
{
    public class ScraperConfiguration
    {
        public string ApiBaseUri { get; set; }
        public int MaxRequestCount { get; set; }
        public int MaxRequestsIntervalSeconds { get; set; }
        public int PollIntervalSeconds { get; set; }
    }
}
