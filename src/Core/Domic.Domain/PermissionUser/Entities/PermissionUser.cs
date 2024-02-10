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
    /// <param name="dateTime"></param>
    /// <param name="id"></param>
    /// <param name="createdBy"></param>
    /// <param name="createdRole"></param>
    /// <param name="userId"></param>
    /// <param name="permissionId"></param>
    public PermissionUser(IDateTime dateTime, string id, string createdBy, string createdRole, string userId, 
        string permissionId
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        Id           = id;
        CreatedBy    = createdBy;
        CreatedRole  = createdRole;
        UserId       = userId;
        PermissionId = permissionId;
        CreatedAt    = new CreatedAt(nowDateTime, nowPersianDateTime);
    }
}