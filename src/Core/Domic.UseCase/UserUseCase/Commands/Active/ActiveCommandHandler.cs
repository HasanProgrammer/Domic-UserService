#pragma warning disable CS0649

using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Domic.UseCase.UserUseCase.Commands.Active;

public class ActiveCommandHandler : ICommandHandler<ActiveCommand, string>
{
    private readonly object _validationResult;

    private readonly IDateTime              _dateTime;
    private readonly ISerializer            _serializer;
    private readonly IUserCommandRepository _userCommandRepository;
    private readonly IIdentityUser          _identityUser;

    public ActiveCommandHandler(IUserCommandRepository userCommandRepository, IDateTime dateTime, 
        ISerializer serializer, [FromKeyedServices("http1")] IIdentityUser identityUser
    )
    {
        _dateTime               = dateTime;
        _serializer             = serializer;
        _userCommandRepository  = userCommandRepository;
        _identityUser           = identityUser;
    }

    public Task BeforeHandleAsync(ActiveCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public Task<string> HandleAsync(ActiveCommand command, CancellationToken cancellationToken)
    {
        var targetUser = _validationResult as User;
        
        targetUser.Active(_dateTime, _identityUser, _serializer);

        _userCommandRepository.Change(targetUser);

        return Task.FromResult(targetUser.Id);
    }

    public Task AfterHandleAsync(ActiveCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}