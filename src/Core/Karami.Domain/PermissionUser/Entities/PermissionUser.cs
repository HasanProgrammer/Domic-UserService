using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Enumerations;
using Karami.Core.Domain.ValueObjects;

namespace Karami.Domain.PermissionUser.Entities;

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
    /// <param name="dotrisDateTime"></param>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <param name="permissionId"></param>
    public PermissionUser(IDotrisDateTime dotrisDateTime, string id, string userId, string permissionId)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dotrisDateTime.ToPersianShortDate(nowDateTime);
        
        Id           = id;
        UserId       = userId;
        PermissionId = permissionId;
        CreatedAt    = new CreatedAt(nowDateTime, nowPersianDateTime);
        UpdatedAt    = new UpdatedAt(nowDateTime, nowPersianDateTime);
        IsActive     = IsActive.Active;
    }
}