using Domic.UseCase.UserUseCase.DTOs.ViewModels;
using Domic.Core.Common.ClassExtensions;
using Domic.Core.Common.ClassHelpers;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Queries.ReadAllPaginated;

public class ReadAllPaginatedQueryHandler : IQueryHandler<ReadAllPaginatedQuery, PaginatedCollection<UsersDto>>
{
    private readonly ICacheService _cacheService;

    public ReadAllPaginatedQueryHandler(ICacheService cacheService) => _cacheService = cacheService;

    [WithValidation]
    public async Task<PaginatedCollection<UsersDto>> HandleAsync(ReadAllPaginatedQuery query, 
        CancellationToken cancellationToken
    )
    {
        int pageNumber   = Convert.ToInt32(query.PageNumber);
        int countPerPage = Convert.ToInt32(query.CountPerPage);

        var result = await _cacheService.GetAsync<List<UsersDto>>(cancellationToken);

        return result.ToPaginatedCollection(result.Count, countPerPage, pageNumber);
    }
}