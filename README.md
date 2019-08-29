# TinyQueueLogger
Usage:
```C#
public class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
        .ConfigureLogging((hostingContext, logging) =>
        {
            logging.ClearProviders();

            var queueSection = hostingContext.Configuration.GetSection("QueueLoggerOptions");
            
            logging.AddQueue((config)=> {
                queueSection.Bind(config);
            }); 
        })
            .UseStartup<Startup>();
}
```