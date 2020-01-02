using System;
using Etdb.ReportingService.Domain;
using Etdb.ServiceBase.DocumentRepository.Abstractions;

namespace Etdb.ReportingService.Repositories.Abstractions
{
    public interface IUserAuthenticationsRepository : IDocumentRepository<UserAuthentication, Guid>
    {
        
    }
}