#pragma warning disable CS0649

using Domic.UseCase.PermissionUseCase.DTOs.ViewModels;
using Domic.UseCase.RoleUseCase.DTOs.ViewModels;
using Domic.UseCase.UserUseCase.DTOs.ViewModels;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.User.Entities;

namespace Domic.UseCase.UserUseCase.Queries.ReadOne;

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