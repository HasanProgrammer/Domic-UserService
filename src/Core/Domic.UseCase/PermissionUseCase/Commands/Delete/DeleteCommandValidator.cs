using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.Permission.Contracts.Interfaces;

namespace Domic.UseCase.PermissionUseCase.Commands.Delete;

public class DeleteCommandValidator : IValidator<DeleteCommand>
{
    private readonly IPermissionCommandRepository _permissionCommandRepository;

    public DeleteCommandValidator(IPermissionCommandRepository permissionCommandRepository) 
        => _permissionCommandRepository = permissionCommandRepository;

    public async Task<object> ValidateAsync(DeleteCommand input, CancellationToken cancellationToken)
    {
        var permission = 
            await _permissionCommandRepository.FindByIdAsync(input.Id, cancellationToken);
        
        if(permission is null)
            throw new UseCaseException(
                string.Format("سطح دسترسی با شناسه {0} وجود خارجی ندارد !", input.Id ?? "_خالی_")
            );

        return permission;
    }
}