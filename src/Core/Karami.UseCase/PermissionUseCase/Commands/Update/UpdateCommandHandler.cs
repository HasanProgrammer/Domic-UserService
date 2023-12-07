#pragma warning disable CS0649

using Karami.Common.ClassConsts;
using Karami.Core.Common.ClassConsts;
using Karami.Core.Domain.Entities;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Extensions;
using Karami.Core.UseCase.Attributes;
using Karami.Domain.Permission.Contracts.Interfaces;

using Action     = Karami.Core.Common.ClassConsts.Action;
using Permission = Karami.Domain.Permission.Entities.Permission;

namespace Karami.UseCase.PermissionUseCase.Commands.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand, string>
{
    private readonly object _validationResult;

    private readonly IDotrisDateTime              _dotrisDateTime;
    private readonly ISerializer                  _serializer;
    private readonly IJsonWebToken                    _jsonWebToken;
    private readonly IEventCommandRepository      _eventCommandRepository;
    private readonly IPermissionCommandRepository _permissionCommandRepository;

    public UpdateCommandHandler(IPermissionCommandRepository permissionCommandRepository,
        IEventCommandRepository eventCommandRepository, 
        IDotrisDateTime dotrisDateTime,
        ISerializer serializer, 
        IJsonWebToken jsonWebToken
    )
    {
        _dotrisDateTime              = dotrisDateTime;
        _serializer                  = serializer;
        _jsonWebToken                    = jsonWebToken;
        _eventCommandRepository      = eventCommandRepository;
        _permissionCommandRepository = permissionCommandRepository;
    }

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(UpdateCommand command, CancellationToken cancellationToken)
    {
        var targetPermission = _validationResult as Permission;

        targetPermission.Change(_dotrisDateTime, command.Name, command.RoleId);

        _permissionCommandRepository.Change(targetPermission);
        
        #region OutBox

        var events = targetPermission.GetEvents.ToEntityOfEvent(_dotrisDateTime, _serializer,
            Service.UserService, Table.PermissionTable, Action.Update, _jsonWebToken.GetUsername(command.Token)
        );

        foreach (Event @event in events)
            await _eventCommandRepository.AddAsync(@event, cancellationToken);
        
        targetPermission.ClearEvents();

        #endregion

        return targetPermission.Id;
    }
}