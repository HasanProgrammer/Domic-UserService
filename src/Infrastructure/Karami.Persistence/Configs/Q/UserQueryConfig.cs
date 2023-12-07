using Karami.Core.Domain.Enumerations;
using Karami.Domain.User.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Karami.Persistence.Configs.Q;

public class UserQueryConfig : IEntityTypeConfiguration<UserQuery>
{
    public void Configure(EntityTypeBuilder<UserQuery> builder)
    {
        builder.HasKey(user => user.Id);

        builder.ToTable("Users");
        
        /*-----------------------------------------------------------*/

        builder.Property(user => user.IsActive).HasConversion(new EnumToNumberConverter<IsActive , int>());
        
        builder.Property(user => user.IsDeleted).HasConversion(new EnumToNumberConverter<IsDeleted, int>());

        /*-----------------------------------------------------------*/
        
        //Relations

        builder.HasMany(user => user.RoleUsers)
               .WithOne(roleUser => roleUser.User)
               .HasForeignKey(roleUser => roleUser.UserId);
        
        builder.HasMany(user => user.PermissionUsers)
               .WithOne(permissionUser => permissionUser.User)
               .HasForeignKey(permissionUser => permissionUser.UserId);
        
        /*-----------------------------------------------------------*/

        builder.HasQueryFilter(user => user.IsDeleted == IsDeleted.UnDelete);
    }
}