using Karami.Core.Domain.Enumerations;
using Karami.Domain.Role.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Karami.Persistence.Configs.C;

public class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(role => role.Id);
        
        /*-----------------------------------------------------------*/
        
        //Configs

        builder.OwnsOne(role => role.Name)
               .Property(name => name.Value)
               .IsRequired()
               .HasMaxLength(100)
               .HasColumnName("Name");
        
        builder.OwnsOne(role => role.CreatedAt, createdAt => {
               createdAt.Property(vo => vo.EnglishDate).IsRequired().HasColumnName("CreatedAt_EnglishDate");
               createdAt.Property(vo => vo.PersianDate).IsRequired().HasColumnName("CreatedAt_PersianDate");
        });
        
        builder.OwnsOne(role => role.UpdatedAt, createdAt => {
               createdAt.Property(vo => vo.EnglishDate).IsRequired().HasColumnName("UpdatedAt_EnglishDate");
               createdAt.Property(vo => vo.PersianDate).IsRequired().HasColumnName("UpdatedAt_PersianDate");
        });

        builder.Property(role => role.IsActive)
               .HasConversion(new EnumToNumberConverter<IsActive, int>())
               .IsRequired();
        
        builder.Property(role => role.IsDeleted)
               .HasConversion(new EnumToNumberConverter<IsDeleted, int>())
               .IsRequired();
        
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