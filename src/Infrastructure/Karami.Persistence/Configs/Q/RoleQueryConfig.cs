using Karami.Core.Domain.Enumerations;
using Karami.Domain.Role.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Karami.Persistence.Configs.Q;

public class RoleQueryConfig : IEntityTypeConfiguration<RoleQuery>
{
    public void Configure(EntityTypeBuilder<RoleQuery> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(role => role.Id);
        
        /*-----------------------------------------------------------*/
        
        //Configs
        
        builder.Property(role => role.IsDeleted)
               .HasConversion(new EnumToNumberConverter<IsDeleted, int>());

        /*-----------------------------------------------------------*/
        
        //Relations
        
        builder.HasMany(role => role.RoleUsers)
               .WithOne(roleUser => roleUser.Role)
               .HasForeignKey(roleUser => roleUser.RoleId);
        
        builder.HasMany(role => role.Permissions)
               .WithOne(permissions => permissions.Role)
               .HasForeignKey(permissions => permissions.RoleId);
        
        /*-----------------------------------------------------------*/

        builder.HasQueryFilter(role => role.IsDeleted == IsDeleted.UnDelete);
    }
}