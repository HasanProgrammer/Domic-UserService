using Domic.Core.Common.ClassExtensions;
using Domic.Core.Common.ClassHelpers;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Commons.Enumerations;
using Domic.UseCase.RoleUseCase.DTOs;

namespace Domic.UseCase.RoleUseCase.Queries.ReadAllPaginated;

public class ReadAllPaginatedQueryHandler : IQueryHandler<ReadAllPaginatedQuery, PaginatedCollection<RoleDto>>
{
    private readonly IInternalDistributedCacheMediator _cacheMediator;

    public ReadAllPaginatedQueryHandler(IInternalDistributedCacheMediator cacheMediator) 
        => _cacheMediator = cacheMediator;

    [WithValidation]
    public async Task<PaginatedCollection<RoleDto>> HandleAsync(ReadAllPaginatedQuery query, 
        CancellationToken cancellationToken
    )
    {
        var pageNumber   = Convert.ToInt32(query.PageNumber);
        var countPerPage = Convert.ToInt32(query.CountPerPage);
        
        var result = await _cacheMediator.GetAsync<List<RoleDto>>(cancellationToken);
        
        var rolesFiltered = result.Where(u =>
            string.IsNullOrEmpty(query.SearchText) || u.Name.Contains(query.SearchText)
        );

        rolesFiltered = query.Sort == Sort.Newest
            ? rolesFiltered.OrderByDescending(role => role.CreatedAt)
            : rolesFiltered.OrderBy(role => role.CreatedAt);

        return rolesFiltered.ToPaginatedCollection(result.Count, countPerPage, pageNumber);
    }
}