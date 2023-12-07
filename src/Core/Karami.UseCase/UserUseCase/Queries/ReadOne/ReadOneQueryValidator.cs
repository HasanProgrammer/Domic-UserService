using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.UseCase.Exceptions;
using Karami.Domain.User.Contracts.Interfaces;

namespace Karami.UseCase.UserUseCase.Queries.ReadOne;

public class ReadOneQueryValidator : IValidator<ReadOneQuery>
{
    private readonly IUserQueryRepository _userQueryRepository;

    public ReadOneQueryValidator(IUserQueryRepository userQueryRepository) 
        => _userQueryRepository = userQueryRepository;

    public async Task<object> ValidateAsync(ReadOneQuery input, CancellationToken cancellationToken)
    {
        var targetUserQuery = await _userQueryRepository.FindByIdEagerLoadingAsync(input.UserId, cancellationToken);
        
        if(targetUserQuery is null)
            throw new UseCaseException( string.Format("کاربری با شناسه {0} وجود خارجی ندارد !", input.UserId) );

        return targetUserQuery;
    }
}