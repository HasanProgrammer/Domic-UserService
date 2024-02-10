#pragma warning disable CS0649

using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Entities;

namespace Domic.UseCase.UserUseCase.Commands.Active;

public class ActiveCommandHandler : ICommandHandler<ActiveCommand, string>
{
    private readonly object _validationResult;

    private readonly IDateTime               _dateTime;
    private readonly ISerializer             _serializer;
    private readonly IJsonWebToken           _jsonWebToken;
    private readonly IUserCommandRepository  _userCommandRepository;

    public ActiveCommandHandler(IUserCommandRepository userCommandRepository, IDateTime dateTime, 
        ISerializer serializer, IJsonWebToken jsonWebToken
    )
    {
        _dateTime               = dateTime;
        _serializer             = serializer;
        _jsonWebToken           = jsonWebToken;
        _userCommandRepository  = userCommandRepository;
    }

    [WithValidation]
    [WithTransaction]
    public Task<string> HandleAsync(ActiveCommand command, CancellationToken cancellationToken)
    {
        var targetUser  = _validationResult as User;
        var updatedBy   = _jsonWebToken.GetIdentityUserId(command.Token);
        var updatedRole = _serializer.Serialize( _jsonWebToken.GetRoles(command.Token) );
        
        targetUser.Active(_dateTime, updatedBy, updatedRole, _jsonWebToken.GetUsername(command.Token));

        _userCommandRepository.Change(targetUser);

        return Task.FromResult(targetUser.Id);
    }
}