#pragma warning disable CS0649

using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.User.Entities;
using Domic.UseCase.PermissionUseCase.DTOs;
using Domic.UseCase.RoleUseCase.DTOs;
using Domic.UseCase.UserUseCase.DTOs;

namespace Domic.UseCase.UserUseCase.Queries.ReadOne;

public class ReadOneQueryHandler : IQueryHandler<ReadOneQuery, UserDto>
{
    private readonly object _validationResult;

    [WithValidation]
    public Task<UserDto> HandleAsync(ReadOneQuery query, CancellationToken cancellationToken)
    {
        var targetUserQuery = _validationResult as UserQuery;

        return Task.FromResult<UserDto>(new UserDto {
            Id          = targetUserQuery.Id          ,
            FirstName   = targetUserQuery.FirstName   ,
            LastName    = targetUserQuery.LastName    ,
            Username    = targetUserQuery.Username    ,
            PhoneNumber = targetUserQuery.PhoneNumber ,
            Email       = targetUserQuery.Email       ,
            Description = targetUserQuery.Description ,
            Roles       = targetUserQuery.RoleUsers.Select(roleUser => new RoleDto {
                Id   = roleUser.Role.Id,
                Name = roleUser.Role.Name 
            }).ToList() ,
            Permissions = targetUserQuery.PermissionUsers.Select(permissionUser => new PermissionDto {
                Id       = permissionUser.PermissionId,
                Name     = permissionUser.Permission.Name
            }).ToList()
        });
    }
}