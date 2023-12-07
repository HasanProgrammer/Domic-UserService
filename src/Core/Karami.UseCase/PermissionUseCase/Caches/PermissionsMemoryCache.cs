using Karami.Core.Common.ClassConsts;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Permission.Contracts.Interfaces;
using Karami.UseCase.PermissionUseCase.DTOs.ViewModels;

namespace Karami.UseCase.PermissionUseCase.Caches;

public class PermissionsMemoryCache : IMemoryCacheSetter<List<PermissionsViewModel>>
{
    private readonly IPermissionQueryRepository _permissionQueryRepository;

    public PermissionsMemoryCache(IPermissionQueryRepository permissionQueryRepository) 
        => _permissionQueryRepository = permissionQueryRepository;

    [Config(Key = Cache.Permissions, Ttl = 1)]
    public async Task<List<PermissionsViewModel>> SetAsync(CancellationToken cancellationToken)
    {
        var result = await _permissionQueryRepository.FindAllAsync(cancellationToken);
        
        return result.Select(query => new PermissionsViewModel {
            Id     = query.Id     ,
            RoleId = query.RoleId ,
            Name   = query.Name
        }).ToList();
    }
}