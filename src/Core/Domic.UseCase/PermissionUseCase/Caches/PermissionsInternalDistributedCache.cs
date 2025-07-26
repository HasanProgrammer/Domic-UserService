using Domic.Core.Common.ClassConsts;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.UseCase.PermissionUseCase.DTOs;

namespace Domic.UseCase.PermissionUseCase.Caches;

public class PermissionsInternalDistributedCache : IInternalDistributedCacheHandler<List<PermissionDto>>
{
    private readonly IPermissionQueryRepository _permissionQueryRepository;

    public PermissionsInternalDistributedCache(IPermissionQueryRepository permissionQueryRepository) 
        => _permissionQueryRepository = permissionQueryRepository;

    [Config(Key = Cache.Permissions, Ttl = 1)]
    public async Task<List<PermissionDto>> SetAsync(CancellationToken cancellationToken)
    {
        var result = await _permissionQueryRepository.FindAllEagerLoadingAsync(cancellationToken);
        
        return result.Select(query => new PermissionDto {
            Id        = query.Id        ,
            RoleId    = query.RoleId    ,
            RoleName  = query.Role.Name ,
            Name      = query.Name      ,
            CreatedAt = query.CreatedAt_EnglishDate
        }).ToList();
    }
}