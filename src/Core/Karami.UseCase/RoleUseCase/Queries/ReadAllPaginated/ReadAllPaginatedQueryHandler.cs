using Karami.Core.Common.ClassExtensions;
using Karami.Core.Common.ClassHelpers;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.UseCase.RoleUseCase.DTOs.ViewModels;

namespace Karami.UseCase.RoleUseCase.Queries.ReadAllPaginated;

public class ReadAllPaginatedQueryHandler : IQueryHandler<ReadAllPaginatedQuery, PaginatedCollection<RolesViewModel>>
{
    private readonly ICacheService _cacheService;

    public ReadAllPaginatedQueryHandler(ICacheService cacheService) 
        => _cacheService = cacheService;

    [WithValidation]
    public async Task<PaginatedCollection<RolesViewModel>> HandleAsync(ReadAllPaginatedQuery query, 
        CancellationToken cancellationToken
    )
    {
        var pageNumber   = Convert.ToInt32(query.PageNumber);
        var countPerPage = Convert.ToInt32(query.CountPerPage);
        
        var result = await _cacheService.GetAsync<List<RolesViewModel>>(cancellationToken);

        return result.ToPaginatedCollection(result.Count, countPerPage, pageNumber);
    }
}