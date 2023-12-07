using Karami.Core.UseCase.Contracts.Interfaces;

namespace Karami.UseCase.PermissionUseCase.Commands.Delete;

public class DeleteCommand : ICommand<string>
{
    public string Token        { get; set; }
    public string PermissionId { get; set; }
}