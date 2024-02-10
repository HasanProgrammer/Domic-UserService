using Domic.UseCase.PermissionUseCase.DTOs.ViewModels;
using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.PermissionUseCase.Queries.ReadOne;

public class ReadOneQuery : IQuery<PermissionsViewModel>
{
    public string PermissionId { get; set; }
}