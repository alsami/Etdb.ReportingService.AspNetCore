using System.Threading.Tasks;
using Etdb.ReportingService.Services.Abstractions.Models;

namespace Etdb.ReportingService.Services.Models
{
    public class MemoryResourceLock : ResourceLock
    {
        public MemoryResourceLock(bool aquired)
        {
            this.Aquired = aquired;
        }

        public override ValueTask DisposeAsync() => new ValueTask(Task.CompletedTask);
    }
}