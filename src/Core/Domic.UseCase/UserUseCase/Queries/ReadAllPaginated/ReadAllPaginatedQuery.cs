using Domic.Core.Common.ClassHelpers;
using Domic.Core.UseCase.Contracts.Abstracts;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.UseCase.UserUseCase.DTOs;

namespace Domic.UseCase.UserUseCase.Queries.ReadAllPaginated;

public class ReadAllPaginatedQuery : PaginatedQuery, IQuery<PaginatedCollection<UserDto>>
{
    public string FirstName   { get; set; }
    public string LastName    { get; set; }
    public string Username    { get; set; }
    public string PhoneNumber { get; set; }
    public string Email       { get; set; }
}