using Domic.Core.Common.ClassHelpers;
using Domic.Core.UseCase.Contracts.Abstracts;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Commons.Enumerations;
using Domic.UseCase.UserUseCase.DTOs;

namespace Domic.UseCase.UserUseCase.Queries.ReadAllPaginated;

public class ReadAllPaginatedQuery : PaginatedQuery, IQuery<PaginatedCollection<UserDto>>
{
    public Sort Sort          { get; set; }
    public string SearchText  { get; set; }
}