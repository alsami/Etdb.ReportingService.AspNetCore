using System;
using Etdb.ReportingService.Domain;
using Etdb.ReportingService.Repositories.Abstractions;
using Etdb.ServiceBase.DocumentRepository;

namespace Etdb.ReportingService.Repositories
{
    public class UserRegistrationsRepository : GenericDocumentRepository<UserRegistration, Guid>, IUserRegistrationsRepository
    {
        public UserRegistrationsRepository(DocumentDbContext context) : base(context)
        {
        }
    }
}