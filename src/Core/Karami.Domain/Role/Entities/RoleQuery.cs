#pragma warning disable CS0649

using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Domain.Permission.Entities;
using Karami.Domain.RoleUser.Entities;

namespace Karami.Domain.Role.Entities;

public class RoleQuery : BaseEntityQuery<string>
{
    public string Name { get; set; }
    
    /*---------------------------------------------------------------*/
    
    //Relations
    
    public ICollection<RoleUserQuery> RoleUsers     { get; set; }
    public ICollection<PermissionQuery> Permissions { get; set; }
}