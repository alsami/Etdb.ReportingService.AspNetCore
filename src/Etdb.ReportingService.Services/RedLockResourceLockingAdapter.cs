using System;
using System.Threading.Tasks;
using Etdb.ReportingService.Services.Abstractions;
using Etdb.ReportingService.Services.Abstractions.Models;
using Etdb.ReportingService.Services.Models;
using RedLockNet;

namespace Etdb.ReportingService.Services
{
    public class RedLockResourceLockingAdapter : IResourceLockingAdapter
    {
        private readonly IDistributedLockFactory distributedLockFactory;

        public RedLockResourceLockingAdapter(IDistributedLockFactory distributedLockFactory)
        {
            this.distributedLockFactory = distributedLockFactory;
        }

        public async ValueTask<ResourceLock> LockAsync(string key, TimeSpan lockTimeSpan)
        {
            var redLock = await this.distributedLockFactory.CreateLockAsync(key, lockTimeSpan);
            
            return new RedLockResourceLock(redLock);
        }
    }
}