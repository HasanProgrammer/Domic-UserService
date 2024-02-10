using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.Domain.Role.Contracts.Interfaces;
using Domic.Domain.User.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.Create;

public class CreateCommandValidator : IValidator<CreateCommand>
{
    private readonly IRoleCommandRepository       _roleCommandRepository;
    private readonly IPermissionCommandRepository _permissionCommandRepository;
    private readonly IUserCommandRepository       _userCommandRepository;

    public CreateCommandValidator(IRoleCommandRepository roleCommandRepository,
        IPermissionCommandRepository permissionCommandRepository, IUserCommandRepository userCommandRepository
    )
    {
        _roleCommandRepository       = roleCommandRepository;
        _permissionCommandRepository = permissionCommandRepository;
        _userCommandRepository       = userCommandRepository;
    }

    public async Task<object> ValidateAsync(CreateCommand input, CancellationToken cancellationToken)
    {
        if (!input.Roles.Any())
            throw new UseCaseException("فیلد نقوش الزامی می باشد !");
        
        foreach (string roleId in input.Roles.Distinct())
            if(await _roleCommandRepository.FindByIdAsync(roleId, cancellationToken) == null)
                throw new UseCaseException(
                    string.Format("نقشی با شناسه {0} وجود خارجی ندارد !", roleId ?? "_خالی_")
                );
        
        if(!input.Permissions.Any())
            throw new UseCaseException("فیلد سطوح دسترسی الزامی می باشد !");

        foreach (string permissionId in input.Permissions.Distinct())
        {
            var targetPermission = await _permissionCommandRepository.FindByIdAsync(permissionId, cancellationToken) 
                                   ??
                                   throw new UseCaseException(
                                       string.Format(
                                           "سطح دسترسی با شناسه {0} وجود خارجی ندارد !", permissionId ?? "_خالی_"
                                       ) 
                                   );

            if (input.Roles.All(role => role != targetPermission.RoleId))
                throw new UseCaseException(
                    string.Format("سطح دسترسی با شناسه {0} متعلق به نقوش انتخاب شده نمی باشد !", permissionId)  
                );
        }
        
        if(await _userCommandRepository.FindByUsernameAsync(input.Username, cancellationToken) is not null)
            throw new UseCaseException("فیلد نام کاربری مورد نظر قبلا انتخاب شده است !");
        
        if(await _userCommandRepository.FindByPhoneNumberAsync(input.PhoneNumber, cancellationToken) is not null)
            throw new UseCaseException("فیلد شماره تماس مورد نظر قبلا انتخاب شده است !");
        
        if(await _userCommandRepository.FindByEmailAsync(input.EMail, cancellationToken) is not null)
            throw new UseCaseException("فیلد پست الکترونیکی مورد نظر قبلا انتخاب شده است !");

        return default;
    }
}