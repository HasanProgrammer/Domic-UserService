using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.InActive;

public class InActiveCommand : ICommand<string>
{
    public required string Id { get; set; }
}