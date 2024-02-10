using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.CheckExist;

public class CheckExistCommand : IQuery<bool>
{
    public string UserId { get; set; }
}