using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Etdb.ReportingService
{
    public static class Program
    {
        private static readonly string LogPath = Path.Combine(AppContext.BaseDirectory, "Logs",
            $"{Assembly.GetExecutingAssembly().GetName().Name}.log");
        
        public static async Task Main(string[] args)
        {
            using var host = Program.CreateHost(args);

            await host.RunAsync();
        }

        private static IHost CreateHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog(ConfigureLogger)
                .ConfigureAppConfiguration(ConfigureDelegate)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .Build();

        private static void ConfigureDelegate(HostBuilderContext context, IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Sources.Clear();

            configurationBuilder.SetBasePath(AppContext.BaseDirectory);

            configurationBuilder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json");

            if (!context.HostingEnvironment.IsDevelopment()) return;

            configurationBuilder.AddUserSecrets("Etdb_ReportingService");
        }

        private static void ConfigureLogger(HostBuilderContext _, LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration
                .MinimumLevel.Is(LogEventLevel.Debug)
                .Enrich.FromLogContext()
                .WriteTo.RollingFile(Program.LogPath)
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Literate);
        }
    }
}