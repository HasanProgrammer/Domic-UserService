using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.PermissionUseCase.Commands.Delete;

public class DeleteCommand : ICommand<string>
{
    public string Id { get; set; }
}