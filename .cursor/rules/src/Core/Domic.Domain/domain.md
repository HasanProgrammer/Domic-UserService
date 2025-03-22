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
Every entity created in the respective path must have the following folders :

```
- Contracts
    - Interfaces
    - Abstracts
- Entities
- Enumerations
- Events
- Exceptions
- ValueObjects
```

Example :
```
- Category
    - Contracts
        - Interfaces
        - Abstracts
    - Entities
    - Enumerations
    - Events
    - Exceptions
    - ValueObjects
```

## Position of Entities
The location for creating an entity is as follows :

Example :
```
- Category
    - Contracts
        - Interfaces
        - Abstracts
    - Entities
        Category.cs
    - Enumerations
    - Events
    - Exceptions
    - ValueObjects
```

## Implementation of Entity
To implement an entity class, follow the examples below :

1 . If the desired entity is for the `Query` part, it must inherit from the `EntityQuery<string>` class

Example :
```csharp
public class TicketQuery : EntityQuery<string>
{

}
```

2 . If the desired entity is for the `Command` part, it must inherit from the `Entity<string>` class and be implemented as a `Rich Domain Model`

Example :
```csharp
public class Ticket : Entity<string>
{

}
```

When implementing an entity for the `Command` part, consider the following :

1 . The class must have two constructors as shown in the example below :

Example :
```csharp
public class Ticket : Entity<string>
{
    //Fields

    /*---------------------------------------------------------------*/

    //ValueObjects

    /*---------------------------------------------------------------*/

    //Relations (Navigation Properties)

    /*---------------------------------------------------------------*/

    //For EF Core
    private Ticket(){}

    //must have C# comment here
    public Ticket(
        IGlobalUniqueIdGenerator globalUniqueIdGenerator, IDateTime dateTime, 
        IIdentityUser identityUser, ISerializer serializer
    )
    {
        var roles = serializer.Serialize(identityUser.GetRoles());
        var nowDateTime = DateTime.Now;
        var nowPersianDate = dateTime.ToPersianShortDate(nowDateTime);

        Id = globalUniqueIdGenerator.GetRandom(6);

        //audit
        CreatedBy = identityUser.GetIdentity();
        CreatedRole = roles;
        CreatedAt = new CreatedAt(nowDateTime, nowPersianDate);
    }
}
```

According to the above code, the following contracts must be injected into the public constructor :

1. **IGlobalUniqueIdGenerator**
2. **IDateTime**
3. **IIdentityUser**
4. **ISerializer**

2 . If a method is written in the desired entity that edits the entity, it must be implemented as shown in the example below :

Example :
```csharp
public class Ticket : Entity<string>
{
    //Fields

    /*---------------------------------------------------------------*/

    //ValueObjects

    /*---------------------------------------------------------------*/

    //Relations (Navigation Properties)

    /*---------------------------------------------------------------*/

    //For EF Core
    private Ticket(){}

    //must have C# comment here
    public Ticket(
        IGlobalUniqueIdGenerator globalUniqueIdGenerator, IDateTime dateTime, 
        IIdentityUser identityUser, ISerializer serializer
    )
    {
        var roles = serializer.Serialize(identityUser.GetRoles());
        var nowDateTime = DateTime.Now;
        var nowPersianDate = dateTime.ToPersianShortDate(nowDateTime);

        Id = globalUniqueIdGenerator.GetRandom(6);

        //audit
        CreatedBy = identityUser.GetIdentity();
        CreatedRole = roles;
        CreatedAt = new CreatedAt(nowDateTime, nowPersianDate);

        //raise event
        AddEvent(
            new TicketCreated {
                Id = Id,
                CreatedBy = CreatedBy,
                CreatedRole = roles,
                CreatedAt_EnglishDate = nowDateTime,
                CreatedAt_PersianDate = nowPersianDate
            }
        );
    }

    /*---------------------------------------------------------------*/

    //Behaviors

    //must have C# comment here
    public void Change(IDateTime dateTime, ISerializer serializer, IIdentityUser identityUser)
    {
        var roles = serializer.Serialize(identityUser.GetRoles());
        var nowDateTime = DateTime.Now;
        var nowPersianDate = dateTime.ToPersianShortDate(nowDateTime);
        
        //audit
        UpdatedBy = identityUser.GetIdentity();
        UpdatedRole = roles;
        UpdatedAt = new UpdatedAt(nowDateTime, nowPersianDate);
        
        //raise event
        AddEvent(
            new TicketUpdated {
                Id = Id,
                UpdatedRole = roles,
                UpdatedAt_EnglishDate = nowDateTime,
                UpdatedAt_PersianDate = nowPersianDate
            }
        );
    }
}
```

## Position of Events
To create event classes, follow the path below :

Example :
```
- Category
    - Contracts
        - Interfaces
        - Abstracts
    - Entities
    - Enumerations
    - Events
        - TicketCreated.cs
    - Exceptions
    - ValueObjects
```

## Implementation of Event ( for rabbitmq tool )
To implement event classes related to an entity, follow the example below :

