using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace TaskManagerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).
            UseApplicationInsights().
            ConfigureAppConfiguration((hostingContext, config) =>
            {

            }).
            ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                logging.AddDebug();
                logging.AddEventSourceLogger();
                logging.AddApplicationInsights();
                logging.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information);
            }).
            UseUrls("https://localhost:44374/").
            UseStartup<Startup>();
    }
}
