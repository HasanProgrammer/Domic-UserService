using Karami.Core.Domain.Enumerations;
using Karami.Domain.RoleUser.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Karami.Persistence.Configs.C;

public class RoleUserConfig : IEntityTypeConfiguration<RoleUser>
{
    public void Configure(EntityTypeBuilder<RoleUser> builder)
    {
        builder.ToTable("RoleUsers");
        
        builder.HasKey(roleUser => roleUser.Id);
        
        /*-----------------------------------------------------------*/
        
        builder.OwnsOne(roleUser => roleUser.CreatedAt, createdAt => {
               createdAt.Property(vo => vo.EnglishDate).IsRequired().HasColumnName("CreatedAt_EnglishDate");
               createdAt.Property(vo => vo.PersianDate).IsRequired().HasColumnName("CreatedAt_PersianDate");
        });
        
        builder.OwnsOne(roleUser => roleUser.UpdatedAt, updatedAt => {
               updatedAt.Property(vo => vo.EnglishDate).IsRequired().HasColumnName("UpdatedAt_EnglishDate");
               updatedAt.Property(vo => vo.PersianDate).IsRequired().HasColumnName("UpdatedAt_PersianDate");
        });
        
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