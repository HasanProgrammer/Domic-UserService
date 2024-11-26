#pragma warning disable CS0649

using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Domic.UseCase.UserUseCase.Commands.InActive;

public class InActiveCommandHandler : ICommandHandler<InActiveCommand, string>
{
    private readonly object _validationResult;

    private readonly IDateTime              _dateTime;
    private readonly ISerializer            _serializer;
    private readonly IIdentityUser          _identityUser;
    private readonly IUserCommandRepository _userCommandRepository;

    public InActiveCommandHandler(IUserCommandRepository userCommandRepository, 
        IDateTime dateTime, ISerializer serializer, [FromKeyedServices("http1")] IIdentityUser identityUser
    )
    {
        _dateTime              = dateTime;
        _serializer            = serializer;
        _identityUser          = identityUser;
        _userCommandRepository = userCommandRepository;
    }

    public Task BeforeHandleAsync(InActiveCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public Task<string> HandleAsync(InActiveCommand command, CancellationToken cancellationToken)
    {
        var targetUser  = _validationResult as User;
        
        targetUser.InActive(_dateTime, _identityUser, _serializer);

        _userCommandRepository.Change(targetUser);

        return Task.FromResult(targetUser.Id);
    }

    public Task AfterHandleAsync(InActiveCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}