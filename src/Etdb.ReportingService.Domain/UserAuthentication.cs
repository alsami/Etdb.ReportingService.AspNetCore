using System;
using Etdb.ServiceBase.Domain.Abstractions.Documents;

namespace Etdb.ReportingService.Domain
{
    public class UserAuthentication : IDocument<Guid>
    {
        public Guid Id { get; private set; }
    }
}