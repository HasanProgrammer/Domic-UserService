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
using Microsoft.Extensions.DependencyInjection;

namespace Domic.UseCase.UserUseCase.Commands.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand, string>
{
    private readonly object _validationResult;

    private readonly IDateTime                        _dateTime;
    private readonly ISerializer                      _serializer;
    private readonly IUserCommandRepository           _userCommandRepository;
    private readonly IRoleUserCommandRepository       _roleUserCommandRepository;
    private readonly IPermissionUserCommandRepository _permissionUserCommandRepository;
    private readonly IGlobalUniqueIdGenerator         _globalUniqueIdGenerator;
    private readonly IIdentityUser                    _identityUser;

    public UpdateCommandHandler(IUserCommandRepository userCommandRepository, 
        IRoleUserCommandRepository roleUserCommandRepository, 
        IPermissionUserCommandRepository permissionUserCommandRepository, IDateTime dateTime, ISerializer serializer,
        IGlobalUniqueIdGenerator globalUniqueIdGenerator, [FromKeyedServices("Http2")] IIdentityUser identityUser
    )
    {
        _dateTime                        = dateTime;
        _serializer                      = serializer;
        _userCommandRepository           = userCommandRepository;
        _roleUserCommandRepository       = roleUserCommandRepository;
        _permissionUserCommandRepository = permissionUserCommandRepository;
        _globalUniqueIdGenerator         = globalUniqueIdGenerator;
        _identityUser                    = identityUser;
    }

    public Task BeforeHandleAsync(UpdateCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(UpdateCommand command, CancellationToken cancellationToken)
    {
        var targetUser = _validationResult as User;
        
        var roleUsers = command.Roles.Select(role => new RoleUser(
            _globalUniqueIdGenerator, _dateTime, _identityUser, _serializer, command.Id, role
        ));
        
        var permissionUsers = command.Permissions.Select(permission => new PermissionUser(
            _globalUniqueIdGenerator, _dateTime, _identityUser, _serializer, command.Id, permission
        ));
        
        targetUser.Change(
            _dateTime           ,
            _identityUser       ,
            _serializer         ,
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

        await _userCommandRepository.ChangeAsync(targetUser, cancellationToken);
        await _roleUserCommandRepository.RemoveRangeAsync(targetUser.RoleUsers, cancellationToken);
        await _permissionUserCommandRepository.RemoveRangeAsync(targetUser.PermissionUsers, cancellationToken);
        await _roleUserCommandRepository.AddRangeAsync(roleUsers, cancellationToken);
        await _permissionUserCommandRepository.AddRangeAsync(permissionUsers, cancellationToken);

        return _serializer.Serialize(targetUser.PermissionUsers.Select(pu => pu.Permission.Name.Value));
    }

    public Task AfterHandleAsync(UpdateCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}