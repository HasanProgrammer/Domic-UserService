using Karami.Core.Common.ClassConsts;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Commons.Contracts.Interfaces;
using Karami.Domain.User.Events;

namespace Karami.UseCase.UserUseCase.Events;

public class DeleteUserConsumerEventBusHandler : IConsumerEventBusHandler<UserDeleted>
{
    private readonly IQueryUnitOfWork _queryUnitOfWork;

    public DeleteUserConsumerEventBusHandler(IQueryUnitOfWork queryUnitOfWork) => _queryUnitOfWork = queryUnitOfWork;
    
    [WithTransaction]
    [WithCleanCache(Keies = Cache.Users)]
    public void Handle(UserDeleted @event)
    {
        
    }
}