#pragma warning disable CS4014

using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.PermissionUser.Entities;
using Domic.Domain.RoleUser.Entities;
using Domic.Domain.User.Entities;
using Domic.Domain.PermissionUser.Contracts.Interfaces;
using Domic.Domain.RoleUser.Contracts.Interfaces;
using Domic.Domain.User.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.Create;

public class CreateCommandHandler : ICommandHandler<CreateCommand, string>
{
    private readonly IDateTime                        _dateTime;
    private readonly IJsonWebToken                    _jsonWebToken;
    private readonly ISerializer                      _serializer;
    private readonly IUserCommandRepository           _userCommandRepository;
    private readonly IRoleUserCommandRepository       _roleUserCommandRepository;
    private readonly IPermissionUserCommandRepository _permissionUserCommandRepository;
    private readonly IGlobalUniqueIdGenerator         _globalUniqueIdGenerator;

    public CreateCommandHandler(IUserCommandRepository userCommandRepository,
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

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        string userId   = _globalUniqueIdGenerator.GetRandom();
        var createdBy   = _jsonWebToken.GetIdentityUserId(command.Token);
        var createdRole = _serializer.Serialize( _jsonWebToken.GetRoles(command.Token) );
        
        var newUser = new User(
            _dateTime           ,
            userId              ,
            createdBy           ,
            createdRole         ,
            command.FirstName   ,
            command.LastName    ,
            command.Description ,
            command.Username    ,
            command.Password    ,
            command.PhoneNumber ,
            command.EMail       ,
            command.Roles       ,
            command.Permissions
        );
        
        await _userCommandRepository.AddAsync(newUser, cancellationToken);
        
        await _roleUserBuilderAsync(createdBy, createdRole, userId, command.Roles, cancellationToken);
        await _permissionUserBuilderAsync(createdBy, createdRole, userId, command.Permissions, cancellationToken);

        return userId;
    }

    /*---------------------------------------------------------------*/

    private async Task _roleUserBuilderAsync(string createdBy, string createdRole, 
        string userId, IEnumerable<string> roleIds, CancellationToken cancellationToken
    )
    {
        foreach (var roleId in roleIds)
        {
            var newRoleUser = new RoleUser(
                _dateTime, _globalUniqueIdGenerator.GetRandom(), createdBy, createdRole, userId, roleId
            );

            await _roleUserCommandRepository.AddAsync(newRoleUser, cancellationToken);
        }
    }
    
    private async Task _permissionUserBuilderAsync(string createdBy, string createdRole, string userId, 
        IEnumerable<string> permissionIds, CancellationToken cancellationToken
    )
    {
        foreach (var permissionId in permissionIds)
        {
            var newPermissionUser = new PermissionUser(
                _dateTime, _globalUniqueIdGenerator.GetRandom(), createdBy, createdRole, userId, permissionId
            );

            await _permissionUserCommandRepository.AddAsync(newPermissionUser, cancellationToken);
        }
    }
}