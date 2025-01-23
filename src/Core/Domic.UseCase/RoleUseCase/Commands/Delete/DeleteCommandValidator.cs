using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.Role.Contracts.Interfaces;

namespace Domic.UseCase.RoleUseCase.Commands.Delete;

public class DeleteCommandValidator : IValidator<DeleteCommand>
{
    private readonly IRoleCommandRepository _roleCommandRepository;

    public DeleteCommandValidator(IRoleCommandRepository roleCommandRepository) 
        => _roleCommandRepository = roleCommandRepository;

    public async Task<object> ValidateAsync(DeleteCommand input, CancellationToken cancellationToken)
    {
        var targetRole = await _roleCommandRepository.FindByIdAsync(input.Id, cancellationToken);
        
        if(targetRole is null)
            throw new UseCaseException(
                string.Format("نقشی با شناسه {0} وجود خارجی ندارد !", input.Id ?? "_خالی_")
            );

        return targetRole;
    }
}