#pragma warning disable CS0649

using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.Domain.PermissionUser.Contracts.Interfaces;
using Domic.Domain.Role.Contracts.Interfaces;
using Domic.Domain.Role.Entities;
using Domic.Domain.RoleUser.Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Domic.UseCase.RoleUseCase.Commands.Delete;

public class DeleteCommandHandler : ICommandHandler<DeleteCommand, string>
{
    private readonly object _validationResult;

    private readonly IDateTime                        _dateTime;
    private readonly ISerializer                      _serializer;
    private readonly IIdentityUser                    _identityUser;
    private readonly IRoleCommandRepository           _roleCommandRepository;
    private readonly IPermissionCommandRepository     _permissionCommandRepository;
    private readonly IRoleUserCommandRepository       _roleUserCommandRepository;
    private readonly IPermissionUserCommandRepository _permissionUserCommandRepository;

    public DeleteCommandHandler(IRoleCommandRepository roleCommandRepository,
        IPermissionCommandRepository permissionCommandRepository, IRoleUserCommandRepository roleUserCommandRepository,
        IPermissionUserCommandRepository permissionUserCommandRepository, IDateTime dateTime, ISerializer serializer,
        [FromKeyedServices("Http2")] IIdentityUser identityUser
    )
    {
        _dateTime                        = dateTime;
        _serializer                      = serializer;
        _identityUser                    = identityUser;
        _roleCommandRepository           = roleCommandRepository;
        _permissionCommandRepository     = permissionCommandRepository;
        _roleUserCommandRepository       = roleUserCommandRepository;
        _permissionUserCommandRepository = permissionUserCommandRepository;
    }

    public Task BeforeHandleAsync(DeleteCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(DeleteCommand command, CancellationToken cancellationToken)
    {
        var targetRole  = _validationResult as Role;

        #region SoftDeleteRole

        targetRole.Delete(_dateTime, _identityUser, _serializer);
        
        _roleCommandRepository.Change(targetRole);

        #endregion

        #region SoftDeletePermission

        var permissions =
            await _permissionCommandRepository.FindByRoleIdAsync(command.RoleId, cancellationToken);

        foreach (var permission in permissions)
        {
            permission.Delete(_dateTime, _identityUser, _serializer, false);
            
            _permissionCommandRepository.Change(permission);
        }

        #endregion

        #region HardDeleteRoleUser

        var roleUsers = await _roleUserCommandRepository.FindAllByRoleIdAsync(command.RoleId, cancellationToken);
        
        _roleUserCommandRepository.RemoveRange(roleUsers);

        #endregion

        #region HardDeletePermissionUser

        foreach (var permission in permissions)
        {
            var permissionUsers = 
                await _permissionUserCommandRepository.FindAllByPermissionIdAsync(permission.Id, cancellationToken);
        
            _permissionUserCommandRepository.RemoveRange(permissionUsers);
        }

        #endregion

        return command.RoleId;
    }

    public Task AfterHandleAsync(DeleteCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}