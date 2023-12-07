using Karami.Core.Domain.Enumerations;
using Karami.Domain.Permission.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Karami.Persistence.Configs.C;

public class PermissionConfig : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(permission => permission.Id);

        builder.ToTable("Permissions");
        
        /*-----------------------------------------------------------*/
        
        //Configs

        builder.OwnsOne(permission => permission.Name)
               .Property(name => name.Value)
               .IsRequired()
               .HasMaxLength(100)
               .HasColumnName("Name");

        builder.OwnsOne(permission => permission.CreatedAt, createdAt => {
               createdAt.Property(vo => vo.EnglishDate).IsRequired().HasColumnName("CreatedAt_EnglishDate");
               createdAt.Property(vo => vo.PersianDate).IsRequired().HasColumnName("CreatedAt_PersianDate");
        });
        
        builder.OwnsOne(permission => permission.UpdatedAt, createdAt => {
               createdAt.Property(vo => vo.EnglishDate).IsRequired().HasColumnName("UpdatedAt_EnglishDate");
               createdAt.Property(vo => vo.PersianDate).IsRequired().HasColumnName("UpdatedAt_PersianDate");
        });
        
        builder.Property(permission => permission.IsActive).HasConversion(new EnumToNumberConverter<IsActive, int>());
        
        builder.Property(permission => permission.IsDeleted).HasConversion(new EnumToNumberConverter<IsDeleted, int>());

        /*-----------------------------------------------------------*/
        
        //Relations

        builder.HasOne(permission => permission.Role)
               .WithMany(role => role.Permissions)
               .HasForeignKey(permission => permission.RoleId);

        builder.HasMany(permission => permission.PermissionUsers)
               .WithOne(permissionUser => permissionUser.Permission)
               .HasForeignKey(permissionUser => permissionUser.PermissionId);
        
        /*-----------------------------------------------------------*/

        builder.HasQueryFilter(permission => permission.IsDeleted == IsDeleted.UnDelete);
    }
}