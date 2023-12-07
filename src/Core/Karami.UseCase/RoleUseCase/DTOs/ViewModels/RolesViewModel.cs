using Karami.Core.UseCase.DTOs.ViewModels;
using Karami.UseCase.PermissionUseCase.DTOs.ViewModels;

namespace Karami.UseCase.RoleUseCase.DTOs.ViewModels;

public class RolesViewModel : ViewModel
{
    public string Id   { get; set; }
    public string Name { get; set; }
    
    /*---------------------------------------------------------------*/
    
    public IEnumerable<PermissionsViewModel> Permissions { get; set; }
}