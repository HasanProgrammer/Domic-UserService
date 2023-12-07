using Karami.Core.Domain.Exceptions;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.UseCase.Exceptions;
using Karami.Domain.Permission.Contracts.Interfaces;

namespace Karami.UseCase.PermissionUseCase.Commands.Delete;

public class DeleteCommandValidator : IValidator<DeleteCommand>
{
    private readonly IPermissionCommandRepository _permissionCommandRepository;

    public DeleteCommandValidator(IPermissionCommandRepository permissionCommandRepository) 
        => _permissionCommandRepository = permissionCommandRepository;

    public async Task<object> ValidateAsync(DeleteCommand input, CancellationToken cancellationToken)
    {
        var permission = 
            await _permissionCommandRepository.FindByIdAsync(input.PermissionId, cancellationToken);
        
        if(permission is null)
            throw new UseCaseException(
                string.Format("سطح دسترسی با شناسه {0} وجود خارجی ندارد !", input.PermissionId ?? "_خالی_")
            );

        return permission;
    }
}