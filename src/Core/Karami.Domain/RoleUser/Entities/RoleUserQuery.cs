using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Domain.Role.Entities;
using Karami.Domain.User.Entities;

namespace Karami.Domain.RoleUser.Entities;

public class RoleUserQuery : EntityQuery<string>
{
    public string UserId { get; set; }
    public string RoleId { get; set; }
    
    /*---------------------------------------------------------------*/
    
    //Relations
    
    public RoleQuery Role { get; set; }
    public UserQuery User { get; set; }
}