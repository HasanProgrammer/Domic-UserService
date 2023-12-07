using Karami.Core.Domain.Enumerations;
using Karami.Domain.Permission.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Karami.Persistence.Configs.Q;

public class PermissionQueryConfig : IEntityTypeConfiguration<PermissionQuery>
{
    public void Configure(EntityTypeBuilder<PermissionQuery> builder)
    {
        builder.HasKey(permission => permission.Id);

        builder.ToTable("Permissions");
        
        /*-----------------------------------------------------------*/

        //Configs
        
        builder.Property(permission => permission.IsDeleted).HasConversion(new EnumToNumberConverter<IsDeleted, int>());

        /*-----------------------------------------------------------*/

        builder.HasOne(permission => permission.Role)
               .WithMany(role => role.Permissions)
               .HasForeignKey(permission => permission.RoleId);

        builder.HasMany(permission => permission.PermissionUsers)
               .WithOne(permissionUser => permissionUser.Permission)
               .HasForeignKey(permissionUser => permissionUser.PermissionId);
        
        /*-----------------------------------------------------------*/

        builder.HasQueryFilter(query => query.IsDeleted == IsDeleted.UnDelete);
    }
}