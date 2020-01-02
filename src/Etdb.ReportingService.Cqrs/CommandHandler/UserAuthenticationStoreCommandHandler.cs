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
    public class UserAuthenticationStoreCommandHandler : IRequestHandler<UserAuthenticationStoreCommand>
    {
        private readonly IResourceLockingAdapter lockingAdapter;
        private readonly IUserAuthenticationsRepository userAuthenticationsRepository;
        private readonly IMapper mapper;
        
        public UserAuthenticationStoreCommandHandler(IResourceLockingAdapter lockingAdapter, IUserAuthenticationsRepository userAuthenticationsRepository, IMapper mapper)
        {
            this.lockingAdapter = lockingAdapter;
            this.userAuthenticationsRepository = userAuthenticationsRepository;
            this.mapper = mapper;
        }

        public async Task<Unit> Handle(UserAuthenticationStoreCommand command, CancellationToken cancellationToken)
        {
            await using var resourceLock = await this.lockingAdapter.LockAsync(GetLockKey(command), TimeSpan.FromSeconds(30));
            
            if (!resourceLock.Aquired) 
                throw new ResourceLockedException("Another process is already trying to store the user-authentication.");

            var userAuthentication = this.mapper.Map<UserAuthentication>(command);

            await this.userAuthenticationsRepository.AddAsync(userAuthentication);
            
            return Unit.Value;
        }

        private static string GetLockKey(UserAuthenticationStoreCommand command)
            => $"UserAuthenticationStore_{command.UserId}";
    }
}