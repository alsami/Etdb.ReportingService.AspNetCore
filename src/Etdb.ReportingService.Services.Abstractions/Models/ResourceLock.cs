using System;
using System.Threading.Tasks;

namespace Etdb.ReportingService.Services.Abstractions.Models
{
    public abstract class ResourceLock : IAsyncDisposable
    {
        public bool Aquired { get; protected set; }
        public abstract ValueTask DisposeAsync();
    }
}