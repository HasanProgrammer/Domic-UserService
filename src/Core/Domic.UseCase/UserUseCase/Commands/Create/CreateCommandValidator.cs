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
        List<string> errors = new();
        
        if(await _userCommandRepository.IsExistByUsernameAsync(input.Username, cancellationToken))
            errors.Add("فیلد نام کاربری مورد نظر قبلا انتخاب شده است !");
            
        if(await _userCommandRepository.IsExistByPhoneNumberAsync(input.PhoneNumber, cancellationToken))
            errors.Add("فیلد شماره تماس مورد نظر قبلا انتخاب شده است !");
        
        if(await _userCommandRepository.IsExistByEmailAsync(input.EMail, cancellationToken))
            errors.Add("فیلد پست الکترونیکی مورد نظر قبلا انتخاب شده است !");
        
        if (!input.Roles.Any())
            errors.Add("فیلد نقوش الزامی می باشد !");
        
        if(!input.Permissions.Any())
            errors.Add("فیلد سطوح دسترسی الزامی می باشد !");
        
        foreach (string roleId in input.Roles?.Distinct())
            if(!await _roleCommandRepository.IsExistByIdAsync(roleId, cancellationToken))
                errors.Add(string.Format("نقشی با شناسه {0} وجود خارجی ندارد !", roleId ?? "_خالی_"));

        foreach (string permissionId in input.Permissions?.Distinct())
        {
            var targetPermission = await _permissionCommandRepository.FindByIdAsync(permissionId, cancellationToken);
            
            if(targetPermission is null)
                errors.Add(string.Format("سطح دسترسی با شناسه {0} وجود خارجی ندارد !", permissionId ?? "_خالی_"));

            if (input.Roles.All(role => role != targetPermission?.RoleId)) 
                errors.Add(string.Format("سطح دسترسی با شناسه {0} متعلق به نقوش انتخاب شده نمی باشد !", permissionId));
        }

        if (errors.Any())
            throw new UseCaseException(string.Join("|", errors));
        
        return default;
    }
}