using Karami.Core.Persistence.Configs;
using Karami.Domain.RoleUser.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Karami.Persistence.Configs.Q;

public class RoleUserQueryConfig : BaseEntityQueryConfig<RoleUserQuery, string>
{
    public override void Configure(EntityTypeBuilder<RoleUserQuery> builder)
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