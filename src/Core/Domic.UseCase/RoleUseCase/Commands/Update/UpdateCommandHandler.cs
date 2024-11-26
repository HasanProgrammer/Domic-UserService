#pragma warning disable CS0649

using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Domain.Role.Contracts.Interfaces;
using Domic.Domain.Role.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Domic.UseCase.RoleUseCase.Commands.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand, string>
{
    private readonly object _validationResult;

    private readonly IDateTime              _dateTime;
    private readonly ISerializer            _serializer;
    private readonly IIdentityUser          _identityUser;
    private readonly IRoleCommandRepository _roleCommandRepository;

    public UpdateCommandHandler(IRoleCommandRepository roleCommandRepository, IDateTime dateTime, 
        ISerializer serializer, [FromKeyedServices("http1")] IIdentityUser identityUser
    )
    {
        _dateTime              = dateTime;
        _serializer            = serializer;
        _identityUser          = identityUser;
        _roleCommandRepository = roleCommandRepository;
    }

    public Task BeforeHandleAsync(UpdateCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public Task<string> HandleAsync(UpdateCommand command, CancellationToken cancellationToken)
    {
        var targetRole = _validationResult as Role;

        targetRole.Change(_dateTime, _identityUser, _serializer, command.Name);

        _roleCommandRepository.Change(targetRole);

        return Task.FromResult(targetRole.Id);
    }

    public Task AfterHandleAsync(UpdateCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}