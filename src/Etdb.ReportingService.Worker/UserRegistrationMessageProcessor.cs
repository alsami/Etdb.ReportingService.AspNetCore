using System;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ReportingService.Cqrs.Abstractions.Commands;
using Etdb.ReportingService.Services.Abstractions;
using Etdb.ReportingService.Services.Abstractions.Enums;
using Etdb.ReportingService.Services.Abstractions.Models;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Etdb.ReportingService.Worker
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class UserRegistrationMessageProcessor : BackgroundService
    {
        private readonly ILogger<UserRegistrationMessageProcessor> logger;
        private readonly IMediator mediator;
        private readonly IMessageConsumer<UserRegisteredMessage> messageConsumer;

        public UserRegistrationMessageProcessor(IMessageConsumer<UserRegisteredMessage> messageConsumer,
            ILogger<UserRegistrationMessageProcessor> logger, IMediator mediator)
        {
            this.messageConsumer = messageConsumer;
            this.logger = logger;
            this.mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.messageConsumer.RegisterHandler(MessageType.UserRegistered, this.MessageProcessorFunc,
                this.ExceptionHandlerFunc);

            while (!stoppingToken.IsCancellationRequested)
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }

        private Task MessageProcessorFunc(UserRegisteredMessage message) 
            => this.mediator.Send(new UserRegistrationStoreCommand(message.UserId, message.UserName, message.RegisteredAt));

        private void ExceptionHandlerFunc(Exception exception)
        {
            this.logger.LogCritical(exception, "Error while consuming queue-item", exception.Message);
        }
    }
}