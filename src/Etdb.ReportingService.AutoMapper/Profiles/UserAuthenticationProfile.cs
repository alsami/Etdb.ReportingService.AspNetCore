using AutoMapper;
using Etdb.ReportingService.AutoMapper.Converters;
using Etdb.ReportingService.Cqrs.Abstractions.Commands;
using Etdb.ReportingService.Domain;

namespace Etdb.ReportingService.AutoMapper.Profiles
{
    public class UserAuthenticationProfile : Profile
    {
        public UserAuthenticationProfile()
        {
            this.CreateMap<UserAuthenticationStoreCommand, UserAuthentication>()
                .ConvertUsing<UserAuthenticationStoreCommandConverter>();
        }
    }
}