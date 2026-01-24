using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.EmailOtpGeneration;

public class EmailOtpGenerationCommand : ICommand<bool>
{
    public string EmailAddress { get; set; }
}