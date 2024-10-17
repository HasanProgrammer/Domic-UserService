using Domic.Core.Infrastructure.Concretes;
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
        var dateTime          = new DateTime();
        var persianDateTime   = new DomicDateTime();
        var uniqueIdGenerator = new GlobalUniqueIdGenerator();
        var userId            = uniqueIdGenerator.GetRandom(6);

        #region Role Seeder

        var adminRoleId = uniqueIdGenerator.GetRandom(6);
        var newSuperAdminRole = new Role(persianDateTime, adminRoleId, userId, "SuperAdmin", "SuperAdmin");

        var clientRoleId = uniqueIdGenerator.GetRandom(6);
        var newClientRole = new Role(persianDateTime, clientRoleId, userId, "Client", "Client");
        
        _roleIds.Add(adminRoleId);
        _roleIds.Add(clientRoleId);
        
        context.Roles.Add(newSuperAdminRole);
        context.Roles.Add(newClientRole);

        #endregion

        #region Permission Seeder

        _permissionsBuilder(context, adminRoleId, uniqueIdGenerator, dateTime, persianDateTime, userId, "SuperAdmin");
        _permissionsClientBuilder(context, clientRoleId, uniqueIdGenerator, dateTime, persianDateTime, userId, "SuperAdmin");

        #endregion

        #region User Seeder
        
        const string description = "من حسن کرمی محب ؛ برنامه نویس و عاشق معماری های برنامه نویسی ، 26 ساله ، کشور ایران و اهل شهرستان شهریار هستم";
        
        var newUser = new User(new DomicDateTime(), userId, userId, "SuperAdmin", "Hasan", "Domic Moheb", description, 
            "Hasan_Domic_Moheb", "Hasan313@@313!!", "09026676147", "hasanDomic2020313@gmail.com", _roleIds, 
            _permissionIds
        );

        context.Users.Add(newUser);

        #endregion

        #region RoleUser Seeder

        var newRoleUser =
            new RoleUser(new DomicDateTime(), uniqueIdGenerator.GetRandom(6), userId, "SuperAdmin", userId, adminRoleId);

        context.RoleUsers.Add(newRoleUser);

        #endregion
        
        #region PermissionUser Seeder

        _permissionsUsersBuilder(context, userId, uniqueIdGenerator, userId, "SuperAdmin");

        #endregion

        context.SaveChanges();
    }

    private static void _permissionsBuilder(SQLContext context, string roleId, 
        GlobalUniqueIdGenerator uniqueIdGenerator, DateTime dateTime, DomicDateTime domicDateTime, string createdBy, 
        string createdRole
    )
    {
        var permissions = Core.Common.ClassConsts.Permission.GetAll();

        foreach (var permission in permissions)
        {
            var uniqueId = uniqueIdGenerator.GetRandom(6);
            
            _permissionIds.Add(uniqueId);
            
            var newPermission =
                new Permission(new DomicDateTime(), uniqueId, createdBy, createdRole, permission, roleId);

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
                new Permission(new DomicDateTime(), uniqueId, createdBy, createdRole, permission, roleId);

            context.Permissions.Add(newPermission);
        }
    }
    
    private static void _permissionsClientBuilder(SQLContext context, string roleId, 
        GlobalUniqueIdGenerator uniqueIdGenerator, DateTime dateTime, DomicDateTime domicDateTime, string createdBy, 
        string createdRole
    )
    {
        List<string> newPermissions = new() {
            "Ticket.Create",
            "AggregateTicket.ReadOne",
            "AggregateTicket.ReadAllPaginated"
        };
        
        foreach (var permission in newPermissions)
        {
            var uniqueId = uniqueIdGenerator.GetRandom(6);
            
            _permissionIds.Add(uniqueId);
            
            var newPermission =
                new Permission(new DomicDateTime(), uniqueId, createdBy, createdRole, permission, roleId);

            context.Permissions.Add(newPermission);
        }
    }

    private static void _permissionsUsersBuilder(SQLContext context, string userId, 
        GlobalUniqueIdGenerator uniqueIdGenerator, string createdBy, string createdRole
    )
    {
        foreach (var permissionId in _permissionIds)
        {
            var newPermissionUser =
                new PermissionUser(new DomicDateTime(), uniqueIdGenerator.GetRandom(6), createdBy, createdRole, userId, 
                    permissionId
                );

            context.PermissionUsers.Add(newPermissionUser);
        }
    }
}