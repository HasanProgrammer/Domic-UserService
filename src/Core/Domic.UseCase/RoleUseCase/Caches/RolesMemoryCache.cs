using Domic.UseCase.PermissionUseCase.DTOs.ViewModels;
using Domic.UseCase.RoleUseCase.DTOs.ViewModels;
using Domic.Core.Common.ClassConsts;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Role.Contracts.Interfaces;

namespace Domic.UseCase.RoleUseCase.Caches;

public class RolesMemoryCache : IMemoryCacheSetter<List<RolesViewModel>>
{
    private readonly IRoleQueryRepository _roleQueryRepository;

    public RolesMemoryCache(IRoleQueryRepository roleQueryRepository) => _roleQueryRepository = roleQueryRepository;

    [Config(Key = Cache.Roles, Ttl = 1)]
    public async Task<List<RolesViewModel>> SetAsync(CancellationToken cancellationToken)
    {
        var result = await _roleQueryRepository.FindAllEagerLoadingAsync(cancellationToken);

        return result.Select(role => new RolesViewModel {
            Id          = role.Id   ,
            Name        = role.Name ,
            Permissions = role.Permissions.Select(permission => new PermissionsViewModel {
                Id   = permission.Id,
                Name = permission.Name 
            })
        }).ToList();
    }
}