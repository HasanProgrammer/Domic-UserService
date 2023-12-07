#pragma warning disable CS0649

using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Permission.Entities;
using Karami.UseCase.PermissionUseCase.DTOs.ViewModels;

namespace Karami.UseCase.PermissionUseCase.Queries.ReadOne;

public class ReadOneQueryHandler : IQueryHandler<ReadOneQuery, PermissionsViewModel>
{
    private readonly object _validationResult;

    [WithValidation]
    public async Task<PermissionsViewModel> HandleAsync(ReadOneQuery query, CancellationToken cancellationToken)
    {
        return await Task.Run(() => {

            var targetPermissionQuery = _validationResult as PermissionQuery;

            return new PermissionsViewModel {
                Id       = targetPermissionQuery.Id     ,
                Name     = targetPermissionQuery.Name   ,
                RoleId   = targetPermissionQuery.RoleId ,
                RoleName = targetPermissionQuery.Role.Name
            };
            
        }, cancellationToken);
    }
}