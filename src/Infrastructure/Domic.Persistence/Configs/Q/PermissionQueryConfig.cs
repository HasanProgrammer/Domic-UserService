using Domic.Core.Persistence.Configs;
using Domic.Domain.Permission.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domic.Persistence.Configs.Q;

public class PermissionQueryConfig : BaseEntityQueryConfig<PermissionQuery, string>
{
    public override void Configure(EntityTypeBuilder<PermissionQuery> builder)
    {
        base.Configure(builder);

        /*-----------------------------------------------------------*/

        //Configs
        
        builder.ToTable("Permissions");

        /*-----------------------------------------------------------*/

        //Relations
        
        builder.HasOne(permission => permission.Role)
               .WithMany(role => role.Permissions)
               .HasForeignKey(permission => permission.RoleId);

        builder.HasMany(permission => permission.PermissionUsers)
               .WithOne(permissionUser => permissionUser.Permission)
               .HasForeignKey(permissionUser => permissionUser.PermissionId);
    }
}