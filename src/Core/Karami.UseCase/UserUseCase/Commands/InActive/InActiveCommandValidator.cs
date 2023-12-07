using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.UseCase.Exceptions;
using Karami.Domain.User.Contracts.Interfaces;

namespace Karami.UseCase.UserUseCase.Commands.InActive;

public class InActiveCommandValidator : IValidator<InActiveCommand>
{
    private readonly IUserCommandRepository _userCommandRepository;

    public InActiveCommandValidator(IUserCommandRepository userCommandRepository)
        => _userCommandRepository = userCommandRepository;

    public async Task<object> ValidateAsync(InActiveCommand input, CancellationToken cancellationToken)
    {
        var targetUser = await _userCommandRepository.FindByIdAsync(input.Id, cancellationToken)
                         ??
                         throw new UseCaseException(
                             string.Format("کاربری با شناسه {0} وجود خارجی ندارد !", input.Id ?? "_خالی_")
                         );

        return targetUser;
    }
}