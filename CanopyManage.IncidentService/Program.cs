using CanopyManage.Infrastructure.Azure.KeyVault;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CanopyManage.IncidentService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
             .ConfigureAppConfiguration((context, config) =>
             {
                 if (context.HostingEnvironment.IsProduction())
                 {
                     AppConfigBuilder.ConfigAzureKevaultConfiguration(context, config);
                 }
             })
             .UseStartup<Startup>();
    }
}
