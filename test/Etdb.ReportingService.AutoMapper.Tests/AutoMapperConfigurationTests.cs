using Autofac;
using AutoMapper;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Etdb.ReportingService.AutoMapper.Profiles;
using Xunit;

namespace Etdb.ReportingService.Tests
{
    public class AutoMapperConfigurationTests
    {
        [Fact]
        public void AutoMapper_Register_Validate_Does_Not_Throw()
        {
            var mapperConfiguration = new ContainerBuilder()
                .AddAutoMapper(typeof(UserRegistrationProfile).Assembly)
                .Build()
                .Resolve<MapperConfiguration>();
            
            mapperConfiguration.AssertConfigurationIsValid();
        }
    }
}
