using Domic.Domain.User.Entities;
using Domic.UseCase.PermissionUseCase.DTOs;
using Domic.UseCase.RoleUseCase.DTOs;
using Domic.UseCase.UserUseCase.DTOs;

namespace Domic.UseCase.UserUseCase.Extensions;

public static class UserExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public static UserDto ToDto(this User model)
        => new() {
            Id          = model.Id                ,  
            FirstName   = model.FirstName.Value   ,
            LastName    = model.LastName.Value    ,
            Username    = model.Username.Value    ,
            PhoneNumber = model.PhoneNumber.Value ,
            Email       = model.Email.Value       ,
            Description = model.Description.Value ,
            Roles       = model.RoleUsers.Select(RoleUser => new RoleDto {
                Id   = RoleUser.Role.Id ,
                Name = RoleUser.Role.Name.Value 
            }).ToList() ,
            Permissions = model.PermissionUsers.Select(PermissionUser => new PermissionDto {
                Id       = PermissionUser.PermissionId          ,
                Name     = PermissionUser.Permission.Name.Value ,
                RoleId   = PermissionUser.Permission.RoleId     ,
                RoleName = PermissionUser.Permission.Role.Name.Value 
            }).ToList()
        };
}