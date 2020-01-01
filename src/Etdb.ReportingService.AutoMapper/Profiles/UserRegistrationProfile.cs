using AutoMapper;
using Etdb.ReportingService.AutoMapper.Converters;
using Etdb.ReportingService.Cqrs.Abstractions.Commands;
using Etdb.ReportingService.Domain;

namespace Etdb.ReportingService.AutoMapper.Profiles
{
    public class UserRegistrationProfile : Profile
    {
        public UserRegistrationProfile()
        {
            this.CreateMap<UserRegistrationStoreCommand, UserRegistration>()
                .ConvertUsing<UserRegistrationStoreCommandConverter>();
        }
    }
}