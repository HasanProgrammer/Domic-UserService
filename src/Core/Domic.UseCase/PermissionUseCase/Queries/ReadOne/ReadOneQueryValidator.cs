using Domic.Core.Domain.Exceptions;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.Permission.Contracts.Interfaces;

namespace Domic.UseCase.PermissionUseCase.Queries.ReadOne;

public class ReadOneQueryValidator : IValidator<ReadOneQuery>
{
    private readonly IPermissionQueryRepository _permissionQueryRepository;

    public ReadOneQueryValidator(IPermissionQueryRepository permissionQueryRepository) 
        => _permissionQueryRepository = permissionQueryRepository;

    public async Task<object> ValidateAsync(ReadOneQuery input, CancellationToken cancellationToken)
    {
        var permissionQuery = 
            await _permissionQueryRepository.FindByIdEagerLoadingAsync(input.PermissionId, cancellationToken);
        
        if(permissionQuery is null)
            throw new UseCaseException(
                string.Format("سطح دسترسی با شناسه {0} وجود خارجی ندارد !", input.PermissionId) 
            );
        
        return permissionQuery;
    }
}