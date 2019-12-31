using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ReportingService.Services.Abstractions;
using Etdb.ReportingService.Services.Abstractions.Models;
using Etdb.ReportingService.Services.Models;
using Microsoft.Extensions.Logging;

namespace Etdb.ReportingService.Services
{
    public class InMemoryResourceLockingAdapter : IResourceLockingAdapter
    {
        private static readonly ConcurrentDictionary<string, DateTime> LockedKeysByDateTime
            = new ConcurrentDictionary<string, DateTime>();

        private readonly ILogger<InMemoryResourceLockingAdapter> logger;

        public InMemoryResourceLockingAdapter(ILogger<InMemoryResourceLockingAdapter> logger)
        {
            this.logger = logger;
            this.RunCleanupThread();
        }

        public ValueTask<ResourceLock> LockAsync(string key, TimeSpan lockTimeSpan)
        {
            var aquired = InMemoryResourceLockingAdapter.LockedKeysByDateTime.TryAdd(key, DateTime.UtcNow);

            return new ValueTask<ResourceLock>(new MemoryResourceLock(aquired));
        }
        
        private void RunCleanupThread()
        {
            var thread = new Thread(() =>
            {
                while (true)
                {
                    foreach (var key in InMemoryResourceLockingAdapter.LockedKeysByDateTime.Keys)
                    {
                        this.logger.LogInformation("Checking if key {key} needs to be removed", key);
                        if (InMemoryResourceLockingAdapter.LockedKeysByDateTime[key] >= DateTime.UtcNow) return;

                        this.logger.LogInformation("Removing value for key {key}!", key);
                        InMemoryResourceLockingAdapter.LockedKeysByDateTime.TryRemove(key, out _);
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(30));
                }
            });

            thread.Start();
        }
    }
}