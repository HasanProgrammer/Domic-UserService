using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.UseCase.Exceptions;
using Karami.Domain.Permission.Contracts.Interfaces;
using Karami.Domain.Role.Contracts.Interfaces;
using Karami.Domain.User.Contracts.Interfaces;

namespace Karami.UseCase.UserUseCase.Commands.Update;

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
        var targetUser = await _userCommandRepository.FindByIdAsync(input.Id, cancellationToken)
                         ??
                         throw new UseCaseException(
                             string.Format("کاربری با شناسه {0} وجود خارجی ندارد !", input.Id ?? "_خالی_")
                         );
        
        if 
        (
            await _userCommandRepository.FindByUsernameAsync(input.Username, cancellationToken) is not null && 
            !input.Username.Equals(targetUser.Username.Value)
        ) throw new UseCaseException("فیلد نام کاربری مورد نظر قبلا انتخاب شده است !");
        
        if 
        (
            await _userCommandRepository.FindByPhoneNumberAsync(input.PhoneNumber, cancellationToken) is not null && 
            !input.PhoneNumber.Equals(targetUser.PhoneNumber.Value)
        ) throw new UseCaseException("فیلد شماره تماس مورد نظر قبلا انتخاب شده است !");
        
        if 
        (
            await _userCommandRepository.FindByEmailAsync(input.EMail, cancellationToken) is not null &&
            !input.EMail.Equals(targetUser.Email.Value)
        ) throw new UseCaseException("فیلد پست الکترونیکی مورد نظر قبلا انتخاب شده است !");
        
        if (!input.Roles.Any())
            throw new UseCaseException("فیلد نقوش الزامی می باشد !");
        
        foreach (string roleId in input.Roles.Distinct())
            if(await _roleCommandRepository.FindByIdAsync(roleId, cancellationToken) == null)
                throw new UseCaseException( 
                    string.Format("نقشی با شناسه {0} وجود خارجی ندارد !", roleId ?? "_خالی_")
                );
        
        if(!input.Permissions.Any())
            throw new UseCaseException("فیلد سطوح دسترسی الزامی می باشد !");

        foreach (string permissionId in input.Permissions)
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

        return targetUser;
    }
}