using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.User.Contracts.Interfaces;

namespace Karami.UseCase.UserUseCase.Commands.CheckExist;

public class CheckExistCommandHandler : IQueryHandler<CheckExistCommand, bool>
{
    private readonly IUserCommandRepository _userCommandRepository;

    public CheckExistCommandHandler(IUserCommandRepository userCommandRepository) 
        => _userCommandRepository = userCommandRepository;

    public async Task<bool> HandleAsync(CheckExistCommand command, CancellationToken cancellationToken)
    {
        var result = await _userCommandRepository.FindByIdAsync(command.UserId, cancellationToken);

        return result is not null;
    }
}