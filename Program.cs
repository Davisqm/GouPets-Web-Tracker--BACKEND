using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace PetsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseUrls("http://localhost:5000") // Usar HTTP en lugar de HTTPS
                        .UseStartup<Startup>();
                });
    }
}


