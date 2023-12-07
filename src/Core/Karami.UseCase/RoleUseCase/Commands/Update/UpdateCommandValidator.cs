﻿using Karami.Core.Domain.Exceptions;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.UseCase.Exceptions;
using Karami.Domain.Role.Contracts.Interfaces;

namespace Karami.UseCase.RoleUseCase.Commands.Update;

public class UpdateCommandValidator : IValidator<UpdateCommand>
{
    private readonly IRoleCommandRepository _roleCommandRepository;

    public UpdateCommandValidator(IRoleCommandRepository roleCommandRepository) 
        => _roleCommandRepository = roleCommandRepository;

    public async Task<object> ValidateAsync(UpdateCommand input, CancellationToken cancellationToken)
    {
        var targetRole = await _roleCommandRepository.FindByIdAsync(input.Id, cancellationToken);
        
        if(targetRole is null)
            throw new UseCaseException(
                string.Format("نقشی با شناسه {0} وجود خارجی ندارد !", input.Id ?? "_خالی_") 
            );
        
        if 
        (
            await _roleCommandRepository.FindByNameAsync(input.Name, cancellationToken) is not null 
            && 
            !input.Name.Equals(targetRole.Name.Value)
        ) throw new UseCaseException("فیلد نام نقش قبلا انتخاب شده است !");

        return targetRole;
    }
}