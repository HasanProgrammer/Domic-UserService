using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Domain.Permission.Entities;
using Domic.Domain.User.Entities;

namespace Domic.Domain.PermissionUser.Entities;

public class PermissionUserQuery : EntityQuery<string>
{
    public string UserId       { get; set; }
    public string PermissionId { get; set; }
    
    /*---------------------------------------------------------------*/
    
    //Relations
    
    public UserQuery User             { get; set; }
    public PermissionQuery Permission { get; set; }
}