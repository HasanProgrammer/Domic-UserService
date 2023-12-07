using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.UseCase.UserUseCase.DTOs.ViewModels;

namespace Karami.UseCase.UserUseCase.Queries.ReadOne;

public class ReadOneQuery : IQuery<UsersViewModel>
{
    public string UserId { get; set; }
}