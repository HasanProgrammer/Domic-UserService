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

namespace Karami.UseCase.PermissionUseCase.Commands.Create;

public class CreateCommandHandler : ICommandHandler<CreateCommand, string>
{
    private readonly IDateTime                    _dateTime;
    private readonly ISerializer                  _serializer;
    private readonly IJsonWebToken                _jsonWebToken;
    private readonly IEventCommandRepository      _eventCommandRepository;
    private readonly IPermissionCommandRepository _permissionCommandRepository;
    private readonly IGlobalUniqueIdGenerator     _globalUniqueIdGenerator;

    public CreateCommandHandler(IPermissionCommandRepository permissionCommandRepository,
        IEventCommandRepository eventCommandRepository, IDateTime dateTime, IJsonWebToken jsonWebToken,
        ISerializer serializer, IGlobalUniqueIdGenerator globalUniqueIdGenerator
    )
    {
        _dateTime                    = dateTime;
        _serializer                  = serializer;
        _jsonWebToken                = jsonWebToken;
        _eventCommandRepository      = eventCommandRepository;
        _permissionCommandRepository = permissionCommandRepository;
        _globalUniqueIdGenerator     = globalUniqueIdGenerator;
    }

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        var createdBy       = _jsonWebToken.GetIdentityUserId(command.Token);
        var createdRole     = _serializer.Serialize( _jsonWebToken.GetRoles(command.Token) );
        string permissionId = _globalUniqueIdGenerator.GetRandom();
        
        var permission = new Permission(_dateTime, permissionId, createdBy, createdRole, command.Name, command.RoleId);

        #region OutBox

        var events = permission.GetEvents.ToEntityOfEvent(_dateTime, _serializer, Service.UserService, 
            Table.PermissionTable, Action.Create, _jsonWebToken.GetUsername(command.Token)
        );

        foreach (Event @event in events)
            await _eventCommandRepository.AddAsync(@event, cancellationToken);
        
        permission.ClearEvents();

        #endregion

        await _permissionCommandRepository.AddAsync(permission, cancellationToken);

        return permission.Id;
    }
}