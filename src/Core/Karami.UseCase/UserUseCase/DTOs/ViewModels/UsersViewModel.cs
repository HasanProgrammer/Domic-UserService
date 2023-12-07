using Karami.Core.UseCase.DTOs.ViewModels;
using Karami.UseCase.PermissionUseCase.DTOs.ViewModels;
using Karami.UseCase.RoleUseCase.DTOs.ViewModels;

namespace Karami.UseCase.UserUseCase.DTOs.ViewModels;

public class UsersViewModel : ViewModel
{
    public string Id                  { get; set; }
    public string FirstName           { get; set; }
    public string LastName            { get; set; }
    public string Username            { get; set; }
    public string PhoneNumber         { get; set; }
    public string Email               { get; set; }
    public string Description         { get; set; }
    
    /*---------------------------------------------------------------*/
    
    public IEnumerable<RolesViewModel> Roles             { get; set; }
    public IEnumerable<PermissionsViewModel> Permissions { get; set; }
}