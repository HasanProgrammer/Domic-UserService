using Domic.Core.Common.ClassConsts;
using Domic.Core.Domain.Enumerations;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.User.Contracts.Interfaces;
using Domic.UseCase.UserUseCase.DTOs.ViewModels;

namespace Domic.UseCase.UserUseCase.Caches;

public class UsersEagerLoadingMemoryCache : IMemoryCacheSetter<List<UsersDto>>
{
    private readonly IUserQueryRepository _userQueryRepository;

    public UsersEagerLoadingMemoryCache(IUserQueryRepository userQueryRepository)
        => _userQueryRepository = userQueryRepository;

    [Config(Key = Cache.Users, Ttl = 60)]
    public async Task<List<UsersDto>> SetAsync(CancellationToken cancellationToken)
    {
        var result =
            await _userQueryRepository.FindAllWithOrderingAsync(Order.Id, false, cancellationToken);

        return result.Select(user => new UsersDto {
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