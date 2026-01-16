#pragma warning disable CS0649

using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Domain.Enumerations;
using Domic.Core.Domain.ValueObjects;
using Domic.Domain.User.Events;
using Domic.Domain.User.ValueObjects;

namespace Domic.Domain.User.Entities;

public class User : Entity<string>
{
    //Fields
    
    public string ImageUrl { get; private set; }
    
    /*---------------------------------------------------------------*/
    
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
    /// <param name="imageUrl"></param>
    public User(IDateTime dateTime, string id, string createdBy, string createdRole, string firstName, string lastName, 
        string description, string username, string password, string phoneNumber, string email, string imageUrl
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        Id          = id;
        ImageUrl    = imageUrl;
        FirstName   = new FirstName(firstName);
        LastName    = new LastName(lastName);
        Description = new Description(description);
        Username    = new Username(username);
        Password    = new Password(password);
        PhoneNumber = new PhoneNumber(phoneNumber);
        Email       = new Email(email);
        
        //audit
        CreatedBy   = createdBy;
        CreatedRole = createdRole;
        CreatedAt   = new CreatedAt(nowDateTime, nowPersianDateTime);
    }
    
    public User(IDateTime dateTime, string id, string createdBy, string createdRole, string firstName, string lastName, 
        string description, string username, string password, string phoneNumber, string email,
        IEnumerable<string> roleIds, IEnumerable<string> permissionIds
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        Id          = id;
        FirstName   = new FirstName(firstName);
        LastName    = new LastName(lastName);
        Description = new Description(description);
        Username    = new Username(username);
        Password    = new Password(password);
        PhoneNumber = new PhoneNumber(phoneNumber);
        Email       = new Email(email);
        
        //audit
        CreatedBy   = createdBy;
        CreatedRole = createdRole;
        CreatedAt   = new CreatedAt(nowDateTime, nowPersianDateTime);

        AddEvent(
            new UserCreated {
                Id                    = Id                          ,
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
                CreatedBy             = CreatedBy                   , 
                CreatedRole           = CreatedRole                 , 
                CreatedAt_EnglishDate = nowDateTime                 ,
                CreatedAt_PersianDate = nowPersianDateTime
            }
        );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="identityUser"></param>
    /// <param name="serializer"></param>
    /// <param name="globalUniqueIdGenerator"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="description"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="email"></param>
    /// <param name="imageUrl"></param>
    /// <param name="roleIds"></param>
    /// <param name="permissionIds"></param>
    public User(IDateTime dateTime, IIdentityUser identityUser, ISerializer serializer, 
        IGlobalUniqueIdGenerator globalUniqueIdGenerator, string firstName, string lastName, string description, 
        string username, string password, string phoneNumber, string email, string imageUrl, IEnumerable<string> roleIds, 
        IEnumerable<string> permissionIds
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        Id          = globalUniqueIdGenerator.GetRandom(6);
        ImageUrl    = imageUrl;
        FirstName   = new FirstName(firstName);
        LastName    = new LastName(lastName);
        Description = new Description(description);
        Username    = new Username(username);
        Password    = new Password(password);
        PhoneNumber = new PhoneNumber(phoneNumber);
        Email       = new Email(email);
        
        //audit
        CreatedBy   = identityUser.GetIdentity();
        CreatedRole = serializer.Serialize(identityUser.GetRoles());
        CreatedAt   = new CreatedAt(nowDateTime, nowPersianDateTime);

        AddEvent(
            new UserCreated {
                Id                    = Id                          ,
                ImageUrl              = imageUrl                    ,
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
                CreatedBy             = CreatedBy                   , 
                CreatedRole           = CreatedRole                 , 
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
    /// <param name="identityUser"></param>
    /// <param name="serializer"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="description"></param>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="imageUrl"></param>
    /// <param name="roleIds"></param>
    /// <param name="permissionIds"></param>
    /// <param name="password"></param>
    public void Change(IDateTime dateTime, IIdentityUser identityUser, ISerializer serializer, string firstName, 
        string lastName, string description, string username, string email, string phoneNumber, 
        string imageUrl, IEnumerable<string> roleIds, IEnumerable<string> permissionIds, string password = null
    )
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        ImageUrl    = imageUrl;
        FirstName   = new FirstName(firstName);
        LastName    = new LastName(lastName);
        Description = new Description(description);
        Username    = new Username(username);
        Email       = new Email(email);
        PhoneNumber = new PhoneNumber(phoneNumber);
        
        //audit
        UpdatedBy   = identityUser.GetIdentity();
        UpdatedRole = serializer.Serialize(identityUser.GetRoles());
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);

        if (password != null)
            Password = new Password(password);

        AddEvent(
            new UserUpdated {
                Id                    = Id            ,
                ImageUrl              = imageUrl      , 
                FirstName             = firstName     ,
                UpdatedBy             = UpdatedBy     , 
                UpdatedRole           = UpdatedRole   , 
                LastName              = lastName      ,
                Username              = username      ,
                Password              = password      ,
                Description           = description   ,
                PhoneNumber           = phoneNumber   ,
                Email                 = email         , 
                Roles                 = roleIds       ,
                Permissions           = permissionIds ,
                UpdatedAt_EnglishDate = nowDateTime   ,
                UpdatedAt_PersianDate = nowPersianDateTime
            }
        );
    }
    
    public void ResetPassword(IDateTime dateTime, string password, string updatedBy, string updatedRole)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);
        
        Password = new Password(password);
        
        //audit
        UpdatedBy   = updatedBy;
        UpdatedRole = updatedRole;
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);

        AddEvent(
            new UserPasswordChanged {
                Id                    = Id            ,
                UpdatedBy             = UpdatedBy     , 
                UpdatedRole           = UpdatedRole   ,
                NewPassword           = password      ,
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
    public void Active(IDateTime dateTime, IIdentityUser identityUser, ISerializer serializer)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        IsActive = IsActive.Active;
        
        //audit
        UpdatedBy   = identityUser.GetIdentity();
        UpdatedRole = serializer.Serialize(identityUser.GetRoles());
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);

        AddEvent(
            new UserActived {
                Id                    = Id            ,
                UpdatedBy             = UpdatedBy     , 
                UpdatedRole           = UpdatedRole   ,
                UpdatedAt_EnglishDate = nowDateTime   ,
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
    public void InActive(IDateTime dateTime, IIdentityUser identityUser, ISerializer serializer)
    {
        var nowDateTime        = DateTime.Now;
        var nowPersianDateTime = dateTime.ToPersianShortDate(nowDateTime);

        IsActive = IsActive.InActive;
        
        //audit
        UpdatedBy   = identityUser.GetIdentity();
        UpdatedRole = serializer.Serialize(identityUser.GetRoles());
        UpdatedAt   = new UpdatedAt(nowDateTime, nowPersianDateTime);

        AddEvent(
            new UserInActived {
                Id                    = Id            ,
                UpdatedBy             = UpdatedBy     ,
                UpdatedRole           = UpdatedRole   ,
                UpdatedAt_EnglishDate = nowDateTime   ,
                UpdatedAt_PersianDate = nowPersianDateTime
            }
        );
    }
}