using System;
using System.Threading.Tasks;
using Etdb.ReportingService.Services.Abstractions;
using Etdb.ReportingService.Services.Abstractions.Enums;

namespace Etdb.ReportingService.Services
{
    public class NoopMessageConsumer<TMessage> : IMessageConsumer<TMessage> where TMessage : class
    {
        public void RegisterHandler(MessageType messageType, Func<TMessage, Task> messageProcessorFunc, Action<Exception> exceptionHandlerFunc)
        {
            
        }
    }
}