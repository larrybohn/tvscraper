using System;
using System.Diagnostics;
using System.Fabric;
using System.IO;
using System.Threading;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Runtime;
using Scraper.Domain.Contracts;
using Scraper.Infrastructure;
using Scraper.Infrastructure.Helpers;
using Scraper.Infrastructure.Mapping;
using Scraper.Infrastructure.Providers;
using Scraper.Infrastructure.Repositories;

namespace ScraperInjectorService
{
    internal static class Program
    {
        private static void Main()
        {
            try
            {
                var configurationRoot = new ConfigurationBuilder()
                    .SetBasePath(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName))
                    .AddJsonFile("sharedsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                ServiceRuntime.RegisterServiceAsync("ScraperInjectorServiceType",
                    context =>
                    {
                        var serviceProvider = Startup.ConfigureServices(context, configurationRoot);
                        return serviceProvider.GetService<ScraperInjectorService>();
                    }).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(ScraperInjectorService).Name);

                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}
