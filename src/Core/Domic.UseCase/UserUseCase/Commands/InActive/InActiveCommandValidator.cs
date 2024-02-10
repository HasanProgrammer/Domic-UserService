using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.User.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.InActive;

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