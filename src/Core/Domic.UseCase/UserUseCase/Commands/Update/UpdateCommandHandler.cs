#pragma warning disable CS0649

using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.PermissionUser.Contracts.Interfaces;
using Domic.Domain.PermissionUser.Entities;
using Domic.Domain.RoleUser.Contracts.Interfaces;
using Domic.Domain.RoleUser.Entities;
using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Entities;

namespace Domic.UseCase.UserUseCase.Commands.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand, string>
{
    private readonly object _validationResult;

    private readonly IDateTime                        _dateTime;
    private readonly ISerializer                      _serializer;
    private readonly IJsonWebToken                    _jsonWebToken;
    private readonly IUserCommandRepository           _userCommandRepository;
    private readonly IRoleUserCommandRepository       _roleUserCommandRepository;
    private readonly IPermissionUserCommandRepository _permissionUserCommandRepository;
    private readonly IGlobalUniqueIdGenerator         _globalUniqueIdGenerator;

    public UpdateCommandHandler(IUserCommandRepository userCommandRepository, 
        IRoleUserCommandRepository roleUserCommandRepository, 
        IPermissionUserCommandRepository permissionUserCommandRepository, IDateTime dateTime, ISerializer serializer, 
        IJsonWebToken jsonWebToken, IGlobalUniqueIdGenerator globalUniqueIdGenerator
    )
    {
        _dateTime                        = dateTime;
        _serializer                      = serializer;
        _jsonWebToken                    = jsonWebToken;
        _userCommandRepository           = userCommandRepository;
        _roleUserCommandRepository       = roleUserCommandRepository;
        _permissionUserCommandRepository = permissionUserCommandRepository;
        _globalUniqueIdGenerator         = globalUniqueIdGenerator;
    }

    public Task BeforeHandleAsync(UpdateCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(UpdateCommand command, CancellationToken cancellationToken)
    {
        var targetUser  = _validationResult as User;
        var updatedBy   = _jsonWebToken.GetIdentityUserId(command.Token);
        var updatedRole = _serializer.Serialize( _jsonWebToken.GetRoles(command.Token) );
        
        targetUser.Change(
            _dateTime           ,
            updatedBy           ,
            updatedRole         ,
            command.FirstName   ,
            command.LastName    ,
            command.Description ,
            command.Username    ,
            command.EMail       ,
            command.PhoneNumber ,
            command.Roles       ,
            command.Permissions ,
            command.Password
        );

        _userCommandRepository.Change(targetUser);
        
        await _roleUserBuilderAsync(updatedBy, updatedRole, targetUser.Id, command.Roles, cancellationToken);
        await _permissionUserBuilderAsync(updatedBy, updatedRole, targetUser.Id, command.Permissions, cancellationToken);

        return targetUser.Id;
    }

    public Task AfterHandleAsync(UpdateCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;

    /*---------------------------------------------------------------*/

    private async Task _roleUserBuilderAsync(string createdBy, string createdRole, string userId, 
        IEnumerable<string> roleIds, CancellationToken cancellationToken
    )
    {
        //1 . Remove already user roles
        foreach (
            var roleUser in await _roleUserCommandRepository.FindAllByUserIdAsync(userId, cancellationToken)
        ) _roleUserCommandRepository.Remove(roleUser);
        
        //2 . Assign new roles for user
        foreach (var roleId in roleIds)
        {
            var newRoleUser = new RoleUser(
                _dateTime, _globalUniqueIdGenerator.GetRandom(), createdBy, createdRole, userId, roleId
            );

            await _roleUserCommandRepository.AddAsync(newRoleUser, cancellationToken);
        }
    }
    
    private async Task _permissionUserBuilderAsync(string createdBy, string createdRole, string userId , 
        IEnumerable<string> permissionIds, CancellationToken cancellationToken
    )
    {
        //1 . Remove already user permissions
        foreach (
            var permissionUser in await _permissionUserCommandRepository.FindAllByUserIdAsync(userId, cancellationToken)
        ) _permissionUserCommandRepository.Remove(permissionUser);
        
        //2 . Assign new permissions for user
        foreach (var permissionId in permissionIds)
        {
            var newPermissionUser = new PermissionUser(
                _dateTime,_globalUniqueIdGenerator.GetRandom(), createdBy, createdRole, userId, permissionId
            );

            await _permissionUserCommandRepository.AddAsync(newPermissionUser, cancellationToken);
        }
    }
}