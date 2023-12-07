using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Enumerations;
using Karami.Core.Domain.ValueObjects;

namespace Karami.Domain.RoleUser.Entities;

public class RoleUser : Entity<string>
{
    //Value Objects
    
    public string UserId { get; private set; }
    public string RoleId { get; private set; }
    
    /*---------------------------------------------------------------*/
    
    //Relations
    
    public Role.Entities.Role Role        { get; set; }
    public Domain.User.Entities.User User { get; set; }
    
    /*---------------------------------------------------------------*/

    //EF Core
    private RoleUser() {}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dotrisDateTime"></param>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <param name="roleId"></param>
    public RoleUser(IDotrisDateTime dotrisDateTime, string id, string userId , string roleId)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dotrisDateTime.ToPersianShortDate(nowDateTime);

        Id        = id;
        UserId    = userId;
        RoleId    = roleId;
        CreatedAt = new CreatedAt(nowDateTime, nowPersianDateTime);
        UpdatedAt = new UpdatedAt(nowDateTime, nowPersianDateTime);
        IsActive  = IsActive.Active;
    }
}