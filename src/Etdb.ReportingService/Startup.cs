using Autofac;
using Etdb.ReportingService.Autofac.Configuration;
using Etdb.ReportingService.Autofac.Extensions;
using Etdb.ReportingService.Worker;
using Etdb.ServiceBase.Constants;
using Etdb.ServiceBase.DocumentRepository.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
            services.AddControllers(options =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();

                    options.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services.AddHostedService<UserRegistrationMessageProcessor>()
                .AddHostedService<UserAuthenticationMessageProcessor>();

            services.Configure<DocumentDbContextOptions>(options =>
                this.configuration.Bind(nameof(DocumentDbContextOptions), options));

            services.Configure<AzureServiceBusConfiguration>(options =>
                options.ConnectionString = this.configuration.GetConnectionString("AzureServiceBus"));

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = this.configuration["Authority"];
                    options.ApiName = ServiceNames.ReportingService;
                    options.RequireHttpsMetadata = this.environment.IsProduction();
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseAuthentication();

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