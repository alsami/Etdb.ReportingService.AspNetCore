using System;

namespace Etdb.ReportingService.Services.Abstractions.Models
{
    public class UserRegisteredMessage
    {
        public Guid UserId { get; }

        public string UserName { get; }

        public DateTime RegisteredAt { get; }

        public UserRegisteredMessage(Guid userId, string userName, DateTime registeredAt)
        {
            this.UserId = userId;
            this.UserName = userName;
            this.RegisteredAt = registeredAt;
        }
    }
}