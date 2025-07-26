using Domic.Core.Common.ClassExtensions;
using Domic.Core.Common.ClassHelpers;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Commons.Enumerations;
using Domic.UseCase.PermissionUseCase.DTOs;

namespace Domic.UseCase.PermissionUseCase.Queries.ReadAllPaginated;

public class ReadAllPaginatedQueryHandler : IQueryHandler<ReadAllPaginatedQuery, PaginatedCollection<PermissionDto>>
{
    private readonly IInternalDistributedCacheMediator _cacheMediator;

    public ReadAllPaginatedQueryHandler(IInternalDistributedCacheMediator cacheMediator) 
        => _cacheMediator = cacheMediator;

    [WithValidation]
    public async Task<PaginatedCollection<PermissionDto>> HandleAsync(ReadAllPaginatedQuery query, 
        CancellationToken cancellationToken
    )
    {
        var pageNumber   = Convert.ToInt32(query.PageNumber);
        var countPerPage = Convert.ToInt32(query.CountPerPage);
        
        var result = await _cacheMediator.GetAsync<List<PermissionDto>>(cancellationToken);
        
        var permissionsFiltered = result.Where(permission =>
            string.IsNullOrEmpty(query.SearchText)     || 
            permission.Name.Contains(query.SearchText) ||
            permission.RoleName.Contains(query.SearchText)
        );

        permissionsFiltered = query.Sort == Sort.Newest
            ? permissionsFiltered.OrderByDescending(permission => permission.CreatedAt)
            : permissionsFiltered.OrderBy(permission => permission.CreatedAt);
        
        return permissionsFiltered.ToPaginatedCollection(result.Count, countPerPage, pageNumber, true);
    }
}