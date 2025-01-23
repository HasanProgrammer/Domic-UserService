using Domic.UseCase.UserUseCase.DTOs.ViewModels;
using Domic.Core.Common.ClassExtensions;
using Domic.Core.Common.ClassHelpers;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Queries.ReadAllPaginated;

public class ReadAllPaginatedQueryHandler : IQueryHandler<ReadAllPaginatedQuery, PaginatedCollection<UsersDto>>
{
    private readonly IInternalDistributedCacheMediator _cacheMediator;

    public ReadAllPaginatedQueryHandler(IInternalDistributedCacheMediator cacheMediator) 
        => _cacheMediator = cacheMediator;

    [WithValidation]
    public async Task<PaginatedCollection<UsersDto>> HandleAsync(ReadAllPaginatedQuery query, 
        CancellationToken cancellationToken
    )
    {
        int pageNumber   = Convert.ToInt32(query.PageNumber);
        int countPerPage = Convert.ToInt32(query.CountPerPage);

        var users = await _cacheMediator.GetAsync<List<UsersDto>>(cancellationToken);

        var usersFiltered = users.Where(u => 
            ( string.IsNullOrEmpty(query.FirstName) || u.FirstName == query.FirstName ) &&
            ( string.IsNullOrEmpty(query.LastName) || u.LastName == query.LastName ) &&
            ( string.IsNullOrEmpty(query.Username) || u.Username == query.Username ) &&
            ( string.IsNullOrEmpty(query.PhoneNumber) || u.PhoneNumber == query.PhoneNumber ) &&
            ( string.IsNullOrEmpty(query.Email) || u.Email == query.Email )
        );

        return usersFiltered.ToPaginatedCollection(usersFiltered.Count(), countPerPage, pageNumber);
    }
}