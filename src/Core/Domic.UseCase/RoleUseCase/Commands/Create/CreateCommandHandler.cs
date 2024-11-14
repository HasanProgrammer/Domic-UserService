using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Role.Entities;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Domain.Role.Contracts.Interfaces;

namespace Domic.UseCase.RoleUseCase.Commands.Create;

public class CreateCommandHandler : ICommandHandler<CreateCommand, string>
{
    private readonly IDateTime                _dateTime;
    private readonly ISerializer              _serializer;
    private readonly IJsonWebToken            _jsonWebToken;
    private readonly IRoleCommandRepository   _roleCommandRepository;
    private readonly IGlobalUniqueIdGenerator _globalUniqueIdGenerator;

    public CreateCommandHandler(IRoleCommandRepository roleCommandRepository, IDateTime dateTime, 
        ISerializer serializer, IJsonWebToken jsonWebToken, IGlobalUniqueIdGenerator globalUniqueIdGenerator
    )
    {
        _dateTime                = dateTime;
        _serializer              = serializer;
        _jsonWebToken            = jsonWebToken;
        _roleCommandRepository   = roleCommandRepository;
        _globalUniqueIdGenerator = globalUniqueIdGenerator;
    }

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        string roleId   = _globalUniqueIdGenerator.GetRandom();
        var createdBy   = _jsonWebToken.GetIdentityUserId(command.Token);
        var createdRole = _serializer.Serialize( _jsonWebToken.GetRoles(command.Token) );
        
        var role = new Role(_dateTime, roleId, createdBy, createdRole, command.Name);

        await _roleCommandRepository.AddAsync(role, cancellationToken);

        return role.Id;
    }

    public Task AfterTransactionHandleAsync(CreateCommand message, CancellationToken cancellationToken)
        => Task.CompletedTask;
}