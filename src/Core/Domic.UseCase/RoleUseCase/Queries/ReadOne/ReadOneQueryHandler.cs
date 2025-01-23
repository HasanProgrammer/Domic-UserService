#pragma warning disable CS0649

using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Role.Entities;
using Domic.UseCase.PermissionUseCase.DTOs;
using Domic.UseCase.RoleUseCase.DTOs;

namespace Domic.UseCase.RoleUseCase.Queries.ReadOne;

public class ReadOneQueryHandler : IQueryHandler<ReadOneQuery , RoleDto>
{
    private readonly object _validationResult;
    
    [WithValidation]
    public Task<RoleDto> HandleAsync(ReadOneQuery query, CancellationToken cancellationToken)
    {
        var targetRoleQuery = _validationResult as RoleQuery;

        return Task.FromResult(new RoleDto {
            Id          = targetRoleQuery.Id   ,
            Name        = targetRoleQuery.Name ,
            Permissions = targetRoleQuery.Permissions.Select(permission => new PermissionDto {
                Id   = permission.Id ,
                Name = permission.Name
            })
        });
    }
}