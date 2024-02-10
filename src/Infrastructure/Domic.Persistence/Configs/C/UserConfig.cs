using Domic.Core.Persistence.Configs;
using Domic.Domain.User.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domic.Persistence.Configs.C;

public class UserConfig : BaseEntityConfig<User, string>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        
        /*-----------------------------------------------------------*/
        
        //Configs

        builder.ToTable("Users");

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

        /*-----------------------------------------------------------*/
        
        //Relations

        builder.HasMany(user => user.RoleUsers)
               .WithOne(roleUser => roleUser.User)
               .HasForeignKey(roleUser => roleUser.UserId);
        
        builder.HasMany(user => user.PermissionUsers)
               .WithOne(permissionUser => permissionUser.User)
               .HasForeignKey(permissionUser => permissionUser.UserId);
    }
}