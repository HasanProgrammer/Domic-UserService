using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.RoleUseCase.Commands.Create;

public class CreateCommand : ICommand<string>
{
    public required string Name { get; set; }
}