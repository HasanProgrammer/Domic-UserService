using Karami.Core.Persistence.Configs;
using Karami.Domain.Role.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Karami.Persistence.Configs.C;

public class RoleConfig : BaseEntityConfig<Role, string>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);

        /*-----------------------------------------------------------*/
        
        //Configs
        
        builder.ToTable("Roles");

        builder.OwnsOne(role => role.Name)
               .Property(name => name.Value)
               .IsRequired()
               .HasMaxLength(100)
               .HasColumnName("Name");
        
        /*-----------------------------------------------------------*/
        
        //Relations
        
        builder.HasMany(role => role.RoleUsers)
               .WithOne(roleUser => roleUser.Role)
               .HasForeignKey(roleUser => roleUser.RoleId);
        
        builder.HasMany(role => role.Permissions)
               .WithOne(permissions => permissions.Role)
               .HasForeignKey(permissions => permissions.RoleId);
    }
}