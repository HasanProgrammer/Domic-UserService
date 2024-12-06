using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.Infrastructure.Concretes;
using Domic.Core.Infrastructure.Extensions;
using Domic.Domain.PermissionUser.Entities;
using Domic.Domain.Role.Entities;
using Domic.Domain.RoleUser.Entities;
using Domic.Domain.User.Entities;
using Domic.Persistence.Contexts.C;

using Permission = Domic.Domain.Permission.Entities.Permission;

namespace Domic.Infrastructure.Extensions.C;

public static class SQLContextExtension
{
    private static List<string> _roleIds       = new();
    private static List<string> _permissionIds = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public static void Seed(this SQLContext context)
    {
        var persianDateTime   = new DomicDateTime();
        var uniqueIdGenerator = new GlobalUniqueIdGenerator();
        var userId            = uniqueIdGenerator.GetRandom(6);

        #region Role Seeder

        var adminRoleId = uniqueIdGenerator.GetRandom(6);
        var newSuperAdminRole = new Role(persianDateTime, adminRoleId, userId, new List<string>{ "SuperAdmin" }.Serialize(), "SuperAdmin");

        var clientRoleId = uniqueIdGenerator.GetRandom(6);
        var newClientRole = new Role(persianDateTime, clientRoleId, userId, new List<string>{ "Client" }.Serialize(), "Client");
        
        _roleIds.Add(adminRoleId);
        
        context.Roles.Add(newSuperAdminRole);
        context.Roles.Add(newClientRole);

        #endregion

        #region Permission Seeder

        _permissionsBuilder(context, adminRoleId, uniqueIdGenerator, persianDateTime, userId, new List<string>{ "SuperAdmin" }.Serialize());
        _permissionsClientBuilder(context, clientRoleId, uniqueIdGenerator, persianDateTime, userId, new List<string>{ "SuperAdmin" }.Serialize());

        #endregion

        #region User Seeder
        
        const string description = "من حسن کرمی محب ؛ برنامه نویس و عاشق معماری های برنامه نویسی ، 27 ساله ، کشور ایران و اهل شهرستان شهریار هستم";
        
        var newUser = new User(persianDateTime, userId, userId, new List<string> { "SuperAdmin" }.Serialize(), "Hasan", "Karami Moheb", description, 
            "Hasan_Karami_Moheb", "Domic123!@#", "09026676147", "hasankarami2020313@gmail.com", _roleIds, 
            _permissionIds
        );

        context.Users.Add(newUser);

        #endregion

        #region RoleUser Seeder

        var newRoleUser =
            new RoleUser(uniqueIdGenerator, persianDateTime, userId, adminRoleId, userId, 
                new List<string> { "SuperAdmin" }.Serialize()
            );

        context.RoleUsers.Add(newRoleUser);

        #endregion
        
        #region PermissionUser Seeder

        _permissionsUsersBuilder(context, userId, uniqueIdGenerator, persianDateTime, userId,
            new List<string> { "SuperAdmin" }.Serialize()
        );

        #endregion

        context.SaveChanges();
    }

    private static void _permissionsBuilder(SQLContext context, string roleId, 
        IGlobalUniqueIdGenerator uniqueIdGenerator, IDateTime dateTime, string createdBy, 
        string createdRole
    )
    {
        var permissions = Core.Common.ClassConsts.Permission.GetAll();

        foreach (var permission in permissions)
        {
            var uniqueId = uniqueIdGenerator.GetRandom(6);
            
            _permissionIds.Add(uniqueId);
            
            var newPermission =
                new Permission(dateTime, uniqueId, createdBy, createdRole, permission, roleId);

            context.Permissions.Add(newPermission);
        }

        List<string> newPermissions = new() {
            "AggregateTicket.ReadOne",
            "AggregateTicket.ReadAllPaginated"
        };
        
        foreach (var permission in newPermissions)
        {
            var uniqueId = uniqueIdGenerator.GetRandom(6);
            
            _permissionIds.Add(uniqueId);
            
            var newPermission =
                new Permission(dateTime, uniqueId, createdBy, createdRole, permission, roleId);

            context.Permissions.Add(newPermission);
        }
    }
    
    private static void _permissionsClientBuilder(SQLContext context, string roleId, 
        IGlobalUniqueIdGenerator uniqueIdGenerator, IDateTime dateTime, string createdBy, 
        string createdRole
    )
    {
        List<string> newPermissions = new() {
            "Ticket.Create",
            "AggregateTicket.ReadOne",
            "AggregateTicket.ReadAllPaginated",
            "Financial.Create",
            "Financial.PaymentVerification"
        };
        
        foreach (var permission in newPermissions)
        {
            var uniqueId = uniqueIdGenerator.GetRandom(6);
            
            _permissionIds.Add(uniqueId);
            
            var newPermission =
                new Permission(dateTime, uniqueId, createdBy, createdRole, permission, roleId);

            context.Permissions.Add(newPermission);
        }
    }

    private static void _permissionsUsersBuilder(SQLContext context, string userId, 
        IGlobalUniqueIdGenerator uniqueIdGenerator, IDateTime dateTime, string createdBy, string createdRole
    )
    {
        foreach (var permissionId in _permissionIds)
        {
            var newPermissionUser =
                new PermissionUser(uniqueIdGenerator, dateTime, userId, permissionId, createdBy, createdRole);

            context.PermissionUsers.Add(newPermissionUser);
        }
    }
}