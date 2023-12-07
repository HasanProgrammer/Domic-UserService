using Karami.Domain.Permission.Entities;
using Karami.Domain.PermissionUser.Entities;
using Karami.Domain.Role.Entities;
using Karami.Domain.RoleUser.Entities;
using Karami.Domain.User.Entities;
using Karami.Persistence.Configs.Q;
using Microsoft.EntityFrameworkCore;

namespace Karami.Persistence.Contexts.Q;

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