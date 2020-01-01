using System;
using AutoMapper;
using Etdb.ReportingService.Cqrs.Abstractions.Commands;
using Etdb.ReportingService.Domain;

namespace Etdb.ReportingService.AutoMapper.Converters
{
    public class UserRegistrationStoreCommandConverter : ITypeConverter<UserRegistrationStoreCommand, UserRegistration>
    {
        public UserRegistration Convert(UserRegistrationStoreCommand source, UserRegistration destination, ResolutionContext context)
            => new UserRegistration(Guid.NewGuid(), source.UserId, source.UserName, source.RegisteredAt);
    }
}