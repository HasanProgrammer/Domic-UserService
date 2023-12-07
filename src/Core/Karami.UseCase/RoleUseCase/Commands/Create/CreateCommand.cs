using Karami.Core.UseCase.Contracts.Interfaces;

namespace Karami.UseCase.RoleUseCase.Commands.Create;

public class CreateCommand : ICommand<string>
{
    public required string Token { get; set; }
    public required string Name  { get; set; }
}