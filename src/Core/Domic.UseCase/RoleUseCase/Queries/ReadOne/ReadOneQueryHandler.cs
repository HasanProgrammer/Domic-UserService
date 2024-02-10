#pragma warning disable CS0649

using Domic.UseCase.PermissionUseCase.DTOs.ViewModels;
using Domic.UseCase.RoleUseCase.DTOs.ViewModels;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Role.Entities;

namespace Domic.UseCase.RoleUseCase.Queries.ReadOne;

public class ReadOneQueryHandler : IQueryHandler<ReadOneQuery , RolesViewModel>
{
    private readonly object _validationResult;
    
    [WithValidation]
    public async Task<RolesViewModel> HandleAsync(ReadOneQuery query, CancellationToken cancellationToken)
    {
        return await Task.Run(() => {

            var targetRoleQuery = _validationResult as RoleQuery;

            return new RolesViewModel {
                Id          = targetRoleQuery.Id   ,
                Name        = targetRoleQuery.Name ,
                Permissions = targetRoleQuery.Permissions.Select(permission => new PermissionsViewModel {
                    Id   = permission.Id ,
                    Name = permission.Name
                })
            };
            
        }, cancellationToken);
    }
}