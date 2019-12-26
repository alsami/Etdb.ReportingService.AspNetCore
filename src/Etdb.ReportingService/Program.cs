using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Etdb.ReportingService
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = Program.CreateHost(args);

            await host.RunAsync();
        }

        private static IHost CreateHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .Build();
    }
}