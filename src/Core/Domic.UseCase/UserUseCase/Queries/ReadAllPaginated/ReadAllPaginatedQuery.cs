using Domic.UseCase.UserUseCase.DTOs.ViewModels;
using Domic.Core.Common.ClassHelpers;
using Domic.Core.UseCase.Contracts.Abstracts;
using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Queries.ReadAllPaginated;

public class ReadAllPaginatedQuery : PaginatedQuery, IQuery<PaginatedCollection<UsersDto>>
{
    public string FirstName   { get; set; }
    public string LastName    { get; set; }
    public string Username    { get; set; }
    public string PhoneNumber { get; set; }
    public string Email       { get; set; }
}