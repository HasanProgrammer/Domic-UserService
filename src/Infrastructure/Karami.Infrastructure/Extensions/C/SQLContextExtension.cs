using Karami.Core.Domain.Implementations;
using Karami.Core.Infrastructure.Implementations;
using Karami.Domain.PermissionUser.Entities;
using Karami.Domain.Role.Entities;
using Karami.Domain.RoleUser.Entities;
using Karami.Domain.User.Entities;
using Karami.Persistence.Contexts.C;

using Permission = Karami.Domain.Permission.Entities.Permission;

namespace Karami.Infrastructure.Extensions.C;

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

        var roleId  = uniqueIdGenerator.GetRandom(6);
        var newRole = new Role(persianDateTime, roleId, userId, "SuperAdmin", "SuperAdmin");

        _roleIds.Add(roleId);
        
        context.Roles.Add(newRole);

        #endregion

        #region Permission Seeder

        _permissionsBuilder(context, roleId, uniqueIdGenerator, dateTime, persianDateTime, userId, "Hasan_Karami_Moheb");

        #endregion

        #region User Seeder
        
        const string description = "من حسن کرمی محب ؛ برنامه نویس و عاشق معماری های برنامه نویسی ، 26 ساله ، کشور ایران و اهل شهرستان شهریار هستم";
        
        var newUser = new User(new DomicDateTime(), userId, userId, "SuperAdmin", "Hasan", "Karami Moheb", description, 
            "Hasan_Karami_Moheb", "Hasan313@@313!!", "09026676147", "hasankarami2020313@gmail.com", _roleIds, 
            _permissionIds
        );

        context.Users.Add(newUser);

        #endregion

        #region RoleUser Seeder

        var newRoleUser =
            new RoleUser(new DomicDateTime(), uniqueIdGenerator.GetRandom(6), userId, "SuperAdmin", userId, roleId);

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