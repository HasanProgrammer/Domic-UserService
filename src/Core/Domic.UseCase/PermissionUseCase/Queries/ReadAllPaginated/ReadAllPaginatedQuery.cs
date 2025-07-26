using Domic.Core.Common.ClassHelpers;
using Domic.Core.UseCase.Contracts.Abstracts;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Commons.Enumerations;
using Domic.UseCase.PermissionUseCase.DTOs;

namespace Domic.UseCase.PermissionUseCase.Queries.ReadAllPaginated;

public class ReadAllPaginatedQuery : PaginatedQuery, IQuery<PaginatedCollection<PermissionDto>>
{
    public Sort Sort          { get; set; }
    public string SearchText  { get; set; }
}