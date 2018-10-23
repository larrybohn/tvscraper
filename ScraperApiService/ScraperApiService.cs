using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace ScraperApiService
{
    internal sealed class ScraperApiService : StatelessService
    {
        public ScraperApiService(StatelessServiceContext context)
            : base(context)
        { }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        return CreateWebHostBuilder(serviceContext, url, listener);
                    }))
            };
        }

        public static IWebHost CreateWebHostBuilder(StatelessServiceContext serviceContext, string url, AspNetCoreCommunicationListener listener)
        {
            return new WebHostBuilder()
                .UseKestrel()
                .ConfigureAppConfiguration((builderContext, config) =>
                    {
                        config
                            .SetBasePath(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName))
                            .AddJsonFile("sharedsettings.json", optional: true)
                            .AddJsonFile("appsettings.json", optional: false)
                            .AddJsonFile($"appsettings.{builderContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    })
                .ConfigureServices(services =>
                    services.AddSingleton<StatelessServiceContext>(serviceContext))
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddDebug();
                })
                .UseStartup<Startup>()
                .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                .UseUrls(url)
                .Build();
        }
    }
}
