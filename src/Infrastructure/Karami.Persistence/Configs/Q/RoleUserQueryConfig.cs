using Karami.Core.Domain.Enumerations;
using Karami.Domain.RoleUser.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Karami.Persistence.Configs.Q;

public class RoleUserQueryConfig : IEntityTypeConfiguration<RoleUserQuery>
{
    public void Configure(EntityTypeBuilder<RoleUserQuery> builder)
    {
        builder.ToTable("RoleUsers");
        
        builder.HasKey(roleUser => roleUser.Id);
        
        /*-----------------------------------------------------------*/
        
        //Configs
        
        builder.Property(roleUser => roleUser.IsDeleted)
               .HasConversion(new EnumToNumberConverter<IsDeleted, int>());
        
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