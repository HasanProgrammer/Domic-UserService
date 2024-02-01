#pragma warning disable CS0649

using Karami.Common.ClassConsts;
using Karami.Core.Common.ClassConsts;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.Domain.Entities;
using Karami.Core.Domain.Extensions;
using Karami.Core.UseCase.Attributes;
using Karami.Domain.Permission.Contracts.Interfaces;
using Karami.Domain.PermissionUser.Contracts.Interfaces;
using Karami.Domain.Role.Contracts.Interfaces;
using Karami.Domain.Role.Entities;
using Karami.Domain.RoleUser.Contracts.Interfaces;

using Action = Karami.Core.Common.ClassConsts.Action;

namespace Karami.UseCase.RoleUseCase.Commands.Delete;

public class DeleteCommandHandler : ICommandHandler<DeleteCommand, string>
{
    private readonly object _validationResult;

    private readonly IDateTime                       _dateTime;
    private readonly ISerializer                     _serializer;
    private readonly IJsonWebToken                   _jsonWebToken;
    private readonly IRoleCommandRepository           _roleCommandRepository;
    private readonly IPermissionCommandRepository     _permissionCommandRepository;
    private readonly IRoleUserCommandRepository       _roleUserCommandRepository;
    private readonly IPermissionUserCommandRepository _permissionUserCommandRepository;
    private readonly IEventCommandRepository          _eventCommandRepository;

    public DeleteCommandHandler(IRoleCommandRepository roleCommandRepository,
        IPermissionCommandRepository permissionCommandRepository, IRoleUserCommandRepository roleUserCommandRepository,
        IPermissionUserCommandRepository permissionUserCommandRepository, 
        IEventCommandRepository eventCommandRepository, IDateTime dateTime, ISerializer serializer, 
        IJsonWebToken jsonWebToken
    )
    {
        _dateTime                        = dateTime;
        _serializer                      = serializer;
        _jsonWebToken                    = jsonWebToken;
        _roleCommandRepository           = roleCommandRepository;
        _permissionCommandRepository     = permissionCommandRepository;
        _roleUserCommandRepository       = roleUserCommandRepository;
        _permissionUserCommandRepository = permissionUserCommandRepository;
        _eventCommandRepository          = eventCommandRepository;
    }

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(DeleteCommand command, CancellationToken cancellationToken)
    {
        var targetRole  = _validationResult as Role;
        var updatedBy   = _jsonWebToken.GetIdentityUserId(command.Token);
        var updatedRole = _serializer.Serialize( _jsonWebToken.GetRoles(command.Token) );

        #region SoftDelete Role

        targetRole.Delete(_dateTime, updatedBy, updatedRole);
        
        _roleCommandRepository.Change(targetRole);

        #endregion

        #region SoftDelete Permission

        var permissions =
            await _permissionCommandRepository.FindByRoleIdAsync(command.RoleId, cancellationToken);

        foreach (var permission in permissions)
        {
            permission.Delete(_dateTime, updatedBy, updatedRole, false);
            
            _permissionCommandRepository.Change(permission);
        }

        #endregion

        #region HardDelete RoleUser

        var roleUsers = await _roleUserCommandRepository.FindAllByRoleIdAsync(command.RoleId, cancellationToken);
        
        _roleUserCommandRepository.RemoveRange(roleUsers);

        #endregion

        #region HardDelete PermissionUser

        foreach (var permission in permissions)
        {
            var permissionUsers = 
                await _permissionUserCommandRepository.FindAllByPermissionIdAsync(permission.Id, cancellationToken);
        
            _permissionUserCommandRepository.RemoveRange(permissionUsers);
        }

        #endregion

        #region OutBox

        var events = targetRole.GetEvents.ToEntityOfEvent(_dateTime, _serializer, Service.UserService, 
            Table.RoleTable, Action.Delete, _jsonWebToken.GetUsername(command.Token)
        );

        foreach (Event @event in events)
            await _eventCommandRepository.AddAsync(@event, cancellationToken);
        
        #endregion

        return command.RoleId;
    }
}