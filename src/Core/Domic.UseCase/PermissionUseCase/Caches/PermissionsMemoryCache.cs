using Domic.Core.Common.ClassConsts;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.UseCase.PermissionUseCase.DTOs.ViewModels;

namespace Domic.UseCase.PermissionUseCase.Caches;

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