using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.ValueObjects;

namespace Domic.Domain.RoleUser.Entities;

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
    /// <param name="globalUniqueIdGenerator"></param>
    /// <param name="dateTime"></param>
    /// <param name="userId"></param>
    /// <param name="roleId"></param>
    /// <param name="createdBy"></param>
    /// <param name="createdRole"></param>
    public RoleUser(IGlobalUniqueIdGenerator globalUniqueIdGenerator, IDateTime dateTime, string userId, string roleId,
        string createdBy, string createdRole
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        Id          = globalUniqueIdGenerator.GetRandom(6);
        UserId      = userId;
        RoleId      = roleId;
        CreatedBy   = createdBy;
        CreatedRole = createdRole;
        CreatedAt   = new CreatedAt(nowDateTime, nowPersianDateTime);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="globalUniqueIdGenerator"></param>
    /// <param name="dateTime"></param>
    /// <param name="identityUser"></param>
    /// <param name="serializer"></param>
    /// <param name="userId"></param>
    /// <param name="roleId"></param>
    public RoleUser(IGlobalUniqueIdGenerator globalUniqueIdGenerator, IDateTime dateTime, IIdentityUser identityUser, 
        ISerializer serializer, string userId , string roleId
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        Id          = globalUniqueIdGenerator.GetRandom(6);
        UserId      = userId;
        RoleId      = roleId;
        CreatedBy   = identityUser.GetIdentity();
        CreatedRole = serializer.Serialize(identityUser.GetRoles());
        CreatedAt   = new CreatedAt(nowDateTime, nowPersianDateTime);
    }
}