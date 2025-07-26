using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.Domain.Role.Contracts.Interfaces;
using Domic.Domain.User.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.Update;

public class UpdateCommandValidator : IValidator<UpdateCommand>
{
    private readonly IUserCommandRepository       _userCommandRepository;
    private readonly IRoleCommandRepository       _roleCommandRepository;
    private readonly IPermissionCommandRepository _permissionCommandRepository;

    public UpdateCommandValidator(IUserCommandRepository userCommandRepository,
        IRoleCommandRepository roleCommandRepository,
        IPermissionCommandRepository permissionCommandRepository
    )
    {
        _userCommandRepository       = userCommandRepository;
        _roleCommandRepository       = roleCommandRepository;
        _permissionCommandRepository = permissionCommandRepository;
    }

    public async Task<object> ValidateAsync(UpdateCommand input, CancellationToken cancellationToken)
    {
        List<string> errors = new();
        
        var targetUser = await _userCommandRepository.FindByIdEagerLoadingAsync(input.Id, cancellationToken)
                         ??
                         throw new UseCaseException(
                             string.Format("کاربری با شناسه {0} وجود خارجی ندارد !", input.Id ?? "_خالی_")
                         );

        var userWithUsernameConditions = (
            await _userCommandRepository.IsExistByUsernameAsync(input.Username, cancellationToken) && 
            !input.Username.Equals(targetUser.Username.Value)
        );
        
        if (userWithUsernameConditions)
            errors.Add("فیلد نام کاربری مورد نظر قبلا انتخاب شده است !");

        var userWithPhoneNumberConditions = (
            await _userCommandRepository.IsExistByPhoneNumberAsync(input.PhoneNumber, cancellationToken) && 
            !input.PhoneNumber.Equals(targetUser.PhoneNumber.Value)
        );
        
        if (userWithPhoneNumberConditions)
            errors.Add("فیلد شماره تماس مورد نظر قبلا انتخاب شده است !");
        
        var userWithEmailConditions = (
            await _userCommandRepository.IsExistByEmailAsync(input.EMail, cancellationToken) && 
            !input.EMail.Equals(targetUser.Email.Value)
        );
        
        if(userWithEmailConditions)
            errors.Add("فیلد پست الکترونیکی مورد نظر قبلا انتخاب شده است !");
        
        if (!input.Roles.Any())
            errors.Add("فیلد نقوش الزامی می باشد !");
        
        foreach (string roleId in input.Roles?.Distinct())
            if(!await _roleCommandRepository.IsExistByIdAsync(roleId, cancellationToken))
                errors.Add(string.Format("نقشی با شناسه {0} وجود خارجی ندارد !", roleId ?? "_خالی_"));
        
        if(!input.Permissions.Any())
            errors.Add("فیلد سطوح دسترسی الزامی می باشد !");

        foreach (string permissionId in input.Permissions?.Distinct())
        {
            var targetPermission = await _permissionCommandRepository.FindByIdAsync(permissionId, cancellationToken);
            
            if(targetPermission is null)
                errors.Add(string.Format("سطح دسترسی با شناسه {0} وجود خارجی ندارد !", permissionId ?? "_خالی_"));

            if (!input.Roles.Any(role => role == targetPermission?.RoleId)) 
                errors.Add(string.Format("سطح دسترسی با شناسه {0} متعلق به نقوش انتخاب شده نمی باشد !", permissionId));
        }
        
        if (errors.Any())
            throw new UseCaseException(string.Join("|", errors));

        return targetUser;
    }
}