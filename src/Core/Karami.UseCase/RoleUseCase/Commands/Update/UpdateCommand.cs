using Karami.Core.UseCase.Contracts.Interfaces;

namespace Karami.UseCase.RoleUseCase.Commands.Update;

public class UpdateCommand : ICommand<string>
{
    public required string Token { get; set; }
    public required string Id    { get; set; }
    public required string Name  { get; set; }
}