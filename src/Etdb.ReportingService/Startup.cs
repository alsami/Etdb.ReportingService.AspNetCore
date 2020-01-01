using Autofac;
using Etdb.ReportingService.Autofac.Configuration;
using Etdb.ReportingService.Autofac.Extensions;
using Etdb.ReportingService.Worker;
using Etdb.ServiceBase.DocumentRepository.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Etdb.ReportingService
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHostedService<UserRegistrationMessageProcessor>();

            services.Configure<DocumentDbContextOptions>(options =>
                this.configuration.Bind(nameof(DocumentDbContextOptions), options));

            services.Configure<AzureServiceBusConfiguration>(options =>
                options.ConnectionString = this.configuration.GetConnectionString("AzureServiceBus"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
           containerBuilder.SetupDependencies(this.environment);
        }
    }
}