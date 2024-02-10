using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.PermissionUseCase.Commands.Update;

public class UpdateCommand : ICommand<string>
{
    public required string Token  { get; set; }
    public required string Id     { get; set; }
    public required string RoleId { get; set; }
    public required string Name   { get; set; }
}