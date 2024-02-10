using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Domain.Role.Entities;
using Domic.Domain.User.Entities;

namespace Domic.Domain.RoleUser.Entities;

public class RoleUserQuery : EntityQuery<string>
{
    public string UserId { get; set; }
    public string RoleId { get; set; }
    
    /*---------------------------------------------------------------*/
    
    //Relations
    
    public RoleQuery Role { get; set; }
    public UserQuery User { get; set; }
}