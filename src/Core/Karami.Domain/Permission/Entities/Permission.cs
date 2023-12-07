using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Enumerations;
using Karami.Core.Domain.ValueObjects;
using Karami.Domain.Permission.Events;
using Karami.Domain.Role.ValueObjects;

namespace Karami.Domain.Permission.Entities;

public class Permission : Entity<string>
{
    //Value Objects
    
    public string RoleId { get; private set; }
    public Name Name     { get; private set; }
    
    /*---------------------------------------------------------------*/
    
    //Relations
    
    public Role.Entities.Role Role { get; set; }
    
    public ICollection<PermissionUser.Entities.PermissionUser> PermissionUsers { get; set; }

    /*---------------------------------------------------------------*/
    
    //EF Core
    private Permission() {}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dotrisDateTime"></param>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="roleId"></param>
    public Permission(IDotrisDateTime dotrisDateTime, string id, string name, string roleId)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dotrisDateTime.ToPersianShortDate(nowDateTime);

        Id        = id;
        RoleId    = roleId;
        Name      = new Name(name);
        CreatedAt = new CreatedAt(nowDateTime, nowPersianDateTime);
        UpdatedAt = new UpdatedAt(nowDateTime, nowPersianDateTime);
        IsActive  = IsActive.Active;

        AddEvent(
            new PermissionCreated {
                Id     = id     ,
                RoleId = roleId ,
                Name   = name   ,
                CreatedAt_EnglishDate = nowDateTime        ,
                UpdatedAt_EnglishDate = nowDateTime        ,
                CreatedAt_PersianDate = nowPersianDateTime ,
                UpdatedAt_PersianDate = nowPersianDateTime
            }
        );
    }
    
    /*---------------------------------------------------------------*/
        
    //Behaviors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dotrisDateTime"></param>
    /// <param name="name"></param>
    /// <param name="roleId"></param>
    public void Change(IDotrisDateTime dotrisDateTime, string name, string roleId)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dotrisDateTime.ToPersianShortDate(nowDateTime);
        
        RoleId    = roleId;
        Name      = new Name(name);
        UpdatedAt = new UpdatedAt(nowDateTime, nowPersianDateTime);

        AddEvent(
            new PermissionUpdated {
                Id                    = Id          ,
                RoleId                = roleId      ,
                Name                  = name        ,
                UpdatedAt_EnglishDate = nowDateTime ,
                UpdatedAt_PersianDate = nowPersianDateTime
            }
        );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dotrisDateTime"></param>
    /// <param name="raiseEvent"></param>
    public void Delete(IDotrisDateTime dotrisDateTime, bool raiseEvent = true)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dotrisDateTime.ToPersianShortDate(nowDateTime);

        IsDeleted = IsDeleted.Delete;
        UpdatedAt = new UpdatedAt(nowDateTime, nowPersianDateTime);
        
        if(raiseEvent)
            AddEvent(
                new PermissionDeleted {
                    Id                    = Id          ,
                    UpdatedAt_EnglishDate = nowDateTime ,
                    UpdatedAt_PersianDate = nowPersianDateTime
                }
            );
    }
}