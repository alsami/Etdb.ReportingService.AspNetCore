using System;
using MediatR;

namespace Etdb.ReportingService.Cqrs.Abstractions.Commands
{
    public class UserAuthenticationStoreCommand : IRequest
    {
        public Guid UserId { get; }

        public string UserName { get; }

        public string AuthenticationLogType { get; }

        public string IpAddress { get; }

        public DateTime LoggedAt { get; }

        public string? AdditionalInfo { get; }

        public UserAuthenticationStoreCommand(Guid userId, string userName, string authenticationLogType, string ipAddress, DateTime loggedAt, string? additionalInfo)
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