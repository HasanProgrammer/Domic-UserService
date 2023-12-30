using Karami.Core.Persistence.Configs;
using Karami.Domain.PermissionUser.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Karami.Persistence.Configs.C;

public class PermissionUserConfig : BaseEntityConfig<PermissionUser, string>
{
    public override void Configure(EntityTypeBuilder<PermissionUser> builder)
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