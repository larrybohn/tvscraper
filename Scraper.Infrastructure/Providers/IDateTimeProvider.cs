using System;

namespace Scraper.Infrastructure.Providers
{
    public interface IDateTimeProvider
    {
        DateTime GetNow();
    }
}
