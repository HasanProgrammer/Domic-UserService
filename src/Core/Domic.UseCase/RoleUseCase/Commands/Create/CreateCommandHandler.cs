using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Role.Entities;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Domain.Role.Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Domic.UseCase.RoleUseCase.Commands.Create;

public class CreateCommandHandler : ICommandHandler<CreateCommand, string>
{
    private readonly IDateTime                _dateTime;
    private readonly ISerializer              _serializer;
    private readonly IRoleCommandRepository   _roleCommandRepository;
    private readonly IGlobalUniqueIdGenerator _globalUniqueIdGenerator;
    private readonly IIdentityUser            _identityUser;

    public CreateCommandHandler(IRoleCommandRepository roleCommandRepository, IDateTime dateTime, 
        ISerializer serializer, IGlobalUniqueIdGenerator globalUniqueIdGenerator, 
        [FromKeyedServices("Http2")] IIdentityUser identityUser
    )
    {
        _dateTime                = dateTime;
        _serializer              = serializer;
        _roleCommandRepository   = roleCommandRepository;
        _globalUniqueIdGenerator = globalUniqueIdGenerator;
        _identityUser            = identityUser;
    }

    public Task BeforeHandleAsync(CreateCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        var role = new Role(_globalUniqueIdGenerator, _dateTime, _identityUser, _serializer, command.Name);

        await _roleCommandRepository.AddAsync(role, cancellationToken);

        return role.Id;
    }

    public Task AfterHandleAsync(CreateCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}