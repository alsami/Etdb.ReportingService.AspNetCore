using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Etdb.ReportingService.Cqrs.Abstractions.Commands;
using Etdb.ReportingService.Domain;
using Etdb.ReportingService.Misc.Exceptions;
using Etdb.ReportingService.Repositories.Abstractions;
using Etdb.ReportingService.Services.Abstractions;
using MediatR;

namespace Etdb.ReportingService.Cqrs.CommandHandler
{
    public class UserRegistrationStoreCommandHandler : IRequestHandler<UserRegistrationStoreCommand>
    {
        private readonly IResourceLockingAdapter lockingAdapter;
        private readonly IUserRegistrationsRepository userRegistrationsRepository;
        private readonly IMapper mapper;
        
        public UserRegistrationStoreCommandHandler(IResourceLockingAdapter lockingAdapter, IUserRegistrationsRepository userRegistrationsRepository, IMapper mapper)
        {
            this.lockingAdapter = lockingAdapter;
            this.userRegistrationsRepository = userRegistrationsRepository;
            this.mapper = mapper;
        }

        public async Task<Unit> Handle(UserRegistrationStoreCommand command, CancellationToken cancellationToken)
        {
            await using var resourceLock = await this.lockingAdapter.LockAsync(GetLockKey(command), TimeSpan.FromSeconds(30));
            
            if (!resourceLock.Aquired) 
                throw new ResourceLockedException("Another process is already trying to store the user-registration.");

            var userRegistration = this.mapper.Map<UserRegistration>(command);

            await this.userRegistrationsRepository.AddAsync(userRegistration);
            
            return Unit.Value;
        }

        private static string GetLockKey(UserRegistrationStoreCommand command)
            => $"UserRegistrationStore_{command.UserId}";
    }
}