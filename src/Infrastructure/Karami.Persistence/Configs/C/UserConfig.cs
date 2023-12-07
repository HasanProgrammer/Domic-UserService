using Karami.Core.Domain.Enumerations;
using Karami.Domain.User.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Karami.Persistence.Configs.C;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);

        builder.ToTable("Users");
        
        /*-----------------------------------------------------------*/
        
        //Configs
        
        builder.OwnsOne(user => user.FirstName)
               .Property(firstName => firstName.Value)
               .IsRequired()
               .HasMaxLength(50)
               .HasColumnName("FirstName");
        
        builder.OwnsOne(user => user.LastName)
               .Property(lastName => lastName.Value)
               .IsRequired()
               .HasMaxLength(80)
               .HasColumnName("LastName");
        
        builder.OwnsOne(user => user.Username)
               .Property(username => username.Value)
               .IsRequired()
               .HasMaxLength(30)
               .HasColumnName("Username");
        
        builder.OwnsOne(user => user.Password)
               .Property(password => password.Value)
               .IsRequired()
               .HasColumnName("Password");
        
        builder.OwnsOne(user => user.Description)
               .Property(description => description.Value)
               .IsRequired()
               .HasMaxLength(500)
               .HasColumnName("Description");
        
        builder.OwnsOne(user => user.PhoneNumber)
               .Property(phoneNumber => phoneNumber.Value)
               .IsRequired()
               .HasMaxLength(11)
               .HasColumnName("PhoneNumber");
        
        builder.OwnsOne(user => user.Email)
               .Property(email => email.Value)
               .IsRequired()
               .HasMaxLength(30)
               .HasColumnName("Email");
        
        builder.OwnsOne(user => user.CreatedAt, createdAt => {
               createdAt.Property(vo => vo.EnglishDate).IsRequired().HasColumnName("CreatedAt_EnglishDate");
               createdAt.Property(vo => vo.PersianDate).IsRequired().HasColumnName("CreatedAt_PersianDate");
        });
        
        builder.OwnsOne(user => user.UpdatedAt, updatedAt => {
               updatedAt.Property(vo => vo.EnglishDate).IsRequired().HasColumnName("UpdatedAt_EnglishDate");
               updatedAt.Property(vo => vo.PersianDate).IsRequired().HasColumnName("UpdatedAt_PersianDate");
        });

        builder.Property(user => user.IsActive)
               .HasConversion(new EnumToNumberConverter<IsActive , int>())
               .IsRequired();
        
        builder.Property(user => user.IsDeleted)
               .HasConversion(new EnumToNumberConverter<IsDeleted , int>())
               .IsRequired();

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