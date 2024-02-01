#pragma warning disable CS0649

using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Enumerations;
using Karami.Core.Domain.ValueObjects;
using Karami.Domain.User.ValueObjects;
using Karami.Domain.User.Events;

namespace Karami.Domain.User.Entities;

public class User : Entity<string>
{
    //Value Objects

    public FirstName FirstName     { get; private set; }
    public LastName LastName       { get; private set; }
    public Description Description { get; private set; }
    public Username Username       { get; private set; }
    public Password Password       { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public Email Email             { get; private set; }

    /*---------------------------------------------------------------*/
    
    //Relations

    public ICollection<RoleUser.Entities.RoleUser> RoleUsers                   { get; set; }
    public ICollection<PermissionUser.Entities.PermissionUser> PermissionUsers { get; set; }

    /*---------------------------------------------------------------*/

    //EF Core
    public User() {}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="id"></param>
    /// <param name="createdBy"></param>
    /// <param name="createdRole"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="description"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="email"></param>
    public User(IDateTime dateTime, string id, string createdBy, string createdRole, string firstName, string lastName, 
        string description, string username, string password, string phoneNumber, string email
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        Id          = id;
        CreatedBy   = createdBy;
        CreatedRole = createdRole;
        FirstName   = new FirstName(firstName);
        LastName    = new LastName(lastName);
        Description = new Description(description);
        Username    = new Username(username);
        Password    = new Password(password);
        PhoneNumber = new PhoneNumber(phoneNumber);
        Email       = new Email(email);
        CreatedAt   = new CreatedAt(nowDateTime, nowPersianDateTime);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="id"></param>
    /// <param name="createdBy"></param>
    /// <param name="createdRole"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="description"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="email"></param>
    /// <param name="roleIds"></param>
    /// <param name="permissionIds"></param>
    public User(IDateTime dateTime, string id, string createdBy, string createdRole, string firstName, string lastName, 
        string description, string username, string password, string phoneNumber, string email, 
        IEnumerable<string> roleIds, IEnumerable<string> permissionIds
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        Id          = id;
        CreatedBy   = createdBy;
        CreatedRole = createdRole;
        FirstName   = new FirstName(firstName);
        LastName    = new LastName(lastName);
        Description = new Description(description);
        Username    = new Username(username);
        Password    = new Password(password);
        PhoneNumber = new PhoneNumber(phoneNumber);
        Email       = new Email(email);
        CreatedAt   = new CreatedAt(nowDateTime, nowPersianDateTime);

        AddEvent(
            new UserCreated {
                Id                    = id                          ,
                CreatedBy             = createdBy                   , 
                CreatedRole           = createdRole                 , 
                FirstName             = firstName                   ,
                LastName              = lastName                    ,
                Username              = username                    ,
                Password              = password                    ,
                Description           = description                 ,
                PhoneNumber           = phoneNumber                 ,
                Email                 = email                       , 
                Roles                 = roleIds                     ,
                Permissions           = permissionIds               ,
                IsActive              = IsActive == IsActive.Active ,
                CreatedAt_EnglishDate = nowDateTime                 ,
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
    /// <param name="updateBy"></param>
    /// <param name="updateRole"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="description"></param>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="roleIds"></param>
    /// <param name="permissionIds"></param>
    /// <param name="password"></param>
    public void Change(IDateTime dateTime, string updateBy, string updateRole, string firstName, string lastName, 
        string description, string username, string email, string phoneNumber, IEnumerable<string> roleIds, 
        IEnumerable<string> permissionIds, string password = null
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        UpdatedBy   = updateBy;
        UpdatedRole = updateRole;
        FirstName   = new FirstName(firstName);
        LastName    = new LastName(lastName);
        Description = new Description(description);
        Username    = new Username(username);
        Email       = new Email(email);
        PhoneNumber = new PhoneNumber(phoneNumber);
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);

        if (password != null)
            Password = new Password(password);

        AddEvent(
            new UserUpdated {
                Id                    = Id                          ,
                FirstName             = firstName                   ,
                UpdatedBy             = updateBy                    , 
                UpdatedRole           = updateRole                  , 
                LastName              = lastName                    ,
                Username              = username                    ,
                Password              = password                    ,
                Description           = description                 ,
                PhoneNumber           = phoneNumber                 ,
                Email                 = email                       , 
                Roles                 = roleIds                     ,
                Permissions           = permissionIds               ,
                IsActive              = IsActive == IsActive.Active ,
                UpdatedAt_EnglishDate = nowDateTime                 ,
                UpdatedAt_PersianDate = nowPersianDateTime
            }
        );
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="updateBy"></param>
    /// <param name="updateRole"></param>
    /// <param name="ownerUsername"></param>
    public void Active(IDateTime dateTime, string updateBy, string updateRole, string ownerUsername)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        UpdatedBy   = updateBy;
        UpdatedRole = updateRole;
        IsActive    = IsActive.Active;
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);

        AddEvent(
            new UserActived {
                Id                    = Id            ,
                UpdatedBy             = updateBy      , 
                UpdatedRole           = updateRole    , 
                OwnerUsername         = ownerUsername ,
                UpdatedAt_EnglishDate = nowDateTime   ,
                UpdatedAt_PersianDate = nowPersianDateTime
            }
        );
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="updateBy"></param>
    /// <param name="updateRole"></param>
    /// <param name="ownerUsername"></param>
    public void InActive(IDateTime dateTime, string updateBy, string updateRole, string ownerUsername)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        UpdatedBy   = updateBy;
        UpdatedRole = updateRole;
        IsActive    = IsActive.InActive;
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);

        AddEvent(
            new UserInActived {
                Id                    = Id            ,
                UpdatedBy             = updateBy      ,
                UpdatedRole           = updateRole    ,
                OwnerUsername         = ownerUsername ,
                UpdatedAt_EnglishDate = nowDateTime   ,
                UpdatedAt_PersianDate = nowPersianDateTime
            }
        );
    }
}