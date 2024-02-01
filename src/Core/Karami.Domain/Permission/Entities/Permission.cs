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
    /// <param name="dateTime"></param>
    /// <param name="id"></param>
    /// <param name="createdBy"></param>
    /// <param name="createdRole"></param>
    /// <param name="name"></param>
    /// <param name="roleId"></param>
    public Permission(IDateTime dateTime, string id, string createdBy, string createdRole, string name, string roleId)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        Id          = id;
        CreatedBy   = createdBy;
        CreatedRole = createdRole;
        RoleId      = roleId;
        Name        = new Name(name);
        CreatedAt   = new CreatedAt(nowDateTime, nowPersianDateTime);
        IsActive    = IsActive.Active;

        AddEvent(
            new PermissionCreated {
                Id          = id          ,
                CreatedBy   = createdBy   ,
                CreatedRole = createdRole ,
                RoleId      = roleId      ,
                Name        = name        ,
                CreatedAt_EnglishDate = nowDateTime ,
                CreatedAt_PersianDate = nowPersianDateTime 
            }
        );
    }
    
    /*---------------------------------------------------------------*/
        
    //Behaviors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="updatedBy"></param>
    /// <param name="updatedRole"></param>
    /// <param name="name"></param>
    /// <param name="roleId"></param>
    public void Change(IDateTime dateTime, string updatedBy, string updatedRole, string name, string roleId)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        UpdatedBy   = updatedBy;
        UpdatedRole = updatedRole;
        RoleId      = roleId;
        Name        = new Name(name);
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);

        AddEvent(
            new PermissionUpdated {
                Id                    = Id          ,
                UpdatedBy             = updatedBy   ,
                UpdatedRole           = updatedRole , 
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
    /// <param name="dateTime"></param>
    /// <param name="updatedBy"></param>
    /// <param name="updatedRole"></param>
    /// <param name="raiseEvent"></param>
    public void Delete(IDateTime dateTime, string updatedBy, string updatedRole, bool raiseEvent = true)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        UpdatedBy   = updatedBy;
        UpdatedRole = updatedRole;
        IsDeleted   = IsDeleted.Delete;
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);
        
        if(raiseEvent)
            AddEvent(
                new PermissionDeleted {
                    Id                    = Id          ,
                    UpdatedBy             = updatedBy   ,
                    UpdatedRole           = updatedRole , 
                    UpdatedAt_EnglishDate = nowDateTime ,
                    UpdatedAt_PersianDate = nowPersianDateTime
                }
            );
    }
}