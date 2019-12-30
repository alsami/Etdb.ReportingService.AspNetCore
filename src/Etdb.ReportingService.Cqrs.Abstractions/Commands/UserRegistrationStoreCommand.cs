using System;
using MediatR;

namespace Etdb.ReportingService.Cqrs.Abstractions.Commands
{
    public class UserRegistrationStoreCommand : IRequest
    {
        public Guid UserId { get; }

        public string UserName { get; }

        public DateTime RegisteredAt { get; }

        public UserRegistrationStoreCommand(Guid userId, string userName, DateTime registeredAt)
        {
            this.UserId = userId;
            this.UserName = userName;
            this.RegisteredAt = registeredAt;
        }
    }
}