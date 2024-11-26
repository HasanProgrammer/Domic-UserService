#pragma warning disable CS0649

using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Domain.Permission.Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Permission = Domic.Domain.Permission.Entities.Permission;

namespace Domic.UseCase.PermissionUseCase.Commands.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand, string>
{
    private readonly object _validationResult;

    private readonly IDateTime                    _dateTime;
    private readonly ISerializer                  _serializer;
    private readonly IIdentityUser                _identityUser;
    private readonly IPermissionCommandRepository _permissionCommandRepository;

    public UpdateCommandHandler(IPermissionCommandRepository permissionCommandRepository, IDateTime dateTime, 
        ISerializer serializer, [FromKeyedServices("Http2")] IIdentityUser identityUser
    )
    {
        _dateTime                    = dateTime;
        _serializer                  = serializer;
        _identityUser                = identityUser;
        _permissionCommandRepository = permissionCommandRepository;
    }

    public Task BeforeHandleAsync(UpdateCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public Task<string> HandleAsync(UpdateCommand command, CancellationToken cancellationToken)
    {
        var targetPermission = _validationResult as Permission;

        targetPermission.Change(_dateTime, _identityUser, _serializer, command.Name, command.RoleId);

        _permissionCommandRepository.Change(targetPermission);

        return Task.FromResult(targetPermission.Id);
    }

    public Task AfterHandleAsync(UpdateCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}