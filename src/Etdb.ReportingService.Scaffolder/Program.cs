using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Etdb.ReportingService.Autofac.Extensions;
using Etdb.ReportingService.Scaffolder.Migrations;
using Etdb.ServiceBase.DocumentRepository;
using Etdb.ServiceBase.DocumentRepository.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using Moq;

namespace Etdb.ReportingService.Scaffolder
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = CreateHost(args);

            await host.StartAsync();

            var scaffolder = host.Services.GetRequiredService<DatabaseScaffolder>();

            await scaffolder.ScaffoldAsync();            

            await host.StopAsync();
        }

        private static IHost CreateHost(string[] args)
            => Host.CreateDefaultBuilder(args)
                .UseEnvironment("Development")
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureServices(ConfigureServices)
                .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
                .Build();

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var configuration = hostContext.Configuration;

            services.AddScoped<DatabaseScaffolder>();
            services.AddScoped<CollectionsFactory>();
            services.AddScoped<SampleDataFactory>();

            services.Configure<DocumentDbContextOptions>(options =>
            {
                configuration.Bind(nameof(DocumentDbContextOptions), options);
            });
        }

        private static void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            var environmentMock = new Mock<IHostEnvironment>();

            environmentMock.Setup(environment => environment.EnvironmentName)
                .Returns("Development");

            containerBuilder.SetupDependencies(environmentMock.Object);
        }

        private static void ConfigureAppConfiguration(HostBuilderContext _, IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .AddEnvironmentVariables()
                .AddUserSecrets("Etdb_ReportingService");
        }
    }
}