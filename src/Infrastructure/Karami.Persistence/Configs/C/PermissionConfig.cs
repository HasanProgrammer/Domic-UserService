using Karami.Core.Persistence.Configs;
using Karami.Domain.Permission.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Karami.Persistence.Configs.C;

public class PermissionConfig : BaseEntityConfig<Permission, string>
{
    public override void Configure(EntityTypeBuilder<Permission> builder)
    {
        base.Configure(builder);
        
        /*-----------------------------------------------------------*/
        
        //Configs
        
        builder.ToTable("Permissions");
        
        builder.OwnsOne(permission => permission.Name)
               .Property(name => name.Value)
               .IsRequired()
               .HasMaxLength(100)
               .HasColumnName("Name");

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