#pragma warning disable CS0649

using Domic.UseCase.PermissionUseCase.DTOs.ViewModels;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Permission.Entities;

namespace Domic.UseCase.PermissionUseCase.Queries.ReadOne;

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