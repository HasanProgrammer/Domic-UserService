using Karami.Core.Domain.Implementations;
using Karami.Domain.Permission.Entities;
using Karami.Domain.PermissionUser.Entities;
using Karami.Domain.Role.Entities;
using Karami.Domain.RoleUser.Entities;
using Karami.Domain.User.Entities;
using Karami.Persistence.Contexts.C;

namespace Karami.Infrastructure.Extensions.C;

public static class SQLContextExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public static void Seed(this SQLContext context)
    {
        #region Role Seeder

        const string roleId = "4b3a6d73-6822-48b3-a8fd-3c0c38d1841c";
        var newRole = new Role(new DotrisDateTime(), roleId, "SuperAdmin");

        context.Roles.Add(newRole);

        #endregion

        #region Permission Seeder

        _permissionsBuilder(context, roleId);

        #endregion

        #region User Seeder
        
        const string userId      = "66a16cff-449d-4b3e-b8b0-b46a1cd2df44";
        const string description = "من حسن کرمی محب ؛ برنامه نویس و عاشق معماری های برنامه نویسی ، 26 ساله ، کشور ایران و اهل شهرستان شهریار هستم";
        
        var newUser = new User(new DotrisDateTime(), userId, "Hasan", "Karami Moheb", description, "Hasan_Karami_Moheb",
            "Hasan313@@313!!", "09026676147", "hasankarami2020313@gmail.com"
        );

        context.Users.Add(newUser);

        #endregion

        #region RoleUser Seeder

        const string roleUserId = "003439d8-90e2-4bdd-9176-49a13d665df5";
        var newRoleUser = new RoleUser(new DotrisDateTime(), roleUserId, userId, roleId);

        context.RoleUsers.Add(newRoleUser);

        #endregion
        
        #region PermissionUser Seeder

        _permissionsUsersBuilder(context, userId);

        #endregion

        context.SaveChanges();
    }

    //37
    private static List<string> _permissionsId() => new() {
        "f7fb0317-dd57-4868-9c8e-4e9b25bd2724" , //1
        "0562683f-4106-4247-b0a4-6129646ff7e4" , //2
        "53582fcb-140d-4f5b-9336-a32d3989189b" , //3
        "d90bff3f-8f95-41ba-a833-71b19b8c5c3c" , //4
        "0fb7b925-0a6b-4ac6-804d-457916f80f04" , //5
        "280b1e46-3a7b-437e-a958-4bbb7b940be3" , //6
        "bb308a72-60ad-412f-811c-6f21c1cfa91b" , //7
        "2c766767-9a1d-401b-9c8e-1ffa485c4de3" , //8
        "b6ebf47b-fdcc-473a-ab62-1f8ee761c85b" , //9
        "76ca827a-39ca-4f9a-a5f1-5afc36de30c3" , //10
        "dae7c39e-3a49-4b8d-b45e-b30cca8dc450" , //11
        "b4d2cdc6-8ac6-4a1a-a8be-105fcdc19f18" , //12
        "da86fe00-72e4-44a4-9a1d-0c64c325655f" , //13
        "4a1fd696-6375-40c0-ad81-a23c79b43c49" , //14
        "479af2ad-8fa4-447f-bfc7-c0a79605513b" , //15
        "7de716a3-9ae7-4aa0-93d4-59a9be8b43a5" , //16
        "e6fe7b38-d917-4355-9f2f-71ed5ed89745" , //17
        "468aac39-f433-4135-a2c2-72dc790a58fb" , //18
        "504c4714-ec0b-4398-a3dd-f65820743983" , //19
        "908c76a1-052b-4970-98ef-1d00de66b295" , //20
        "1d013cb0-1aa1-479d-8f21-36027d6b7fbf" , //21
        "1b7a8e67-bb29-4eb1-9843-2b4eb3d1915e" , //22
        "fc2a66fa-ce64-419a-af80-5c03ef6ec405" , //23
        "d069b778-d494-4ace-83d6-c77eae43e1bc" , //24
        "2bb4a1dd-04c3-4abd-954e-5a8aa602bf1f" , //25
        "65900ce3-6114-4b17-a084-25fceaec4f86" , //26
        "e04dadbd-7f5c-4794-9d97-0e79b38db8ce" , //27
        "9b70fbf2-681c-40a0-a197-351764e5b33a" , //28
        "ac25c8eb-150b-456e-9e20-0f734e968230" , //29
        "09bc7a4e-f875-4185-819a-1cfa49aafd6d" , //30
        "12e582e9-1ba9-426f-9812-6627143a1c9f" , //31
        "7774015f-4fea-47a7-95c6-fe564d406897" , //32
        "d40c4629-9c58-4fa9-8a02-041d0e8ead03" , //33
        "5549e80f-0aff-4d60-90fe-a2dd5d959cb4" , //34
        "59d14cc1-7302-4cb3-a2e7-76b110c6ecdb" , //35
        "ba28734a-e75c-4904-bd00-8ab43e371b11" , //36
        "b06b537f-c048-4703-8874-0c5955662704" , //37
    };

    private static void _permissionsBuilder(SQLContext context, string roleId)
    {
        var permissions = Core.Common.ClassConsts.Permission.GetAll();
        
        for (int i = 0; i < permissions.Count; i++)
        {
            var newPermission = new Permission(new DotrisDateTime(), _permissionsId()[i],
                permissions[i], roleId
            );

            context.Permissions.Add(newPermission);
        }
    }

    private static void _permissionsUsersBuilder(SQLContext context, string userId)
    {
        for (int i = 0; i < _permissionsId().Count; i++)
        {
            var newPermissionUser =
                new PermissionUser(new DotrisDateTime(), Guid.NewGuid().ToString(), userId, _permissionsId()[i]);

            context.PermissionUsers.Add(newPermissionUser);
        }
    }
}