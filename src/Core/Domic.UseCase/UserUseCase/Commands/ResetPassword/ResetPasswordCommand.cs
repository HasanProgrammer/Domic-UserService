using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.ResetPassword;

public class ResetPasswordCommand : ICommand<bool>
{
    public string NewPassword { get; set; }
    public string EmailAddress { get; set; }
    public string EmailCode { get; set; }
}