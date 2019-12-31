using System;
using Autofac;
using Autofac.Extensions.FluentBuilder;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Etdb.ReportingService.AutoMapper;
using Etdb.ReportingService.Cqrs.Abstractions.Commands;
using Etdb.ReportingService.Cqrs.CommandHandler;
using Etdb.ReportingService.Modules;
using Etdb.ReportingService.Services;
using Etdb.ReportingService.Services.Abstractions;
using Etdb.ReportingService.Services.Abstractions.Enums;
using Etdb.ReportingService.Worker;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Etdb.ReportingService
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHostedService<UserRegistrationMessageProcessor>();

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
            containerBuilder.AddAutoMapper(typeof(TestProfile).Assembly);
            containerBuilder.AddMediatR(typeof(UserRegistrationStoreCommandHandler).Assembly);
            
            new AutofacFluentBuilder(containerBuilder)
                .AddGenericAsTransient(typeof(AzureServiceBusMessageConsumer<>), typeof(IMessageConsumer<>))
                .ApplyModule(new ResourceLockingAdapterModule(false));

            containerBuilder.Register<Func<MessageType, IQueueClient>>(outerComponentContext =>
                {
                    var innerComponentContext = outerComponentContext.Resolve<IComponentContext>();

                    return messageType =>
                    {
                        var options = innerComponentContext.Resolve<IOptions<AzureServiceBusConfiguration>>();

                        return messageType switch
                        {
                            MessageType.UserRegistered => new QueueClient(options.Value.ConnectionString,
                                options.Value.UserRegisteredTopic),
                            MessageType.UserAuthenticated => new QueueClient(options.Value.ConnectionString,
                                options.Value.UserAuthenticatedTopic),
                            _ => throw new ArgumentOutOfRangeException(nameof(messageType))
                        };
                    };
                })
                .InstancePerDependency();
        }
    }
}