using Karami.Core.Common.ClassExtensions;
using Karami.Core.Common.ClassHelpers;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.UseCase.UserUseCase.DTOs.ViewModels;

namespace Karami.UseCase.UserUseCase.Queries.ReadAllPaginated;

public class ReadAllPaginatedQueryHandler : IQueryHandler<ReadAllPaginatedQuery, PaginatedCollection<UsersViewModel>>
{
    private readonly ICacheService _cacheService;

    public ReadAllPaginatedQueryHandler(ICacheService cacheService) => _cacheService = cacheService;

    [WithValidation]
    public async Task<PaginatedCollection<UsersViewModel>> HandleAsync(ReadAllPaginatedQuery query, 
        CancellationToken cancellationToken
    )
    {
        int pageNumber   = Convert.ToInt32(query.PageNumber);
        int countPerPage = Convert.ToInt32(query.CountPerPage);

        var result = await _cacheService.GetAsync<List<UsersViewModel>>(cancellationToken);

        return result.ToPaginatedCollection(result.Count, countPerPage, pageNumber);
    }
}