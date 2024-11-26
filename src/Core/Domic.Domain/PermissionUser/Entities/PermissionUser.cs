using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.ValueObjects;

namespace Domic.Domain.PermissionUser.Entities;

public class PermissionUser : Entity<string>
{
    //Value Objects
    
    public string UserId       { get; private set; }
    public string PermissionId { get; private set; }
    
    /*---------------------------------------------------------------*/
    
    //Relations
    
    public User.Entities.User User                   { get; set; }
    public Permission.Entities.Permission Permission { get; set; }
    
    /*---------------------------------------------------------------*/
    
    //EF Core
    private PermissionUser() {}
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="globalUniqueIdGenerator"></param>
    /// <param name="dateTime"></param>
    /// <param name="userId"></param>
    /// <param name="permissionId"></param>
    /// <param name="createdBy"></param>
    /// <param name="createdRole"></param>
    public PermissionUser(IGlobalUniqueIdGenerator globalUniqueIdGenerator, IDateTime dateTime, 
        string userId, string permissionId, string createdBy, string createdRole
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        Id           = globalUniqueIdGenerator.GetRandom(6);
        CreatedBy    = createdBy;
        CreatedRole  = createdRole;
        UserId       = userId;
        PermissionId = permissionId;
        CreatedAt    = new CreatedAt(nowDateTime, nowPersianDateTime);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="globalUniqueIdGenerator"></param>
    /// <param name="dateTime"></param>
    /// <param name="identityUser"></param>
    /// <param name="serializer"></param>
    /// <param name="userId"></param>
    /// <param name="permissionId"></param>
    public PermissionUser(IGlobalUniqueIdGenerator globalUniqueIdGenerator, IDateTime dateTime, 
        IIdentityUser identityUser, ISerializer serializer, string userId, string permissionId
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        Id           = globalUniqueIdGenerator.GetRandom(6);
        CreatedBy    = identityUser.GetIdentity();
        CreatedRole  = serializer.Serialize(identityUser.GetRoles());
        UserId       = userId;
        PermissionId = permissionId;
        CreatedAt    = new CreatedAt(nowDateTime, nowPersianDateTime);
    }
}