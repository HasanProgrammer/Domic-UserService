#pragma warning disable CS0649

using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.Enumerations;
using Domic.Core.Domain.ValueObjects;
using Domic.Domain.Role.Events;
using Domic.Domain.Role.ValueObjects;

namespace Domic.Domain.Role.Entities;

public class Role : Entity<string>
{
    //Value Objects
    
    public Name Name { get; private set; }
    
    /*---------------------------------------------------------------*/
    
    //Relations
    
    public ICollection<RoleUser.Entities.RoleUser> RoleUsers       { get; set; }
    public ICollection<Permission.Entities.Permission> Permissions { get; set; }
    
    /*---------------------------------------------------------------*/

    //EF Core
    private Role() {}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="id"></param>
    /// <param name="createdBy"></param>
    /// <param name="createdRole"></param>
    /// <param name="name"></param>
    public Role(IDateTime dateTime, string id, string createdBy, string createdRole, string name)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        Id          = id;
        Name        = new Name(name);
        CreatedBy   = createdBy;
        CreatedRole = createdRole;
        CreatedAt   = new CreatedAt(nowDateTime, nowPersianDateTime);
        
        AddEvent(
            new RoleCreated {
                Id                    = id                 ,
                CreatedBy             = createdBy          , 
                CreatedRole           = createdRole        , 
                Name                  = name               ,
                CreatedAt_EnglishDate = nowDateTime        ,
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
    public Role(IGlobalUniqueIdGenerator globalUniqueIdGenerator, IDateTime dateTime, IIdentityUser identityUser, 
        ISerializer serializer, string name
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        Id          = globalUniqueIdGenerator.GetRandom(6);
        Name        = new Name(name);
        CreatedBy   = identityUser.GetIdentity();
        CreatedRole = serializer.Serialize(identityUser.GetRoles());
        CreatedAt   = new CreatedAt(nowDateTime, nowPersianDateTime);
        
        AddEvent(
            new RoleCreated {
                Id                    = Id                 ,
                Name                  = name               ,
                CreatedBy             = CreatedBy          , 
                CreatedRole           = CreatedRole        , 
                CreatedAt_EnglishDate = nowDateTime        ,
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
    public void Change(IDateTime dateTime, IIdentityUser identityUser, ISerializer serializer, string name)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        Name        = new Name(name);
        UpdatedBy   = identityUser.GetIdentity();
        UpdatedRole = serializer.Serialize(identityUser.GetRoles());
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);
        
        AddEvent(
            new RoleUpdated {
                Id                    = Id          ,
                UpdatedBy             = UpdatedBy   ,
                UpdatedRole           = UpdatedRole ,
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
    /// <param name="identityUser"></param>
    /// <param name="serializer"></param>
    public void Delete(IDateTime dateTime, IIdentityUser identityUser, ISerializer serializer)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        IsDeleted   = IsDeleted.Delete;
        UpdatedBy   = identityUser.GetIdentity();
        UpdatedRole = serializer.Serialize(identityUser.GetRoles());
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);
        
        AddEvent(
            new RoleDeleted {
                Id                    = Id          ,
                UpdatedBy             = UpdatedBy   ,
                UpdatedRole           = UpdatedRole ,
                UpdatedAt_EnglishDate = nowDateTime ,
                UpdatedAt_PersianDate = nowPersianDateTime
            }
        );
    }
}