using System;
using Etdb.ServiceBase.Domain.Abstractions.Documents;

namespace Etdb.ReportingService.Domain
{
    public class UserRegistration : IDocument<Guid>
    {
        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public string UserName { get; private set; }

        public DateTime RegisteredSince { get; private set; }

        public UserRegistration(Guid id, Guid userId, string userName, DateTime registeredSince)
        {
            this.Id = id;
            this.UserId = userId;
            this.UserName = userName;
            this.RegisteredSince = registeredSince;
        }
    }
}