Example :
```csharp
[EventConfig(ExchangeType = Exchange.FanOut, Exchange = "Ticket_Ticket_Exchange")]
public class TicketCreated : CreateDomainEvent<string>
{
    public required string CategoryId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required int Status { get; init; }
    public required int Priority { get; init; }
}
```

General rules for defining an event are stated below :

Example :
```csharp
//ExchangeType : Exchange.FanOut | Exchange.Direct | Exchange.Headers | Exchange.Topic

//FanOut-Exchange

//create event
[EventConfig(ExchangeType = Exchange.FanOut, Exchange = "exchange")]
public class TicketCreated : CreateDomainEvent<string> //any type of identity key
{
    //payload
}

//update event
[EventConfig(ExchangeType = Exchange.FanOut, Exchange = "exchange")]
public class TicketUpdated : UpdateDomainEvent<string> //any type of identity key
{
    //payload
}

//delete event
[EventConfig(ExchangeType = Exchange.FanOut, Exchange = "exchange")]
public class TicketDeleted : DeleteDomainEvent<string> //any type of identity key
{
    //payload
}

/*---------------------------------------------------------------*/

//ExchangeType : Exchange.FanOut | Exchange.Direct | Exchange.Headers | Exchange.Topic

//Direct-Exchange

//create event
[EventConfig(ExchangeType = Exchange.Direct, Exchange = "exchange", Route = "route")]
public class TicketCreated : CreateDomainEvent<string> //any type of identity key
{
    //payload
}

//update event
[EventConfig(ExchangeType = Exchange.Direct, Exchange = "exchange", Route = "route")]
public class TicketUpdated : UpdateDomainEvent<string> //any type of identity key
{
    //payload
}

//delete event
[EventConfig(ExchangeType = Exchange.Direct, Exchange = "exchange", Route = "route")]
public class TicketDeleted : DeleteDomainEvent<string> //any type of identity key
{
    //payload
}
```

If the service mentioned above, in addition to producing these events ( Producer ), is also a consumer of these events ( Consumer ), it must be implemented as follows :

Example :
```csharp
//FanOut-Exchange

//create event
[EventConfig(ExchangeType = Exchange.FanOut, Exchange = "exchange", Queue = "queue")]
public class TicketCreated : CreateDomainEvent<string> //any type of identity key
{
    //payload
}

//update event
[EventConfig(ExchangeType = Exchange.FanOut, Exchange = "exchange", Queue = "queue")]
public class TicketUpdated : UpdateDomainEvent<string> //any type of identity key
{
    //payload
}

//delete event
[EventConfig(ExchangeType = Exchange.FanOut, Exchange = "exchange", Queue = "queue")]
public class TicketDeleted : DeleteDomainEvent<string> //any type of identity key
{
    //payload
}

/*---------------------------------------------------------------*/

//Direct-Exchange

//create event
[EventConfig(ExchangeType = Exchange.Direct, Exchange = "exchange", Route = "route", Queue = "queue")]
public class TicketCreated : CreateDomainEvent<string> //any type of identity key
{
    //payload
}

//update event
[EventConfig(ExchangeType = Exchange.Direct, Exchange = "exchange", Route = "route", Queue = "queue")]
public class TicketUpdated : UpdateDomainEvent<string> //any type of identity key
{
    //payload
}

//delete event
[EventConfig(ExchangeType = Exchange.Direct, Exchange = "exchange", Route = "route", Queue = "queue")]
public class TicketDeleted : DeleteDomainEvent<string> //any type of identity key
{
    //payload
}
```

## Implementation of Event ( for kafka tool )
To implement event classes related to an entity, follow the example below :

Example :
```csharp
[EventConfig(Topic = "Ticket")]
public class TicketCreated : CreateDomainEvent<string>
{
    public required string CategoryId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required int Status { get; init; }
    public required int Priority { get; init; }
}
```

General rules for defining an event are stated below :

Example :
```csharp
//create event
[EventConfig(Topic = "Topic")]
public class Created : CreateDomainEvent<string> //any type of identity key
{
    //payload
}

//update event
[EventConfig(Topic = "Topic")]
public class Updated : UpdateDomainEvent<string> //any type of identity key
{
    //payload
}

//delete event
[EventConfig(Topic = "Topic")]
public class Deleted : DeleteDomainEvent<string> //any type of identity key
{
    //payload
}
```

## Position of Contracts ( like repository per entity )
To create contracts that an entity needs, follow the example and specified path below :

Example :
```
- Category
    - Contracts
        - Interfaces
            - ICategoryCommandRepository.cs
        - Abstracts
    - Entities
    - Enumerations
    - Events
    - Exceptions
    - ValueObjects
```

## Implementation of Repository Contract
To implement the repository interface for the respective entity, follow the examples below :

**Example 1 :**
```csharp
public interface ICategoryCommandRepository : ICommandRepository<Category, string>
{
    //custom contracts
}
```

**Example 2 :**
```csharp
public interface ICategoryQueryRepository : IQueryRepository<CategoryQuery, string>
{
    //custom contracts
}
```