#pragma warning disable CS0649

using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.Domain.PermissionUser.Contracts.Interfaces;

using Permission = Domic.Domain.Permission.Entities.Permission;

namespace Domic.UseCase.PermissionUseCase.Commands.Delete;

public class DeleteCommandHandler : ICommandHandler<DeleteCommand, string>
{
    private readonly object  _validationResult;

    private readonly IDateTime                        _dateTime;
    private readonly ISerializer                      _serializer;
    private readonly IJsonWebToken                    _jsonWebToken;
    private readonly IPermissionCommandRepository     _permissionCommandRepository;
    private readonly IPermissionUserCommandRepository _permissionUserCommandRepository;
    private readonly IEventCommandRepository          _eventCommandRepository;

    public DeleteCommandHandler(IPermissionUserCommandRepository permissionUserCommandRepository,
        IPermissionCommandRepository permissionCommandRepository, IEventCommandRepository eventCommandRepository, 
        IDateTime dateTime, ISerializer serializer, IJsonWebToken jsonWebToken
    )
    {
        _dateTime                        = dateTime;
        _serializer                      = serializer;
        _jsonWebToken                    = jsonWebToken;
        _permissionCommandRepository     = permissionCommandRepository;
        _permissionUserCommandRepository = permissionUserCommandRepository;
        _eventCommandRepository          = eventCommandRepository;
    }

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(DeleteCommand command, CancellationToken cancellationToken)
    {
        var updateBy         = _jsonWebToken.GetIdentityUserId(command.Token);
        var updateRole       = _serializer.Serialize( _jsonWebToken.GetRoles(command.Token) );
        var targetPermission = _validationResult as Permission;

        #region HardDelete PermissionUser

        var permissionUsers = 
            await _permissionUserCommandRepository.FindAllByPermissionIdAsync(targetPermission.Id, cancellationToken);
        
        _permissionUserCommandRepository.RemoveRange(permissionUsers);

        #endregion

        #region SoftDelete Permission

        targetPermission.Delete(_dateTime, updateBy, updateRole);

        _permissionCommandRepository.Change(targetPermission);

        #endregion

        return targetPermission.Id;
    }
}