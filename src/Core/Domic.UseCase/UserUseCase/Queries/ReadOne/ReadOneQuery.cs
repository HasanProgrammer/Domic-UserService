using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.UseCase.UserUseCase.DTOs;

namespace Domic.UseCase.UserUseCase.Queries.ReadOne;

public class ReadOneQuery : IQuery<UserDto>
{
    public string Id { get; set; }
}