#pragma warning disable CS0649

using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.User.Entities;
using Karami.UseCase.PermissionUseCase.DTOs.ViewModels;
using Karami.UseCase.RoleUseCase.DTOs.ViewModels;
using Karami.UseCase.UserUseCase.DTOs.ViewModels;

namespace Karami.UseCase.UserUseCase.Queries.ReadOne;

public class ReadOneQueryHandler : IQueryHandler<ReadOneQuery, UsersViewModel>
{
    private readonly object _validationResult;

    [WithValidation]
    public async Task<UsersViewModel> HandleAsync(ReadOneQuery query, CancellationToken cancellationToken)
    {
        return await Task.Run(() => {
            
            var targetUserQuery = _validationResult as UserQuery;
            
            return new UsersViewModel {
                Id          = targetUserQuery.Id          ,  
                FirstName   = targetUserQuery.FirstName   ,
                LastName    = targetUserQuery.LastName    ,
                Username    = targetUserQuery.Username    ,
                PhoneNumber = targetUserQuery.PhoneNumber ,
                Email       = targetUserQuery.Email       ,
                Description = targetUserQuery.Description ,
                Roles       = targetUserQuery.RoleUsers.Select(roleUser => new RolesViewModel {
                    Id   = roleUser.Role.Id,
                    Name = roleUser.Role.Name 
                }).ToList() ,
                Permissions = targetUserQuery.PermissionUsers.Select(permissionUser => new PermissionsViewModel {
                    Id       = permissionUser.PermissionId,
                    Name     = permissionUser.Permission.Name
                }).ToList()
            };
            
        }, cancellationToken);
    }
}