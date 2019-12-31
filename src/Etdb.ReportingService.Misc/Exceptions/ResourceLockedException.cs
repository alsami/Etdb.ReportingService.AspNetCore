using System;

namespace Etdb.ReportingService.Misc.Exceptions
{
    public class ResourceLockedException : Exception
    {
        public ResourceLockedException(string message) : base(message)
        {
            
        }
    }
}