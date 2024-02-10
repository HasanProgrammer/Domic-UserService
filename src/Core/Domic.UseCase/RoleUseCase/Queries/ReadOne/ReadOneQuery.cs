using Domic.UseCase.RoleUseCase.DTOs.ViewModels;
using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.RoleUseCase.Queries.ReadOne;

public class ReadOneQuery : IQuery<RolesViewModel>
{
    public string RoleId { get; set; }
}