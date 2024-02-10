using Domic.Core.Persistence.Configs;
using Domic.Domain.Role.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domic.Persistence.Configs.Q;

public class RoleQueryConfig : BaseEntityQueryConfig<RoleQuery, string>
{
    public override void Configure(EntityTypeBuilder<RoleQuery> builder)
    {
        base.Configure(builder);

        /*-----------------------------------------------------------*/
        
        //Configs
        
        builder.ToTable("Roles");

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