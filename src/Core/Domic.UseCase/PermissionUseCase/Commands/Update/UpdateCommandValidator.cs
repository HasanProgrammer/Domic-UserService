using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.Domain.Role.Contracts.Interfaces;

namespace Domic.UseCase.PermissionUseCase.Commands.Update;

public class UpdateCommandValidator : IValidator<UpdateCommand>
{
    private readonly IRoleCommandRepository       _roleCommandRepository;
    private readonly IPermissionCommandRepository _permissionCommandRepository;

    public UpdateCommandValidator(IPermissionCommandRepository permissionCommandRepository,
        IRoleCommandRepository roleCommandRepository
    )
    {
        _roleCommandRepository       = roleCommandRepository;
        _permissionCommandRepository = permissionCommandRepository;
    }

    public async Task<object> ValidateAsync(UpdateCommand input, CancellationToken cancellationToken)
    {
        var targetPermission
            = await _permissionCommandRepository.FindByIdAsync(input.Id, cancellationToken);
        
        if(targetPermission is null)
            throw new UseCaseException(
                string.Format("سطح دسترسی با شناسه {0} وجود خارجی ندارد !", input.Id ?? "_خالی_")
            );

        var permissionByName =
            await _permissionCommandRepository.FindByNameAsync(input.Name, cancellationToken);
        
        if (permissionByName is not null && !input.Name.Equals(targetPermission.Name.Value)) 
            throw new UseCaseException("فیلد نام سطح دسترسی قبلا انتخاب شده است !");
        
        if (await _roleCommandRepository.FindByIdAsync(input.RoleId, cancellationToken) is null)
            throw new UseCaseException(
                string.Format("نقشی با شناسه {0} وجود خارجی ندارد !", input.RoleId ?? "_خالی_")
            );

        return targetPermission;
    }
}