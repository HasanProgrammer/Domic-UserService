using Karami.Core.Common.ClassConsts;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Role.Contracts.Interfaces;
using Karami.UseCase.PermissionUseCase.DTOs.ViewModels;
using Karami.UseCase.RoleUseCase.DTOs.ViewModels;

namespace Karami.UseCase.RoleUseCase.Caches;

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