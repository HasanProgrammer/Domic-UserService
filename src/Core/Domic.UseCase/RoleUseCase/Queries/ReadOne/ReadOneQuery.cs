using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.UseCase.RoleUseCase.DTOs;

namespace Domic.UseCase.RoleUseCase.Queries.ReadOne;

public class ReadOneQuery : IQuery<RoleDto>
{
    public string RoleId { get; set; }
}