using System;

namespace Etdb.ReportingService.Services.Abstractions.Models
{
    public class UserAuthenticatedMessage
    {
        public Guid UserId { get; }

        public string UserName { get; }

        public string AuthenticationLogType { get; }

        public string IpAddress { get; }

        public DateTime LoggedAt { get; }

        public string? AdditionalInfo { get; }

        public UserAuthenticatedMessage(Guid userId, string userName, string authenticationLogType, string ipAddress, DateTime loggedAt, string? additionalInfo)
        {
            this.UserId = userId;
            this.UserName = userName;
            this.AuthenticationLogType = authenticationLogType;
            this.IpAddress = ipAddress;
            this.LoggedAt = loggedAt;
            this.AdditionalInfo = additionalInfo;
        }
    }
}