using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.UseCase.PermissionUseCase.DTOs.ViewModels;

namespace Karami.UseCase.PermissionUseCase.Queries.ReadOne;

public class ReadOneQuery : IQuery<PermissionsViewModel>
{
    public string PermissionId { get; set; }
}