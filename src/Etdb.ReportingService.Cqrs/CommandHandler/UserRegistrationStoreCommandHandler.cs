using System.Threading;
using System.Threading.Tasks;
using Etdb.ReportingService.Cqrs.Abstractions.Commands;
using MediatR;

namespace Etdb.ReportingService.Cqrs.CommandHandler
{
    public class UserRegistrationStoreCommandHandler : IRequestHandler<UserRegistrationStoreCommand>
    {
        public Task<Unit> Handle(UserRegistrationStoreCommand request, CancellationToken cancellationToken) =>
            Unit.Task;
    }
}