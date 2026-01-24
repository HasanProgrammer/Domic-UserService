#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value

using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.User.Contracts.Interfaces;
using Domic.Domain.User.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Domic.UseCase.UserUseCase.Commands.EmailOtpGeneration;

public class EmailOtpGenerationCommandHandler(
    IUserCommandRepository userCommandRepository, IDateTime dateTime, ISerializer serializer,
    [FromKeyedServices("Http2")] IIdentityUser identityUser
) : ICommandHandler<EmailOtpGenerationCommand, bool>
{
    private readonly object _validationResult;
    
    public Task BeforeHandleAsync(EmailOtpGenerationCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    public async Task<bool> HandleAsync(EmailOtpGenerationCommand command, CancellationToken cancellationToken)
    {
        var targetUser = _validationResult as User;
        
        targetUser.SetEmailOtpCode(dateTime, identityUser, serializer);

        await userCommandRepository.ChangeAsync(targetUser, cancellationToken);

        return true;
    }

    public Task AfterHandleAsync(EmailOtpGenerationCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}