using Domic.Domain.Permission.Events;
using Domic.Domain.Role.ValueObjects;
using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.Enumerations;
using Domic.Core.Domain.ValueObjects;

namespace Domic.Domain.Permission.Entities;

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
    /// <param name="uniqueId"></param>
    /// <param name="createdBy"></param>
    /// <param name="createdRole"></param>
    /// <param name="name"></param>
    /// <param name="roleId"></param>
    public Permission(IDateTime dateTime, string uniqueId, string createdBy, string createdRole, string name,
        string roleId
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        Id          = uniqueId;
        CreatedBy   = createdBy;
        CreatedRole = createdRole;
        RoleId      = roleId;
        Name        = new Name(name);
        CreatedAt   = new CreatedAt(nowDateTime, nowPersianDateTime);
        IsActive    = IsActive.Active;

        AddEvent(
            new PermissionCreated {
                Id          = uniqueId    ,
                CreatedBy   = createdBy   ,
                CreatedRole = createdRole ,
                RoleId      = roleId      ,
                Name        = name        ,
                CreatedAt_EnglishDate = nowDateTime ,
                CreatedAt_PersianDate = nowPersianDateTime 
            }
        );
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="globalUniqueIdGenerator"></param>
    /// <param name="dateTime"></param>
    /// <param name="identityUser"></param>
    /// <param name="serializer"></param>
    /// <param name="name"></param>
    /// <param name="roleId"></param>
    public Permission(IGlobalUniqueIdGenerator globalUniqueIdGenerator, IDateTime dateTime, IIdentityUser identityUser, 
        ISerializer serializer, string name, string roleId
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        Id          = globalUniqueIdGenerator.GetRandom(6);
        RoleId      = roleId;
        Name        = new Name(name);
        IsActive    = IsActive.Active;
        CreatedBy   = identityUser.GetIdentity();
        CreatedRole = serializer.Serialize(identityUser.GetRoles());
        CreatedAt   = new CreatedAt(nowDateTime, nowPersianDateTime);

        AddEvent(
            new PermissionCreated {
                Id          = Id          ,
                RoleId      = roleId      ,
                Name        = name        ,
                CreatedBy   = CreatedBy   ,
                CreatedRole = CreatedRole ,
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
    /// <param name="identityUser"></param>
    /// <param name="serializer"></param>
    /// <param name="name"></param>
    /// <param name="roleId"></param>
    public void Change(IDateTime dateTime, IIdentityUser identityUser, ISerializer serializer, 
        string name, string roleId
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        RoleId      = roleId;
        Name        = new Name(name);
        UpdatedRole = serializer.Serialize(identityUser.GetRoles());
        UpdatedBy   = identityUser.GetIdentity();
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);

        AddEvent(
            new PermissionUpdated {
                Id                    = Id          ,
                RoleId                = roleId      ,
                Name                  = name        ,
                UpdatedBy             = UpdatedBy   ,
                UpdatedRole           = UpdatedRole , 
                UpdatedAt_EnglishDate = nowDateTime ,
                UpdatedAt_PersianDate = nowPersianDateTime
            }
        );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="identityUser"></param>
    /// <param name="serializer"></param>
    /// <param name="raiseEvent"></param>
    public void Delete(IDateTime dateTime, IIdentityUser identityUser, ISerializer serializer, bool raiseEvent = true)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        UpdatedBy   = identityUser.GetIdentity();
        UpdatedRole = serializer.Serialize(identityUser.GetRoles());
        IsDeleted   = IsDeleted.Delete;
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);
        
        if(raiseEvent)
            AddEvent(
                new PermissionDeleted {
                    Id                    = Id          ,
                    UpdatedBy             = UpdatedBy   ,
                    UpdatedRole           = UpdatedRole , 
                    UpdatedAt_EnglishDate = nowDateTime ,
                    UpdatedAt_PersianDate = nowPersianDateTime
                }
            );
    }
}