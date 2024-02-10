#pragma warning disable CS0649

using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Domain.Permission.Contracts.Interfaces;

using Permission = Domic.Domain.Permission.Entities.Permission;

namespace Domic.UseCase.PermissionUseCase.Commands.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand, string>
{
    private readonly object _validationResult;

    private readonly IDateTime                    _dateTime;
    private readonly ISerializer                  _serializer;
    private readonly IJsonWebToken                _jsonWebToken;
    private readonly IEventCommandRepository      _eventCommandRepository;
    private readonly IPermissionCommandRepository _permissionCommandRepository;

    public UpdateCommandHandler(IPermissionCommandRepository permissionCommandRepository,
        IEventCommandRepository eventCommandRepository, IDateTime dateTime, ISerializer serializer, 
        IJsonWebToken jsonWebToken
    )
    {
        _dateTime                    = dateTime;
        _serializer                  = serializer;
        _jsonWebToken                = jsonWebToken;
        _eventCommandRepository      = eventCommandRepository;
        _permissionCommandRepository = permissionCommandRepository;
    }

    [WithValidation]
    [WithTransaction]
    public Task<string> HandleAsync(UpdateCommand command, CancellationToken cancellationToken)
    {
        var updatedBy        = _jsonWebToken.GetIdentityUserId(command.Token);
        var updatedRole      = _serializer.Serialize( _jsonWebToken.GetRoles(command.Token) );
        var targetPermission = _validationResult as Permission;

        targetPermission.Change(_dateTime, updatedBy, updatedRole, command.Name, command.RoleId);

        _permissionCommandRepository.Change(targetPermission);

        return Task.FromResult(targetPermission.Id);
    }
}