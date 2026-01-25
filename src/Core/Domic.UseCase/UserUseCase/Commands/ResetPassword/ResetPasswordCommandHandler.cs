#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value

using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Domic.UseCase.UserUseCase.Commands.ResetPassword;

public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, bool>
{
    private readonly IUserCommandRepository  _userCommandRepository;
    private readonly IDateTime               _dateTime;
    private readonly ISerializer             _serializer;
    private readonly IIdentityUser           _identityUser;
    
    private readonly object _validationResult;

    public ResetPasswordCommandHandler(
        IUserCommandRepository userCommandRepository, IDateTime dateTime, ISerializer serializer, 
        [FromKeyedServices("Http2")] IIdentityUser identityUser
    )
    {
        _userCommandRepository   = userCommandRepository;
        _dateTime                = dateTime;
        _serializer              = serializer;
        _identityUser            = identityUser;
    }

    public Task BeforeHandleAsync(ResetPasswordCommand command, CancellationToken cancellationToken) 
        => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public async Task<bool> HandleAsync(ResetPasswordCommand command, CancellationToken cancellationToken)
    { 
        var targetUser = _validationResult as User;
        
        targetUser.EmailOtpVerified(_dateTime, _identityUser, _serializer);
        
        targetUser.ResetPassword(_dateTime, _identityUser, _serializer, command.NewPassword);
        
        await _userCommandRepository.ChangeAsync(targetUser, cancellationToken);

        return true;
    }

    public Task AfterHandleAsync(ResetPasswordCommand command, CancellationToken cancellationToken) 
        => Task.CompletedTask;
}