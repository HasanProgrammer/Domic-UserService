using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.Domain.Role.Contracts.Interfaces;

namespace Domic.UseCase.PermissionUseCase.Commands.Create;

public class CreateCommandValidator : IValidator<CreateCommand>
{
    private readonly IRoleCommandRepository       _roleCommandRepository;
    private readonly IPermissionCommandRepository _permissionCommandRepository;

    public CreateCommandValidator(IRoleCommandRepository roleCommandRepository,
        IPermissionCommandRepository permissionCommandRepository
    )
    {
        _roleCommandRepository       = roleCommandRepository;
        _permissionCommandRepository = permissionCommandRepository;
    }

    public async Task<object> ValidateAsync(CreateCommand input, CancellationToken cancellationToken)
    {
        if (await _roleCommandRepository.FindByIdAsync(input.RoleId, cancellationToken) is null)
            throw new UseCaseException(
                string.Format("نقشی با شناسه {0} وجود خارجی ندارد !", input.RoleId ?? "_خالی_")
            );
        
        if (await _permissionCommandRepository.FindByNameAsync(input.Name, cancellationToken) is not null) 
            throw new UseCaseException("فیلد نام سطح دسترسی قبلا انتخاب شده است !");

        return default;
    }
}