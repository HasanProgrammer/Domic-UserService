using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.User.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.CheckExist;

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