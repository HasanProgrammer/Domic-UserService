#pragma warning disable CS0649

using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Permission.Entities;
using Domic.UseCase.PermissionUseCase.DTOs;

namespace Domic.UseCase.PermissionUseCase.Queries.ReadOne;

public class ReadOneQueryHandler : IQueryHandler<ReadOneQuery, PermissionDto>
{
    private readonly object _validationResult;

    [WithValidation]
    public Task<PermissionDto> HandleAsync(ReadOneQuery query, CancellationToken cancellationToken)
    {
        var targetPermissionQuery = _validationResult as PermissionQuery;

        return Task.FromResult(new PermissionDto {
            Id = targetPermissionQuery.Id,
            Name = targetPermissionQuery.Name,
            RoleId = targetPermissionQuery.RoleId,
            RoleName = targetPermissionQuery.Role.Name
        });
    }
}