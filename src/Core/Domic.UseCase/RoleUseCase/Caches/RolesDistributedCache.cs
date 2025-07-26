using Domic.Core.Common.ClassConsts;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Role.Contracts.Interfaces;
using Domic.UseCase.PermissionUseCase.DTOs;
using Domic.UseCase.RoleUseCase.DTOs;

namespace Domic.UseCase.RoleUseCase.Caches;

public class RolesDistributedCache : IInternalDistributedCacheHandler<List<RoleDto>>
{
    private readonly IRoleQueryRepository _roleQueryRepository;

    public RolesDistributedCache(IRoleQueryRepository roleQueryRepository) => _roleQueryRepository = roleQueryRepository;

    [Config(Key = Cache.Roles, Ttl = 1)]
    public async Task<List<RoleDto>> SetAsync(CancellationToken cancellationToken)
    {
        var result = await _roleQueryRepository.FindAllEagerLoadingAsync(cancellationToken);

        return result.Select(role => new RoleDto {
            Id          = role.Id   ,
            Name        = role.Name ,
            Permissions = role.Permissions.Select(permission => new PermissionDto {
                Id   = permission.Id,
                Name = permission.Name 
            }),
            CreatedAt = role.CreatedAt_EnglishDate
        }).ToList();
    }
}