using Domic.Core.Domain.Exceptions;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.Role.Contracts.Interfaces;

namespace Domic.UseCase.RoleUseCase.Queries.ReadOne;

public class ReadOneQueryValidator : IValidator<ReadOneQuery>
{
    private readonly IRoleQueryRepository _roleQueryRepository;

    public ReadOneQueryValidator(IRoleQueryRepository roleQueryRepository) => 
        _roleQueryRepository = roleQueryRepository;

    public async Task<object> ValidateAsync(ReadOneQuery input, CancellationToken cancellationToken)
    {
        var targetRoleQuery = await _roleQueryRepository.FindByIdEagerLoadingAsync(input.RoleId, cancellationToken);
        
        if(targetRoleQuery is null)
            throw new UseCaseException(
                string.Format("نقشی با شناسه {0} وجود خارجی ندارد !", input.RoleId) 
            );

        return targetRoleQuery;
    }
}