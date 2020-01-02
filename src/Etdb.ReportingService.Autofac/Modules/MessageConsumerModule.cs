using System;
using Autofac;
using Etdb.ReportingService.Autofac.Configuration;
using Etdb.ReportingService.Autofac.Extensions;
using Etdb.ReportingService.Services;
using Etdb.ReportingService.Services.Abstractions;
using Etdb.ReportingService.Services.Abstractions.Enums;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Etdb.ReportingService.Autofac.Modules
{
    public class MessageConsumerModule : Module
    {
        private readonly IHostEnvironment environment;

        public MessageConsumerModule(IHostEnvironment environment)
        {
            this.environment = environment;
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (this.environment.IsContinousIntegration())
            {
                builder.RegisterGeneric(typeof(NoopMessageConsumer<>))
                    .As(typeof(IMessageConsumer<>))
                    .InstancePerDependency();

                return;
            }
            
            builder.RegisterGeneric(typeof(AzureServiceBusMessageConsumer<>))
                .As(typeof(IMessageConsumer<>))
                .InstancePerDependency();
            
            builder.Register<Func<MessageType, IQueueClient>>(outerComponentContext =>
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