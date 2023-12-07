using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.UseCase.RoleUseCase.DTOs.ViewModels;

namespace Karami.UseCase.RoleUseCase.Queries.ReadOne;

public class ReadOneQuery : IQuery<RolesViewModel>
{
    public string RoleId { get; set; }
}