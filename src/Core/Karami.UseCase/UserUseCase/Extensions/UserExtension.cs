using Karami.Domain.User.Entities;
using Karami.UseCase.PermissionUseCase.DTOs.ViewModels;
using Karami.UseCase.RoleUseCase.DTOs.ViewModels;
using Karami.UseCase.UserUseCase.DTOs.ViewModels;

namespace Karami.UseCase.UserUseCase.Extensions;

public static class UserExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public static UsersViewModel ToViewModel(this User model)
        => new() {
            Id          = model.Id                ,  
            FirstName   = model.FirstName.Value   ,
            LastName    = model.LastName.Value    ,
            Username    = model.Username.Value    ,
            PhoneNumber = model.PhoneNumber.Value ,
            Email       = model.Email.Value       ,
            Description = model.Description.Value ,
            Roles       = model.RoleUsers.Select(RoleUser => new RolesViewModel {
                Id   = RoleUser.Role.Id ,
                Name = RoleUser.Role.Name.Value 
            }).ToList() ,
            Permissions = model.PermissionUsers.Select(PermissionUser => new PermissionsViewModel {
                Id       = PermissionUser.PermissionId          ,
                Name     = PermissionUser.Permission.Name.Value ,
                RoleId   = PermissionUser.Permission.RoleId     ,
                RoleName = PermissionUser.Permission.Role.Name.Value 
            }).ToList()
        };
}