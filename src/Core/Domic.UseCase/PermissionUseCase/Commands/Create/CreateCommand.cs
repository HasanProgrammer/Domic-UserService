using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.PermissionUseCase.Commands.Create;

public class CreateCommand : ICommand<string>
{
    public required string Token  { get; set; }
    public required string RoleId { get; set; }
    public required string Name   { get; set; }
}