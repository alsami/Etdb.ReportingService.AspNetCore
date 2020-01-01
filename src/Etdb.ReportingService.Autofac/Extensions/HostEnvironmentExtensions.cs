using System;
using Microsoft.Extensions.Hosting;

namespace Etdb.ReportingService.Autofac.Extensions
{
    public static class HostEnvironmentExtensions
    {
        public static bool IsAzureDevelopment(this IHostEnvironment environment)
            => environment.EnvironmentName.Equals("AzureDev", StringComparison.InvariantCultureIgnoreCase);

        public static bool IsAzure(this IHostEnvironment environment)
            => environment.IsAzureDevelopment() || environment.EnvironmentName.Equals("Azure", StringComparison.InvariantCultureIgnoreCase);
    }
}