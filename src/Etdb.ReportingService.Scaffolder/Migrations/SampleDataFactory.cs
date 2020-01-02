using System;
using System.Threading.Tasks;
using Etdb.ReportingService.Domain;
using Etdb.ReportingService.Repositories.Abstractions;

namespace Etdb.ReportingService.Scaffolder.Migrations
{
    public class SampleDataFactory
    {
        private readonly IUserRegistrationsRepository userRegistrationsRepository;

        private readonly UserRegistration sampleUserRegistration = new UserRegistration(Guid.NewGuid(), Guid.NewGuid(),
            "485174F4-1330-4159-8F03-5DE6E81E8B8A", DateTime.UtcNow);

        public SampleDataFactory(IUserRegistrationsRepository userRegistrationsRepository)
        {
            this.userRegistrationsRepository = userRegistrationsRepository;
        }

        public async Task CreateSampleDataAsync()
        {
            var existingSampleRegistration = await this.userRegistrationsRepository.FindAsync(userReg => userReg.UserName == this.sampleUserRegistration.UserName);

            if (existingSampleRegistration == null)
                await this.userRegistrationsRepository.AddAsync(this.sampleUserRegistration);
        }
    }
}