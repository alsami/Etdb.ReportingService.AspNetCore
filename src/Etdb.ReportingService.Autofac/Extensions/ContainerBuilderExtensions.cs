using System;
using Autofac;
using Autofac.Extensions.FluentBuilder;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Etdb.ReportingService.Autofac.Configuration;
using Etdb.ReportingService.Autofac.Modules;
using Etdb.ReportingService.AutoMapper.Profiles;
using Etdb.ReportingService.Cqrs.CommandHandler;
using Etdb.ReportingService.Repositories;
using Etdb.ReportingService.Services;
using Etdb.ReportingService.Services.Abstractions;
using Etdb.ReportingService.Services.Abstractions.Enums;
using Etdb.ServiceBase.DocumentRepository;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Etdb.ReportingService.Autofac.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static void SetupDependencies(this ContainerBuilder containerBuilder, IHostEnvironment hostEnvironment)
        {
            containerBuilder.AddAutoMapper(typeof(UserRegistrationProfile).Assembly);
            containerBuilder.AddMediatR(typeof(UserRegistrationStoreCommandHandler).Assembly);
            
            new AutofacFluentBuilder(containerBuilder)
                .AddGenericAsTransient(typeof(AzureServiceBusMessageConsumer<>), typeof(IMessageConsumer<>))
                .AddClosedTypeAsScoped(typeof(GenericDocumentRepository<,>), new []
                {
                    typeof(UserRegistrationsRepository).Assembly
                })
                .ApplyModule(new ResourceLockingAdapterModule(false))
                .ApplyModule(new DocumentDbContextModule(hostEnvironment));

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