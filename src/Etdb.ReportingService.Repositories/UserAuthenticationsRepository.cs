using System;
using Etdb.ReportingService.Domain;
using Etdb.ReportingService.Repositories.Abstractions;
using Etdb.ServiceBase.DocumentRepository;

namespace Etdb.ReportingService.Repositories
{
    public class UserAuthenticationsRepository : GenericDocumentRepository<UserAuthentication, Guid>, IUserAuthenticationsRepository
    {
        public UserAuthenticationsRepository(DocumentDbContext context) : base(context)
        {
        }
    }
}