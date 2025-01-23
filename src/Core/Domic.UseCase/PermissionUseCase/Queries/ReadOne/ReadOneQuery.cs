using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.UseCase.PermissionUseCase.DTOs;

namespace Domic.UseCase.PermissionUseCase.Queries.ReadOne;

public class ReadOneQuery : IQuery<PermissionDto>
{
    public string PermissionId { get; set; }
}