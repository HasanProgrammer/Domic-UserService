#pragma warning disable CS0649

using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Role.Entities;
using Karami.UseCase.PermissionUseCase.DTOs.ViewModels;
using Karami.UseCase.RoleUseCase.DTOs.ViewModels;

namespace Karami.UseCase.RoleUseCase.Queries.ReadOne;

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