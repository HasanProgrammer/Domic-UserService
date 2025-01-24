using Domic.Core.Common.ClassExtensions;
using Domic.Core.Common.ClassHelpers;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.UseCase.PermissionUseCase.DTOs;

namespace Domic.UseCase.PermissionUseCase.Queries.ReadAllBasedOnRolesPaginated;

public class ReadAllBasedOnRolesPaginatedQueryHandler 
    : IQueryHandler<ReadAllBasedOnRolesPaginatedQuery, PaginatedCollection<PermissionDto>>
{
    private readonly IInternalDistributedCacheMediator _cacheMediator;

    public ReadAllBasedOnRolesPaginatedQueryHandler(IInternalDistributedCacheMediator cacheMediator) 
        => _cacheMediator = cacheMediator;
    
    [WithValidation]
    public async Task<PaginatedCollection<PermissionDto>> HandleAsync(ReadAllBasedOnRolesPaginatedQuery query, 
        CancellationToken cancellationToken
    )
    {
        var pageNumber   = Convert.ToInt32(query.PageNumber);
        var countPerPage = Convert.ToInt32(query.CountPerPage);
        
        var permissions = await _cacheMediator.GetAsync<List<PermissionDto>>(cancellationToken);

        var permissionsFiltered = permissions.Where(permission => query.Roles.Contains(permission.RoleId));
        
        return permissionsFiltered.ToPaginatedCollection(permissionsFiltered.Count(), countPerPage, pageNumber);
    }
}