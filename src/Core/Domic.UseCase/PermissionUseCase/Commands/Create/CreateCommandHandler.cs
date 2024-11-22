using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Permission.Contracts.Interfaces;

using Permission = Domic.Domain.Permission.Entities.Permission;

namespace Domic.UseCase.PermissionUseCase.Commands.Create;

public class CreateCommandHandler : ICommandHandler<CreateCommand, string>
{
    private readonly IDateTime                    _dateTime;
    private readonly ISerializer                  _serializer;
    private readonly IJsonWebToken                _jsonWebToken;
    private readonly IPermissionCommandRepository _permissionCommandRepository;
    private readonly IGlobalUniqueIdGenerator     _globalUniqueIdGenerator;

    public CreateCommandHandler(IPermissionCommandRepository permissionCommandRepository, IDateTime dateTime,
        IJsonWebToken jsonWebToken, ISerializer serializer, IGlobalUniqueIdGenerator globalUniqueIdGenerator
    )
    {
        _dateTime                    = dateTime;
        _serializer                  = serializer;
        _jsonWebToken                = jsonWebToken;
        _permissionCommandRepository = permissionCommandRepository;
        _globalUniqueIdGenerator     = globalUniqueIdGenerator;
    }

    public Task BeforeHandleAsync(CreateCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        var createdBy       = _jsonWebToken.GetIdentityUserId(command.Token);
        var createdRole     = _serializer.Serialize( _jsonWebToken.GetRoles(command.Token) );
        string permissionId = _globalUniqueIdGenerator.GetRandom();
        
        var permission = new Permission(_dateTime, permissionId, createdBy, createdRole, command.Name, command.RoleId);

        await _permissionCommandRepository.AddAsync(permission, cancellationToken);

        return permission.Id;
    }

    public Task AfterHandleAsync(CreateCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}