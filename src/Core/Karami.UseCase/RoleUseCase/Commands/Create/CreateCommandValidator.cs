using Karami.Core.Domain.Exceptions;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.UseCase.Exceptions;
using Karami.Domain.Role.Contracts.Interfaces;

namespace Karami.UseCase.RoleUseCase.Commands.Create;

public class CreateCommandValidator : IValidator<CreateCommand>
{
    private readonly IRoleCommandRepository _roleCommandRepository;

    public CreateCommandValidator(IRoleCommandRepository roleCommandRepository) 
        => _roleCommandRepository = roleCommandRepository;

    public async Task<object> ValidateAsync(CreateCommand input, CancellationToken cancellationToken)
    {
        if (await _roleCommandRepository.FindByNameAsync(input.Name, cancellationToken) is not null)
            throw new UseCaseException("فیلد نام نقش قبلا انتخاب شده است !");

        return default;
    }
}