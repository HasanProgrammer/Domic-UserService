using Domic.Core.Common.ClassConsts;
using Domic.Core.Domain.Enumerations;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.User.Contracts.Interfaces;
using Domic.UseCase.UserUseCase.DTOs;

namespace Domic.UseCase.UserUseCase.Caches;

public class UsersEagerLoadingMemoryCache : IInternalDistributedCacheHandler<List<UserDto>>
{
    private readonly IUserQueryRepository _userQueryRepository;

    public UsersEagerLoadingMemoryCache(IUserQueryRepository userQueryRepository)
        => _userQueryRepository = userQueryRepository;

    [Config(Key = Cache.Users, Ttl = 60)]
    public async Task<List<UserDto>> SetAsync(CancellationToken cancellationToken)
    {
        var result =
            await _userQueryRepository.FindAllWithOrderingAsync(Order.Date, false, cancellationToken);

        return result.Select(user => new UserDto {
            Id          = user.Id          ,
            Username    = user.Username    ,
            FirstName   = user.FirstName   ,
            LastName    = user.LastName    ,
            Email       = user.Email       ,
            Description = user.Description ,
            PhoneNumber = user.PhoneNumber ,
            IsActive    = user.IsActive == IsActive.Active,
            CreatedAt   = user.CreatedAt_EnglishDate
        }).ToList();
    }
}