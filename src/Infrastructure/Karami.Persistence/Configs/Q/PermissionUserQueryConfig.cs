using Karami.Core.Persistence.Configs;
using Karami.Domain.PermissionUser.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Karami.Persistence.Configs.Q;

public class PermissionUserQueryConfig : BaseEntityQueryConfig<PermissionUserQuery, string>
{
    public override void Configure(EntityTypeBuilder<PermissionUserQuery> builder)
    {
        base.Configure(builder);
        
        /*-----------------------------------------------------------*/
        
        //Configs
        
        builder.ToTable("PermissionUsers");

        /*-----------------------------------------------------------*/
        
        //Relations
        
        builder.HasOne(permissionUser => permissionUser.Permission)
               .WithMany(permission => permission.PermissionUsers)
               .HasForeignKey(permissionUser => permissionUser.PermissionId);
        
        builder.HasOne(permissionUser => permissionUser.User)
               .WithMany(user => user.PermissionUsers)
               .HasForeignKey(permissionUser => permissionUser.UserId);
    }
}