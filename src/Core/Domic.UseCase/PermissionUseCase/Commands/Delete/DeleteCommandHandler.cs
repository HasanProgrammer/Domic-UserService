#pragma warning disable CS0649

using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.Domain.PermissionUser.Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Permission = Domic.Domain.Permission.Entities.Permission;

namespace Domic.UseCase.PermissionUseCase.Commands.Delete;

public class DeleteCommandHandler : ICommandHandler<DeleteCommand, string>
{
    private readonly object  _validationResult;

    private readonly IDateTime                        _dateTime;
    private readonly ISerializer                      _serializer;
    private readonly IIdentityUser                    _identityUser;
    private readonly IPermissionCommandRepository     _permissionCommandRepository;
    private readonly IPermissionUserCommandRepository _permissionUserCommandRepository;

    public DeleteCommandHandler(IPermissionUserCommandRepository permissionUserCommandRepository,
        IPermissionCommandRepository permissionCommandRepository, IDateTime dateTime,
        ISerializer serializer, [FromKeyedServices("http1")] IIdentityUser identityUser
    )
    {
        _dateTime                        = dateTime;
        _serializer                      = serializer;
        _identityUser                    = identityUser;
        _permissionCommandRepository     = permissionCommandRepository;
        _permissionUserCommandRepository = permissionUserCommandRepository;
    }

    public Task BeforeHandleAsync(DeleteCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(DeleteCommand command, CancellationToken cancellationToken)
    {
        var targetPermission = _validationResult as Permission;

        #region HardDeletePermissionUser

        var permissionUsers =
            await _permissionUserCommandRepository.FindAllByPermissionIdAsync(targetPermission.Id, cancellationToken);
        
        _permissionUserCommandRepository.RemoveRange(permissionUsers);

        #endregion

        #region SoftDeletePermission

        targetPermission.Delete(_dateTime, _identityUser, _serializer);

        _permissionCommandRepository.Change(targetPermission);

        #endregion

        return targetPermission.Id;
    }

    public Task AfterHandleAsync(DeleteCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}