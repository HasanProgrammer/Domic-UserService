using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Permission.Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Permission = Domic.Domain.Permission.Entities.Permission;

namespace Domic.UseCase.PermissionUseCase.Commands.Create;

public class CreateCommandHandler : ICommandHandler<CreateCommand, string>
{
    private readonly IDateTime                    _dateTime;
    private readonly ISerializer                  _serializer;
    private readonly IPermissionCommandRepository _permissionCommandRepository;
    private readonly IGlobalUniqueIdGenerator     _globalUniqueIdGenerator;
    private readonly IIdentityUser                _identityUser;

    public CreateCommandHandler(IPermissionCommandRepository permissionCommandRepository, IDateTime dateTime,
        ISerializer serializer, IGlobalUniqueIdGenerator globalUniqueIdGenerator, 
        [FromKeyedServices("Http1")] IIdentityUser identityUser
    )
    {
        _dateTime                    = dateTime;
        _serializer                  = serializer;
        _permissionCommandRepository = permissionCommandRepository;
        _globalUniqueIdGenerator     = globalUniqueIdGenerator;
        _identityUser                = identityUser;
    }

    public Task BeforeHandleAsync(CreateCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        var permission = new Permission(_globalUniqueIdGenerator, _dateTime, _identityUser, _serializer,
            command.Name, command.RoleId
        );

        await _permissionCommandRepository.AddAsync(permission, cancellationToken);

        return permission.Id;
    }

    public Task AfterHandleAsync(CreateCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}