using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SVT.Platform
{
    public class Program
    {
        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    // configure
                })
                .UseStartup<Startup>();
    }
}
