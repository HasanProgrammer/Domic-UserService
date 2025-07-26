namespace Domic.UseCase.PermissionUseCase.DTOs;

public class PermissionDto
{
    public string Id          { get; set; }
    public string Name        { get; set; }
    public string RoleId      { get; set; }
    public string RoleName    { get; set; }
    public DateTime CreatedAt { get; set; }
}