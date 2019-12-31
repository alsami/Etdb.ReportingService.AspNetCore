using System;
using System.Threading.Tasks;
using Etdb.ReportingService.Services.Abstractions.Models;

namespace Etdb.ReportingService.Services.Abstractions
{
    public interface IResourceLockingAdapter
    {
        ValueTask<ResourceLock> LockAsync(string key, TimeSpan lockTimeSpan);
    }
}