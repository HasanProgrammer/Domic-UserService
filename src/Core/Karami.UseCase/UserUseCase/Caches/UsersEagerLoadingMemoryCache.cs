using Karami.Core.Common.ClassConsts;
using Karami.Core.Domain.Enumerations;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.User.Contracts.Interfaces;
using Karami.UseCase.UserUseCase.DTOs.ViewModels;

namespace Karami.UseCase.UserUseCase.Caches;

public class UsersEagerLoadingMemoryCache : IMemoryCacheSetter<List<UsersViewModel>>
{
    private readonly IUserQueryRepository _userQueryRepository;

    public UsersEagerLoadingMemoryCache(IUserQueryRepository userQueryRepository)
        => _userQueryRepository = userQueryRepository;

    [Config(Key = Cache.Users, Ttl = 60)]
    public async Task<List<UsersViewModel>> SetAsync(CancellationToken cancellationToken)
    {
        var result =
            await _userQueryRepository.FindAllWithOrderingAsync(Order.Id, false, cancellationToken);

        return result.Select(user => new UsersViewModel {
            Id          = user.Id          ,
            Username    = user.Username    ,
            FirstName   = user.FirstName   ,
            LastName    = user.LastName    ,
            Email       = user.Email       ,
            Description = user.Description ,
            PhoneNumber = user.PhoneNumber
        }).ToList();
    }
}