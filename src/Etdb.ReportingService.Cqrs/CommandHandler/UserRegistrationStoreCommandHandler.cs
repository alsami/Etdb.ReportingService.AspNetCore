using System;
using System.Threading;
using System.Threading.Tasks;
using Etdb.ReportingService.Cqrs.Abstractions.Commands;
using Etdb.ReportingService.Misc.Exceptions;
using Etdb.ReportingService.Services.Abstractions;
using Etdb.ReportingService.Services.Abstractions.Models;
using MediatR;

namespace Etdb.ReportingService.Cqrs.CommandHandler
{
    public class UserRegistrationStoreCommandHandler : IRequestHandler<UserRegistrationStoreCommand>
    {
        private readonly IResourceLockingAdapter lockingAdapter;
        
        public UserRegistrationStoreCommandHandler(IResourceLockingAdapter lockingAdapter)
        {
            this.lockingAdapter = lockingAdapter;
        }

        public async Task<Unit> Handle(UserRegistrationStoreCommand request, CancellationToken cancellationToken)
        {
            await using var resourceLock = await this.lockingAdapter.LockAsync(GetLockKey(request), TimeSpan.FromSeconds(30));
            
            if (!resourceLock.Aquired) 
                throw new ResourceLockedException("Another process is already trying to store the user-registration.");
            
            throw new NotImplementedException();
        }

        private static string GetLockKey(UserRegistrationStoreCommand command)
            => $"UserRegistrationStore_{command.UserId}";
    }
}