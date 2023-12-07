using Karami.Core.UseCase.Contracts.Interfaces;

namespace Karami.UseCase.UserUseCase.Commands.InActive;

public class InActiveCommand : ICommand<string>
{
    public required string Token { get; set; }
    public required string Id    { get; set; }
}