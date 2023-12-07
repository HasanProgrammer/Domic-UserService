using Karami.Core.Domain.Entities;
using Karami.Core.Persistence.Configs;
using Karami.Domain.Permission.Entities;
using Karami.Domain.PermissionUser.Entities;
using Karami.Domain.Role.Entities;
using Karami.Domain.RoleUser.Entities;
using Karami.Domain.User.Entities;
using Karami.Persistence.Configs.C;
using Microsoft.EntityFrameworkCore;

namespace Karami.Persistence.Contexts.C;

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