using Domic.UseCase.PermissionUseCase.DTOs.ViewModels;
using Domic.Core.UseCase.DTOs.ViewModels;

namespace Domic.UseCase.RoleUseCase.DTOs.ViewModels;

public class RolesViewModel : ViewModel
{
    public string Id   { get; set; }
    public string Name { get; set; }
    
    /*---------------------------------------------------------------*/
    
    public IEnumerable<PermissionsViewModel> Permissions { get; set; }
}