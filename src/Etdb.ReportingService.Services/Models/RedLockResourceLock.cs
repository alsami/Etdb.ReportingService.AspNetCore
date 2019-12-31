using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Etdb.ReportingService.Services.Abstractions.Models;
using RedLockNet;

namespace Etdb.ReportingService.Services.Models
{
    public class RedLockResourceLock : ResourceLock
    {
        private readonly IRedLock @lock;

        public RedLockResourceLock(IRedLock @lock)
        {
            this.@lock = @lock;
            this.Aquired = @lock.IsAcquired;
        }

        public override ValueTask DisposeAsync()
        {
            this.@lock.Dispose();
            
            return new ValueTask(Task.CompletedTask);
        }
    }
}