using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Domain.PermissionUser.Entities;
using Karami.Domain.Role.Entities;

namespace Karami.Domain.Permission.Entities;

public class PermissionQuery : BaseEntityQuery<string>
{
    public string RoleId { get; set; }
    public string Name   { get; set; }
    
    /*---------------------------------------------------------------*/
    
    //Relations
    
    public RoleQuery Role { get; set; }
    
    public ICollection<PermissionUserQuery> PermissionUsers { get; set; }
}