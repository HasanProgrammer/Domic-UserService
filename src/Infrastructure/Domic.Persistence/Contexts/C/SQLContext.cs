using Domic.Core.Domain.Entities;
using Domic.Core.Persistence.Configs;
using Domic.Domain.Permission.Entities;
using Domic.Domain.PermissionUser.Entities;
using Domic.Domain.Role.Entities;
using Domic.Domain.RoleUser.Entities;
using Domic.Domain.User.Entities;
using Domic.Persistence.Configs.C;
using Microsoft.EntityFrameworkCore;

namespace Domic.Persistence.Contexts.C;

/*Setting*/
public partial class SQLContext : DbContext
{
    public SQLContext(DbContextOptions<SQLContext> options) : base(options)
    {
        
    }
}

/*Entity*/
public partial class SQLContext
{
    public DbSet<User> Users                     { get; set; }
    public DbSet<RoleUser> RoleUsers             { get; set; }
    public DbSet<Role> Roles                     { get; set; }
    public DbSet<Permission> Permissions         { get; set; }
    public DbSet<PermissionUser> PermissionUsers { get; set; }
    public DbSet<Event> Events                   { get; set; }
}

/*Config*/
public partial class SQLContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new UserConfig());
        builder.ApplyConfiguration(new RoleUserConfig());
        builder.ApplyConfiguration(new RoleConfig());
        builder.ApplyConfiguration(new PermissionConfig());
        builder.ApplyConfiguration(new PermissionUserConfig());
        builder.ApplyConfiguration(new EventConfig());
    }
}