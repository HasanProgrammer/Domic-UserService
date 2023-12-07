#pragma warning disable CS0649

using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Enumerations;
using Karami.Core.Domain.ValueObjects;
using Karami.Domain.Role.Events;
using Karami.Domain.Role.ValueObjects;

namespace Karami.Domain.Role.Entities;

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
    /// <param name="dotrisDateTime"></param>
    /// <param name="id"></param>
    /// <param name="name"></param>
    public Role(IDotrisDateTime dotrisDateTime, string id, string name)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dotrisDateTime.ToPersianShortDate(nowDateTime);
        
        Id        = id;
        Name      = new Name(name);
        CreatedAt = new CreatedAt(nowDateTime, nowPersianDateTime);
        UpdatedAt = new UpdatedAt(nowDateTime, nowPersianDateTime);
        IsActive  = IsActive.Active;
        
        AddEvent(
            new RoleCreated {
                Id                    = id                 ,
                Name                  = name               ,
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
    public void Change(IDotrisDateTime dotrisDateTime, string name)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dotrisDateTime.ToPersianShortDate(nowDateTime);
        
        Name      = new Name(name);
        UpdatedAt = new UpdatedAt(nowDateTime, nowPersianDateTime);
        IsActive  = IsActive.Active;
        
        AddEvent(
            new RoleUpdated {
                Id                    = Id   ,
                Name                  = name ,
                UpdatedAt_EnglishDate = nowDateTime  ,
                UpdatedAt_PersianDate = nowPersianDateTime
            }
        );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dotrisDateTime"></param>
    public void Delete(IDotrisDateTime dotrisDateTime)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dotrisDateTime.ToPersianShortDate(nowDateTime);

        IsDeleted = IsDeleted.Delete;
        UpdatedAt = new UpdatedAt(nowDateTime, nowPersianDateTime);
        
        AddEvent(
            new RoleDeleted {
                Id                    = Id          ,
                UpdatedAt_EnglishDate = nowDateTime ,
                UpdatedAt_PersianDate = nowPersianDateTime
            }
        );
    }
}