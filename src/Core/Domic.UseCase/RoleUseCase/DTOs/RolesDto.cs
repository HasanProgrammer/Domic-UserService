using Domic.UseCase.PermissionUseCase.DTOs;

namespace Domic.UseCase.RoleUseCase.DTOs;

public class RoleDto
{
    public string Id          { get; set; }
    public string Name        { get; set; }
    public DateTime CreatedAt { get; set; }
    
    /*---------------------------------------------------------------*/
    
    public IEnumerable<PermissionDto> Permissions { get; set; }
}