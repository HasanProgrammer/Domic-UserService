using Karami.Core.UseCase.DTOs.ViewModels;

namespace Karami.UseCase.PermissionUseCase.DTOs.ViewModels;

public class PermissionsViewModel : ViewModel
{
    public string Id       { get; set; }
    public string Name     { get; set; }
    public string RoleId   { get; set; }
    public string RoleName { get; set; }
}