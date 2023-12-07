#pragma warning disable CS4014

using Karami.Common.ClassConsts;
using Karami.Core.Common.ClassConsts;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.PermissionUser.Entities;
using Karami.Domain.RoleUser.Entities;
using Karami.Domain.User.Entities;
using Karami.Core.Domain.Entities;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Extensions;
using Karami.Core.UseCase.Attributes;
using Karami.Domain.PermissionUser.Contracts.Interfaces;
using Karami.Domain.RoleUser.Contracts.Interfaces;
using Karami.Domain.User.Contracts.Interfaces;

using Action = Karami.Core.Common.ClassConsts.Action;

namespace Karami.UseCase.UserUseCase.Commands.Create;

public class CreateCommandHandler : ICommandHandler<CreateCommand, string>
{
    private readonly IDotrisDateTime                  _dotrisDateTime;
    private readonly IJsonWebToken                    _jsonWebToken;
    private readonly ISerializer                      _serializer;
    private readonly IUserCommandRepository           _userCommandRepository;
    private readonly IRoleUserCommandRepository       _roleUserCommandRepository;
    private readonly IPermissionUserCommandRepository _permissionUserCommandRepository;
    private readonly IEventCommandRepository          _eventCommandRepository;

    public CreateCommandHandler(IUserCommandRepository userCommandRepository,
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
    public async Task<string> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        string userId = Guid.NewGuid().ToString();
        
        var newUser = new User(
            _dotrisDateTime     ,
            userId              ,
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
        
        await _roleUserBuilderAsync(userId, command.Roles, cancellationToken);
        await _permissionUserBuilderAsync(userId, command.Permissions, cancellationToken);

        #region OutBox

        var events = newUser.GetEvents.ToEntityOfEvent(_dotrisDateTime, _serializer, Service.UserService, 
            Table.UserTable, Action.Create, _jsonWebToken.GetUsername(command.Token)
        );

        foreach (Event @event in events)
            await _eventCommandRepository.AddAsync(@event, cancellationToken);
        
        newUser.ClearEvents();

        #endregion

        return userId;
    }

    /*---------------------------------------------------------------*/

    private async Task _roleUserBuilderAsync(string userId, IEnumerable<string> roleIds, 
        CancellationToken cancellationToken
    )
    {
        foreach (var roleId in roleIds)
        {
            var newRoleUser = new RoleUser(_dotrisDateTime, Guid.NewGuid().ToString(), userId, roleId);

            await _roleUserCommandRepository.AddAsync(newRoleUser, cancellationToken);
        }
    }
    
    private async Task _permissionUserBuilderAsync(string userId, IEnumerable<string> permissionIds, 
        CancellationToken cancellationToken
    )
    {
        foreach (var permissionId in permissionIds)
        {
            var newPermissionUser = new PermissionUser(_dotrisDateTime, Guid.NewGuid().ToString(), userId, permissionId);

            await _permissionUserCommandRepository.AddAsync(newPermissionUser, cancellationToken);
        }
    }
}