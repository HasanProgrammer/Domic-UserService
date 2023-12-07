#pragma warning disable CS0649

using Karami.Common.ClassConsts;
using Karami.Core.Common.ClassConsts;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Entities;
using Karami.Core.Domain.Extensions;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.UseCase.Attributes;
using Karami.Domain.Permission.Contracts.Interfaces;
using Karami.Domain.Permission.Entities;
using Karami.Domain.PermissionUser.Contracts.Interfaces;

using Action     = Karami.Core.Common.ClassConsts.Action;
using Permission = Karami.Domain.Permission.Entities.Permission;

namespace Karami.UseCase.PermissionUseCase.Commands.Delete;

public class DeleteCommandHandler : ICommandHandler<DeleteCommand, string>
{
    private readonly object  _validationResult;

    private readonly IDotrisDateTime                  _dotrisDateTime;
    private readonly ISerializer                      _serializer;
    private readonly IJsonWebToken                        _jsonWebToken;
    private readonly IPermissionCommandRepository     _permissionCommandRepository;
    private readonly IPermissionUserCommandRepository _permissionUserCommandRepository;
    private readonly IEventCommandRepository          _eventCommandRepository;

    public DeleteCommandHandler(IPermissionUserCommandRepository permissionUserCommandRepository,
        IPermissionCommandRepository permissionCommandRepository, 
        IEventCommandRepository eventCommandRepository, 
        IDotrisDateTime dotrisDateTime, 
        ISerializer serializer, 
        IJsonWebToken jsonWebToken
    )
    {
        _dotrisDateTime                  = dotrisDateTime;
        _serializer                      = serializer;
        _jsonWebToken                        = jsonWebToken;
        _permissionCommandRepository     = permissionCommandRepository;
        _permissionUserCommandRepository = permissionUserCommandRepository;
        _eventCommandRepository          = eventCommandRepository;
    }

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(DeleteCommand command, CancellationToken cancellationToken)
    {
        var targetPermission = _validationResult as Permission;

        #region HardDelete PermissionUser

        var permissionUsers = 
            await _permissionUserCommandRepository.FindAllByPermissionIdAsync(targetPermission.Id, cancellationToken);
        
        _permissionUserCommandRepository.RemoveRange(permissionUsers);

        #endregion

        #region SoftDelete Permission

        targetPermission.Delete(_dotrisDateTime);

        _permissionCommandRepository.Change(targetPermission);

        #endregion

        #region OutBox

        var events = targetPermission.GetEvents.ToEntityOfEvent(_dotrisDateTime, _serializer,
            Service.UserService, Table.PermissionTable, Action.Delete, _jsonWebToken.GetUsername(command.Token)
        );
        
        foreach (Event @event in events)
            await _eventCommandRepository.AddAsync(@event, cancellationToken);
        
        targetPermission.ClearEvents();

        #endregion

        return targetPermission.Id;
    }
}