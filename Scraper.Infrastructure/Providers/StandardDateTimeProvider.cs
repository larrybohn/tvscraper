using System;

namespace Scraper.Infrastructure.Providers
{
    public class StandardDateTimeProvider: IDateTimeProvider
    {
        public DateTime GetNow()
        {
            return DateTime.Now;
        }
    }
}
