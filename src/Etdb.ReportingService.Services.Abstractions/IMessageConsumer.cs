using System;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ReportingService.Services.Abstractions.Enums;

namespace Etdb.ReportingService.Services.Abstractions
{
    public interface IMessageConsumer<out TMessage> where TMessage : class
    {
        public void RegisterHandler(MessageType messageType, Func<TMessage, Task> messageProcessorFunc, Action<Exception> exceptionHandlerFunc);
    }
}