using Karami.Core.Domain.Exceptions;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.UseCase.Exceptions;
using Karami.Domain.Role.Contracts.Interfaces;

namespace Karami.UseCase.RoleUseCase.Commands.Delete;

public class DeleteCommandValidator : IValidator<DeleteCommand>
{
    private readonly IRoleCommandRepository _roleCommandRepository;

    public DeleteCommandValidator(IRoleCommandRepository roleCommandRepository) 
        => _roleCommandRepository = roleCommandRepository;

    public async Task<object> ValidateAsync(DeleteCommand input, CancellationToken cancellationToken)
    {
        var targetRole = await _roleCommandRepository.FindByIdAsync(input.RoleId, cancellationToken);
        
        if(targetRole is null)
            throw new UseCaseException(
                string.Format("نقشی با شناسه {0} وجود خارجی ندارد !", input.RoleId ?? "_خالی_")
            );

        return targetRole;
    }
}