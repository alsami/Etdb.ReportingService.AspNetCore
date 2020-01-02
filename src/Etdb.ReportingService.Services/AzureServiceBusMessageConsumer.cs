using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ReportingService.Misc.Exceptions;
using Etdb.ReportingService.Services.Abstractions;
using Etdb.ReportingService.Services.Abstractions.Enums;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace Etdb.ReportingService.Services
{
    public class AzureServiceBusMessageConsumer<TMessage> : IMessageConsumer<TMessage>, IAsyncDisposable where TMessage : class
    {
        private readonly Func<MessageType, IQueueClient> queueClientComposer;
        private IQueueClient? queueClient;
        private Func<TMessage, Task> messageHandler = message => Task.CompletedTask;
        private Action<Exception> exceptionHandler = exception => { };

        public AzureServiceBusMessageConsumer(Func<MessageType, IQueueClient> queueClientComposer)
        {
            this.queueClientComposer = queueClientComposer ?? throw new ArgumentException(nameof(queueClientComposer));
        }

        public void RegisterHandler(MessageType messageType, Func<TMessage, Task> messageProcessorFunc, Action<Exception> exceptionHandlerFunc)
        {
            this.queueClient = this.queueClientComposer(messageType);
            this.messageHandler = messageProcessorFunc ?? throw new ArgumentNullException(nameof(messageProcessorFunc));
            this.exceptionHandler = exceptionHandlerFunc;
            this.queueClient.RegisterMessageHandler(this.Handler, this.ExceptionReceivedHandler);
        }

        public async ValueTask DisposeAsync()
        {
            if (this.queueClient == null) return;
            
            await this.queueClient.CloseAsync();
        }
        
        private async Task Handler(Message message, CancellationToken cancellationToken)
        {
            var content = message.Body;
            var json = Encoding.UTF8.GetString(content);
            var extracted = JsonConvert.DeserializeObject<TMessage>(json);

            try
            {
                await this.messageHandler(extracted);
            }
            catch (ResourceLockedException)
            {
                await this.queueClient!.AbandonAsync(message.SystemProperties.LockToken);
            }
            catch (Exception)
            {
                await this.queueClient!.DeadLetterAsync(message.SystemProperties.LockToken);
            }
        }
        
        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            this.exceptionHandler(arg.Exception);
            return Task.CompletedTask;
        }
    }
}