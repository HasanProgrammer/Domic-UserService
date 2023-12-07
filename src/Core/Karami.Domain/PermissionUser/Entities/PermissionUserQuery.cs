using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Domain.Permission.Entities;
using Karami.Domain.User.Entities;

namespace Karami.Domain.PermissionUser.Entities;

public class PermissionUserQuery : BaseEntityQuery<string>
{
    public string UserId       { get; set; }
    public string PermissionId { get; set; }
    
    /*---------------------------------------------------------------*/
    
    //Relations
    
    public UserQuery User             { get; set; }
    public PermissionQuery Permission { get; set; }
}