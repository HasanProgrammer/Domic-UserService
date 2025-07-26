using Domic.Core.Common.ClassExtensions;
using Domic.Core.Common.ClassHelpers;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Commons.Enumerations;
using Domic.UseCase.UserUseCase.DTOs;

namespace Domic.UseCase.UserUseCase.Queries.ReadAllPaginated;

public class ReadAllPaginatedQueryHandler : IQueryHandler<ReadAllPaginatedQuery, PaginatedCollection<UserDto>>
{
    private readonly IInternalDistributedCacheMediator _cacheMediator;

    public ReadAllPaginatedQueryHandler(IInternalDistributedCacheMediator cacheMediator) 
        => _cacheMediator = cacheMediator;

    [WithValidation]
    public async Task<PaginatedCollection<UserDto>> HandleAsync(ReadAllPaginatedQuery query, 
        CancellationToken cancellationToken
    )
    {
        int pageNumber   = Convert.ToInt32(query.PageNumber);
        int countPerPage = Convert.ToInt32(query.CountPerPage);

        var users = await _cacheMediator.GetAsync<List<UserDto>>(cancellationToken);

        var usersFiltered = users.Where(u => 
            string.IsNullOrEmpty(query.SearchText)   || 
            u.FirstName.Contains(query.SearchText)   ||
            u.LastName.Contains(query.SearchText)    ||
            u.Email.Contains(query.SearchText)       ||
            u.PhoneNumber.Contains(query.SearchText) ||
            u.Username.Contains(query.SearchText)
        );

        usersFiltered = query.Sort == Sort.Newest
            ? usersFiltered.OrderByDescending(user => user.CreatedAt) 
            : usersFiltered.OrderBy(user => user.CreatedAt);

        return usersFiltered.ToPaginatedCollection(usersFiltered.Count(), countPerPage, pageNumber);
    }
}