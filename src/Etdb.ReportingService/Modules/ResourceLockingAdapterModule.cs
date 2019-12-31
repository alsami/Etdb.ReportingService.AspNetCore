using System.Collections.Generic;
using System.Net;
using Autofac;
using Etdb.ReportingService.Services;
using Etdb.ReportingService.Services.Abstractions;
using Etdb.ReportingService.Services.Abstractions.Models;
using Microsoft.Extensions.Logging;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

namespace Etdb.ReportingService.Modules
{
    public class ResourceLockingAdapterModule : Module
    {
        private readonly bool useDistributedLock;

        public ResourceLockingAdapterModule(bool useDistributedLock = false)
        {
            this.useDistributedLock = useDistributedLock;
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (this.useDistributedLock)
            {
                builder.Register<IDistributedLockFactory>(ctx => RedLockFactory.Create(new List<RedLockEndPoint>
                    {
                        new DnsEndPoint("localhost", 6397)
                    }, ctx.ResolveOptional<ILoggerFactory>()))
                    .SingleInstance();

                builder.RegisterType<RedLockResourceLockingAdapter>()
                    .As<IResourceLockingAdapter>()
                    .InstancePerLifetimeScope();
                
                return;
            }

            builder.RegisterType<InMemoryResourceLockingAdapter>()
                .As<IResourceLockingAdapter>()
                .SingleInstance();
        }
    }
}