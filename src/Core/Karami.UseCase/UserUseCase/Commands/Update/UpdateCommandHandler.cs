#pragma warning disable CS0649

using Karami.Common.ClassConsts;
using Karami.Core.Common.ClassConsts;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.PermissionUser.Entities;
using Karami.Domain.RoleUser.Entities;
using Karami.Core.Domain.Entities;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Extensions;
using Karami.Core.UseCase.Attributes;
using Karami.Domain.PermissionUser.Contracts.Interfaces;
using Karami.Domain.RoleUser.Contracts.Interfaces;
using Karami.Domain.User.Contracts.Interfaces;
using Karami.Domain.User.Entities;

using Action = Karami.Core.Common.ClassConsts.Action;

namespace Karami.UseCase.UserUseCase.Commands.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand, string>
{
    private readonly object _validationResult;

    private readonly IDotrisDateTime                  _dotrisDateTime;
    private readonly ISerializer                      _serializer;
    private readonly IJsonWebToken                    _jsonWebToken;
    private readonly IUserCommandRepository           _userCommandRepository;
    private readonly IRoleUserCommandRepository       _roleUserCommandRepository;
    private readonly IPermissionUserCommandRepository _permissionUserCommandRepository;
    private readonly IEventCommandRepository          _eventCommandRepository;

    public UpdateCommandHandler(IUserCommandRepository userCommandRepository, 
        IRoleUserCommandRepository roleUserCommandRepository,
        IPermissionUserCommandRepository permissionUserCommandRepository,
        IEventCommandRepository eventCommandRepository, 
        IDotrisDateTime dotrisDateTime, 
        ISerializer serializer, 
        IJsonWebToken jsonWebToken
    )
    {
        _dotrisDateTime                  = dotrisDateTime;
        _serializer                      = serializer;
        _jsonWebToken                    = jsonWebToken;
        _userCommandRepository           = userCommandRepository;
        _roleUserCommandRepository       = roleUserCommandRepository;
        _permissionUserCommandRepository = permissionUserCommandRepository;
        _eventCommandRepository          = eventCommandRepository;
    }

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(UpdateCommand command, CancellationToken cancellationToken)
    {
        var targetUser = _validationResult as User;
        
        targetUser.Change(
            _dotrisDateTime     ,
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
        
        await _roleUserBuilderAsync(targetUser.Id, command.Roles, cancellationToken);
        await _permissionUserBuilderAsync(targetUser.Id, command.Permissions, cancellationToken);

        #region OutBox

        var events = targetUser.GetEvents.ToEntityOfEvent(_dotrisDateTime, _serializer, Service.UserService,
            Table.UserTable, Action.Update, _jsonWebToken.GetUsername(command.Token)
        );

        foreach (Event @event in events)
            await _eventCommandRepository.AddAsync(@event, cancellationToken);
        
        targetUser.ClearEvents();

        #endregion

        return targetUser.Id;
    }

    /*---------------------------------------------------------------*/

    private async Task _roleUserBuilderAsync(string userId , IEnumerable<string> roleIds, 
        CancellationToken cancellationToken
    )
    {
        //1 . Remove already user roles
        foreach (
            var roleUser in await _roleUserCommandRepository.FindAllByUserIdAsync(userId, cancellationToken)
        ) _roleUserCommandRepository.Remove(roleUser);
        
        //2 . Assign new roles for user
        foreach (var roleId in roleIds)
        {
            var newRoleUser = new RoleUser(_dotrisDateTime, Guid.NewGuid().ToString(), userId, roleId);

            await _roleUserCommandRepository.AddAsync(newRoleUser, cancellationToken);
        }
    }
    
    private async Task _permissionUserBuilderAsync(string userId , IEnumerable<string> permissionIds, 
        CancellationToken cancellationToken
    )
    {
        //1 . Remove already user permissions
        foreach (
            var permissionUser in await _permissionUserCommandRepository.FindAllByUserIdAsync(userId, cancellationToken)
        ) _permissionUserCommandRepository.Remove(permissionUser);
        
        //2 . Assign new permissions for user
        foreach (var permissionId in permissionIds)
        {
            var newPermissionUser = new PermissionUser(_dotrisDateTime, Guid.NewGuid().ToString(), userId, permissionId);

            await _permissionUserCommandRepository.AddAsync(newPermissionUser, cancellationToken);
        }
    }
}