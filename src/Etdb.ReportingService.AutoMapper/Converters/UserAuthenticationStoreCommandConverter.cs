using System;
using AutoMapper;
using Etdb.ReportingService.Cqrs.Abstractions.Commands;
using Etdb.ReportingService.Domain;

namespace Etdb.ReportingService.AutoMapper.Converters
{
    public class UserAuthenticationStoreCommandConverter : ITypeConverter<UserAuthenticationStoreCommand, UserAuthentication>
    {
        public UserRegistration Convert(UserRegistrationStoreCommand source, UserRegistration destination, ResolutionContext context)
            => new UserRegistration(Guid.NewGuid(), source.UserId, source.UserName, source.RegisteredAt);

        public UserAuthentication Convert(UserAuthenticationStoreCommand source, UserAuthentication destination,
            ResolutionContext context) 
            => new UserAuthentication(Guid.NewGuid(), source.UserId, source.UserName, source.AuthenticationLogType, source.IpAddress, source.LoggedAt, source.AdditionalInfo); 
    }
}