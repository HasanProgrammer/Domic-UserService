#pragma warning disable CS0649

using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Domain.Permission.Entities;
using Domic.Domain.RoleUser.Entities;

namespace Domic.Domain.Role.Entities;

public class RoleQuery : EntityQuery<string>
{
    public string Name { get; set; }
    
    /*---------------------------------------------------------------*/
    
    //Relations
    
    public ICollection<RoleUserQuery> RoleUsers     { get; set; }
    public ICollection<PermissionQuery> Permissions { get; set; }
}