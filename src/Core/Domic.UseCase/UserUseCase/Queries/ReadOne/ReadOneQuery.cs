using Domic.UseCase.UserUseCase.DTOs.ViewModels;
using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Queries.ReadOne;

public class ReadOneQuery : IQuery<UsersDto>
{
    public string UserId { get; set; }
}