using Domic.Core.Persistence.Configs;
using Domic.Domain.RoleUser.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domic.Persistence.Configs.C;

public class RoleUserConfig : BaseEntityConfig<RoleUser, string>
{
    public override void Configure(EntityTypeBuilder<RoleUser> builder)
    {
        base.Configure(builder);

        /*-----------------------------------------------------------*/
        
        //Configs
        
        builder.ToTable("RoleUsers");
        
        /*-----------------------------------------------------------*/
        
        //Relations
        
        builder.HasOne(roleUser => roleUser.User)
               .WithMany(user => user.RoleUsers)
               .HasForeignKey(roleUser => roleUser.UserId);
        
        builder.HasOne(roleUser => roleUser.Role)
               .WithMany(role => role.RoleUsers)
               .HasForeignKey(roleUser => roleUser.RoleId);
    }
}