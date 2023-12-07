using Karami.Core.Domain.Enumerations;
using Karami.Domain.PermissionUser.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Karami.Persistence.Configs.Q;

public class PermissionUserQueryConfig : IEntityTypeConfiguration<PermissionUserQuery>
{
    public void Configure(EntityTypeBuilder<PermissionUserQuery> builder)
    {
        builder.ToTable("PermissionUsers");
        
        builder.HasKey(permissionUser => permissionUser.Id);
        
        /*-----------------------------------------------------------*/
        
        //Configs
        
        builder.Property(permissionUser => permissionUser.IsDeleted)
               .HasConversion(new EnumToNumberConverter<IsDeleted, int>());
        
        /*-----------------------------------------------------------*/
        
        builder.HasOne(permissionUser => permissionUser.Permission)
               .WithMany(permission => permission.PermissionUsers)
               .HasForeignKey(permissionUser => permissionUser.PermissionId);
        
        builder.HasOne(permissionUser => permissionUser.User)
               .WithMany(user => user.PermissionUsers)
               .HasForeignKey(permissionUser => permissionUser.UserId);
    }
}