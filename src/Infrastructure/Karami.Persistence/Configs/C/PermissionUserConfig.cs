using Karami.Core.Domain.Enumerations;
using Karami.Domain.PermissionUser.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Karami.Persistence.Configs.C;

public class PermissionUserConfig : IEntityTypeConfiguration<PermissionUser>
{
    public void Configure(EntityTypeBuilder<PermissionUser> builder)
    {
        builder.ToTable("PermissionUsers");
        
        builder.HasKey(permissionUser => permissionUser.Id);
        
        /*-----------------------------------------------------------*/
        
        //Configs
        
        builder.OwnsOne(permissionUser => permissionUser.CreatedAt, createdAt => {
               createdAt.Property(vo => vo.EnglishDate).IsRequired().HasColumnName("CreatedAt_EnglishDate");
               createdAt.Property(vo => vo.PersianDate).IsRequired().HasColumnName("CreatedAt_PersianDate");
        });
        
        builder.OwnsOne(permissionUser => permissionUser.UpdatedAt, updatedAt => {
               updatedAt.Property(vo => vo.EnglishDate).IsRequired().HasColumnName("UpdatedAt_EnglishDate");
               updatedAt.Property(vo => vo.PersianDate).IsRequired().HasColumnName("UpdatedAt_PersianDate");
        });
        
        builder.Property(permissionUser => permissionUser.IsDeleted)
               .HasConversion(new EnumToNumberConverter<IsDeleted, int>());
        
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