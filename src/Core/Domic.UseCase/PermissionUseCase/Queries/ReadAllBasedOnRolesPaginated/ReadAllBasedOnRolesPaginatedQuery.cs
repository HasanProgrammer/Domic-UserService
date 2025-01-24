using Domic.Core.Common.ClassHelpers;
using Domic.Core.UseCase.Contracts.Abstracts;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.UseCase.PermissionUseCase.DTOs;

namespace Domic.UseCase.PermissionUseCase.Queries.ReadAllBasedOnRolesPaginated;

public class ReadAllBasedOnRolesPaginatedQuery : PaginatedQuery, IQuery<PaginatedCollection<PermissionDto>>
{
    public List<string> Roles { get; set; }
}