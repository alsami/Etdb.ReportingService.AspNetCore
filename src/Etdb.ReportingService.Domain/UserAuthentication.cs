using System;
using Etdb.ServiceBase.Domain.Abstractions.Documents;

namespace Etdb.ReportingService.Domain
{
    public class UserAuthentication : IDocument<Guid>
    {
        public Guid Id { get; private set; }
        
        public Guid UserId { get; private set; }

        public string UserName { get; private set; }

        public string AuthenticationLogType { get; private set; }

        public string IpAddress { get; private set; }

        public DateTime LoggedAt { get; private set; }

        public string? AdditionalInfo { get; private set; }

        public UserAuthentication(Guid id, Guid userId, string userName, string authenticationLogType, string ipAddress, DateTime loggedAt, string? additionalInfo)
        {
            this.Id = id;
            this.UserId = userId;
            this.UserName = userName;
            this.AuthenticationLogType = authenticationLogType;
            this.IpAddress = ipAddress;
            this.LoggedAt = loggedAt;
            this.AdditionalInfo = additionalInfo;
        }
    }
}