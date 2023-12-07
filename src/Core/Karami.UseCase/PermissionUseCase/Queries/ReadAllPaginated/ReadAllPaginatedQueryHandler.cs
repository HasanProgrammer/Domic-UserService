using Karami.Core.Common.ClassExtensions;
using Karami.Core.Common.ClassHelpers;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.UseCase.PermissionUseCase.DTOs.ViewModels;

namespace Karami.UseCase.PermissionUseCase.Queries.ReadAllPaginated;

public class ReadAllPaginatedQueryHandler : IQueryHandler<ReadAllPaginatedQuery, PaginatedCollection<PermissionsViewModel>>
{
    private readonly ICacheService _cacheService;

    public ReadAllPaginatedQueryHandler(ICacheService cacheService) => _cacheService = cacheService;

    [WithValidation]
    public async Task<PaginatedCollection<PermissionsViewModel>> HandleAsync(ReadAllPaginatedQuery query, 
        CancellationToken cancellationToken
    )
    {
        var pageNumber   = Convert.ToInt32(query.PageNumber);
        var countPerPage = Convert.ToInt32(query.CountPerPage);
        
        var result = await _cacheService.GetAsync<List<PermissionsViewModel>>(cancellationToken);
        
        return result.ToPaginatedCollection(result.Count, countPerPage, pageNumber, true);
    }
}