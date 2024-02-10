using Domic.Domain.Permission.Entities;
using Domic.Domain.PermissionUser.Entities;
using Domic.Domain.Role.Entities;
using Domic.Domain.RoleUser.Entities;
using Domic.Domain.User.Entities;
using Domic.Persistence.Configs.Q;
using Microsoft.EntityFrameworkCore;

namespace Domic.Persistence.Contexts.Q;

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
    public DbSet<UserQuery> Users                     { get; set; }
    public DbSet<RoleUserQuery> RoleUsers             { get; set; }
    public DbSet<RoleQuery> Roles                     { get; set; }
    public DbSet<PermissionQuery> Permissions         { get; set; }
    public DbSet<PermissionUserQuery> PermissionUsers { get; set; }
}

/*Config*/
public partial class SQLContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new UserQueryConfig());
        builder.ApplyConfiguration(new RoleUserQueryConfig());
        builder.ApplyConfiguration(new RoleQueryConfig());
        builder.ApplyConfiguration(new PermissionQueryConfig());
        builder.ApplyConfiguration(new PermissionUserQueryConfig());
    }
}