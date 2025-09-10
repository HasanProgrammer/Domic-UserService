using Domic.UseCase.PermissionUseCase.DTOs;
using Domic.UseCase.RoleUseCase.DTOs;

namespace Domic.UseCase.UserUseCase.DTOs;

public class UserDto
{
    public string Id { get; set; }
    public string ImageUrl { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    
    /*---------------------------------------------------------------*/
    
    public IEnumerable<RoleDto> Roles             { get; set; }
    public IEnumerable<PermissionDto> Permissions { get; set; }
}