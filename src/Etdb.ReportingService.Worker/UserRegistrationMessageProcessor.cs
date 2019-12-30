using System;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ReportingService.Services.Abstractions;
using Etdb.ReportingService.Services.Abstractions.Enums;
using Etdb.ReportingService.Services.Abstractions.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Etdb.ReportingService.Worker
{
    public class UserRegistrationMessageProcessor : BackgroundService
    {
        private readonly IMessageConsumer<UserRegisteredMessage> messageConsumer;
        private readonly ILogger<UserRegistrationMessageProcessor> logger;

        public UserRegistrationMessageProcessor(IMessageConsumer<UserRegisteredMessage> messageConsumer, ILogger<UserRegistrationMessageProcessor> logger)
        {
            this.messageConsumer = messageConsumer;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.messageConsumer.RegisterHandler(MessageType.UserRegistered, message =>
            {
                this.logger.LogInformation("Received khara {khara}", JsonConvert.SerializeObject(message));
                return Task.CompletedTask;
            }, exception =>
            {
                this.logger.LogError(exception, "Got khara error {khara}", exception.Message);
            });
            
            while (!stoppingToken.IsCancellationRequested)
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}