---
description: 
globs: 
alwaysApply: true
---

## Stack & Technology

- .Net 8
- C#
- EF Core
- Docker

## Global Rules

1 . All methods must be implemented using `async` and `await`

2 . If there are no files in the respective folder, a `.keep` file must be placed inside

3 . The overall architecture of the project follows the structure below :

```
- Core
    - Domic.Common
        - ClassConsts
        - ClassCustoms
        - ClassDelegates
        - ClassExtensions
        - ClassExceptions
        - ClassHelpers
        - ClassWrappers
    - Domic.Domain (entities define in here)
         - Commons
            - Contracts
                - Interfaces
                - Abstracts
            - Entities
            - Enumerations
            - Events
            - Exceptions
            - ValueObjects
    - Domic.UseCase (business logics define in here)
        - Commons
            - Caches
            - Contracts
                - Interfaces
                - Abstracts
            - DTOs
            - Exceptions
            - Extensions
- Infrastructure
    - Domic.Infrastructure (implementation of all domain & usecase contracts and all packages install in here)
        - Extensions
        - Implementations.Domain
            - Repositories
            - Services
        - Implementations.UseCase
            - Services
    - Domic.Persistence (database context and configs)
        - Configs
            - C
            - Q
        - Contexts
            - C
            - Q
        - Migrations
            - C
            - Q
- Presentation
    - Domic.WebAPI (api)
        - EntryPoints
            - GRPCs
            - HTTPs
            - HUbs
        - Frameworks
            - Extensions
            - Filters
            - Middlewares

```

Example :
```
- Core
    - Domic.Common
        - ClassConsts
        - ClassCustom
        - ClassDelegates
        - ClassExtensions
        - ClassExceptions
        - ClassHelpers
        - ClassWrappers
    - Domic.Domain
        - Commons
            - Contracts
                - Interfaces
                - Abstracts
            - Entities
            - Enumerations
            - Events
            - Exceptions
            - ValueObjects
        - Category
            - Contracts
                - Interfaces
                    ICategoryCommandRepository.cs
                - Abstracts
            - Entities
            - Enumerations
            - Events
            - Exceptions
            - ValueObjects
    - Domic.UseCase
        - Commons
            - Caches
            - Contracts
                - Interfaces
                    INotificationService.cs
                - Abstracts
            - DTOs
            - Exceptions
            - Extensions
        - CategoryUseCase
            - Caches
            - Commands
            - Contracts
                - Interfaces
                - Abstracts
            - DTOs
            - Events
            - Exceptions
            - Extensions
            - Queries
- Infrastructure
    - Domic.Infrastructure
        - Extensions
        - Implementations.Domain
            - Repositories
                CategoryCommandRepository.cs
            - Services
        - Implementations.UseCase
            - Services
                NotificationService.cs
    - Domic.Persistence
        - Configs
            - C
                CategoryConfig.cs
            - Q
        - Contexts
            - C
            - Q
        - Migrations
            - C
            - Q
- Presentation
    - Domic.WebAPI
        - Domic.WebAPI (api)
            - EntryPoints
                - GRPCs
                - HTTPs
                    - V1
                        CategoryController.cs
                - HUbs
            - Frameworks
                - Extensions
                - Filters
                - Middlewares
```

## Folder Structure Details
The structure of this layer is as follows :

```
- Configs
    - C
    - Q
- Contexts
    - C
    - Q
- Migrations
    - C
    - Q
```

Example:
```
- Configs
    - C
        CategotyConfig.cs
    - Q
        CategoryQueryConfig.cs
- Contexts
    - C
        SQLContext.cs
        SQLContextFactory.cs
    - Q
        SQLContext.cs
        SQLContextFactory.cs
- Migrations
    - C
    - Q
```

Regarding the above structure, there are some points to note :

1 . All entities created in the `Domain` layer must be defined in the `SQLContext.cs` file

Example :
```csharp
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
    public DbSet<ConsumerEventQuery> ConsumerEvents { get; set; }
    public DbSet<TicketQuery> Tickets { get; set; }
    public DbSet<TicketCommentQuery> TicketComments { get; set; }
    public DbSet<CategoryQuery> Categories { get; set; }
    public DbSet<UserQuery> Users { get; set; }
}

/*Config*/
public partial class SQLContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfiguration(new ConsumerEventQueryConfig());
        builder.ApplyConfiguration(new TicketQueryConfig());
        builder.ApplyConfiguration(new CategoryQueryConfig());
        builder.ApplyConfiguration(new UserQueryConfig());
    }
}
```

2 . To create a `Migration` file, you must follow the instructions in the `Guidance.txt` file

3 . All entity configurations of the `Domain` layer must be placed in the `Configs` folder

Example :
```csharp
//for [Query] entity
public class CategoryQueryConfig : BaseEntityQueryConfig<CategoryQuery, string>
{
    public override void Configure(EntityTypeBuilder<CategoryQuery> builder)
    {
        base.Configure(builder);
        
        //Configs
        
        builder.ToTable("Categories");
        
        //relations
        
        builder.HasMany(category => category.Tickets)
               .WithOne(ticket => ticket.Category)
               .HasForeignKey(ticket => ticket.CategoryId);
    }
}

/*---------------------------------------------------------------*/

//for [Command] entity
public class CategoryConfig : BaseEntityConfig<Category, string>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);
        
        //Configs
        
        builder.ToTable("Categories");
        
        //relations
        
        builder.HasMany(category => category.Tickets)
               .WithOne(ticket => ticket.Category)
               .HasForeignKey(ticket => ticket.CategoryId);
    }
}
```