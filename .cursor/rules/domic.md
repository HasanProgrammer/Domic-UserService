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
- src
    - Core
        - Domic.Common
            - ClassConsts
            - ClassCustoms
            - ClassDelegates
            - ClassExtensions
            - ClassExceptions
            - ClassHelpers
            - ClassWrappers
        - Domic.Domain ( entities define in here )
            - Commons
                - Contracts
                    - Interfaces
                    - Abstracts
                - Entities
                - Enumerations
                - Events
                - Exceptions
                - ValueObjects
        - Domic.UseCase ( business logics define in here )
            - Commons
                - Caches
                - Contracts
                    - Interfaces
                    - Abstracts
                - DTOs
                - Exceptions
                - Extensions
    - Infrastructure
        - Domic.Infrastructure ( implementation of all domain & usecase contracts and all packages install in here )
            - Extensions
            - Implementations.Domain
                - Repositories
                    - C
                    - Q
                - Services
            - Implementations.UseCase
                - Services
        - Domic.Persistence ( database context and configs )
            - Configs
                - C
                - Q
            - Contexts
                - C
                - Q
            - Migrations
                - C ( any migration of database for command side )
                - Q ( any migration of database for query side )
    - Presentation
        - Domic.WebAPI ( api )
            - DTOs
            - EntryPoints
                - GRPCs ( google RPC call )
                - HTTPs ( REST api )
                - HUbs
            - Frameworks
                - Extensions
                - Filters
                - Middlewares
- test
    - E2ETests ( load test with NBomber package )
    - IntegrationTests
        - Infrastructure ( mocking with NSubstitute package )
        - Presentation ( mocking with NSubstitute package )
    - UnitTests
        - Core ( mocking with NSubstitute package )
        - Infrastructure ( mocking with NSubstitute package )
        - Presentation ( mocking with NSubstitute package )
```

Example :
```
- src
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
                    - C
                        CategoryCommandRepository.cs
                    - Q
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
            - DTOs
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
- test
    - E2ETests
    - IntegrationTests
        - Infrastructure
        - Presentation
    - UnitTests
        - Core
        - Infrastructure
        - Presentation
```

4 . Be sure to apply the changes to the following path in the migration file when you make changes to the entities in the Domain layer

Example :
```
- src
    - Core
    - Infrastructure
        - Domic.Infrastructure
            - Extensions
            - Implementations.Domain
                - Repositories
                    - C
                    - Q
                - Services
            - Implementations.UseCase
                - Services
        - Domic.Persistence
            - Configs
                - C
                - Q
            - Contexts
                - C
                - Q
            - Migrations
                - C ( any migration of database for command side - execute ef migration command base on Guidance.txt file )
                - Q ( any migration of database for query side - execute ef migration command base on Guidance.txt file )
                Guidance.txt
    - Presentation
- test
    - E2ETests
    - IntegrationTests
    - UnitTests
```

5 . Be sure to implement different tests for each business you implement according to the architecture mentioned in the instructions above; that is, E2E, Integration, and UnitTest tests for different layers of the project

6 . Make sure that any methods you create in `Repositories` or various service contracts in the inner `Core` layers are implemented in the concretes of those contracts and in the `Infrastructure` layer

## Domain Folder Structure Details
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

## UseCase Folder Structure Details
For each entity in the `Domain` layer, a folder must be created with the following structure :

```
- Caches
- Commands
- Contracts
- DTOs
- Events
- Exceptions
- Extensions
- Queries
```

Example :
```
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
```

## Position of Caches
The location for creating a `Cache` manager is as follows :

Example :
```
- CategoryUseCase
    - Caches
        AllCategoryInternalDistributedCache.cs
    - Commands
    - Contracts
        - Interfaces
        - Abstracts
    - DTOs
    - Events
    - Exceptions
    - Extensions
    - Queries
```

## Implementation of Cache
To implement a `Cache` manager class, follow the examples below :

Example :
```csharp
//for current service distributed cache
public class AllCategoryInternalDistributedCache : IInternalDistributedCacheHandler<List<Dto>>
{
    public AllCategoryInternalDistributedCache(){}

    [Config(Key = 'Key', Ttl = 60 /*time to live based on minute*/)]
    public List<Dto> Set()
    {
        //query
        
        return new();
    }
    
    //must be used
    [Config(Key = 'Key', Ttl = 60 /*time to live based on minute*/)]
    public Task<List<Dto>> SetAsync(CancellationToken cancellationToken)
    {
        //query
        
        return Task.FromResult(new());
    }
}

/*---------------------------------------------------------------*/

//for all services distributed cache (global | shared cache)
public class AllCategoryExternalDistributedCache : IExternalDistributedCacheHandler<List<Dto>>
{
    public AllCategoryExternalDistributedCache(){}

    [Config(Key = 'Key', Ttl = 60 /*time to live based on minute*/)]
    public List<Dto> Set()
    {
        //query
        
        return new();
    }
    
    //must be used
    [Config(Key = 'Key', Ttl = 60 /*time to live based on minute*/)]
    public Task<List<Dto>> SetAsync(CancellationToken cancellationToken)
    {
        //query
        
        return Task.FromResult(new());
    }
}
```

In the implementation of `Cache` according to the above instructions, there are a few points to note :

1 . If you do not set a value for `Ttl` in the `ConfigAttribute` above, or set this `Property` to 0, the corresponding `Cache` will remain permanently and without expiration in `Redis`

2 . To use the cached value (according to the above instructions), you must use the interface corresponding to `InternalCache` or `ExternalCache`. For this purpose, two interfaces `IInternalDistributedCacheMediator` and `IExternalDistributedCacheMediator` have been implemented, which can be used as follows :

Example :
```csharp
public class Query : IQuery<List<Dto>>
{
}

public class QueryHandler : IQueryHandler<Query, List<Dto>>
{
    private readonly IInternalDistributedCacheMediator _cacheMediator;

    public QueryHandler(IInternalDistributedCacheMediator cacheMediator) => _cacheMediator = cacheMediator;

    public List<Dto> Handle(Query query)
    {
        var result = _cacheMediator.Get<List<Dto>>(cancellationToken);

        return result;
    }
    
    //must be used
    public async Task<List<Dto>> HandleAsync(Query query, CancellationToken cancellationToken)
    {
        var result = await _cacheMediator.GetAsync<List<Dto>>(cancellationToken);

        return result;
    }
}
```

## Position of Commands
The location for creating a `Command` manager is as follows :

Example :
```
- CategoryUseCase
    - Caches
    - Commands
        - Create
            CreateCommand.cs
            CreateCommandHandler.cs
            CreateCommandValidator.cs
    - Contracts
        - Interfaces
        - Abstracts
    - DTOs
    - Events
    - Exceptions
    - Extensions
    - Queries
```

## Implementation of Command
To implement a `Command` manager class, follow the examples below :

Example :
```csharp
public class CreateCommand : ICommand<string> //any result type
{
    //some properties
}

public class CreateCommandHandler : ICommandHandler<CreateCommand, string>
{
    public CreateCommandHandler(){}
    
    [WithTransaction]
    public string Handle(CreateCommand command)
    {
       //logic
        
        return default;
    }

    //must be used
    [WithTransaction]
    public Task<string> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
       //logic
        
       return Task.FromResult<string>(default);
    }
}
```

In the implementation of the above codes, there are items that need to be managed if necessary :

1 . Using `WithValidationAttribute`

This `Attribute` is used when you need to validate your `Command` or `Query`. To start, you must create the corresponding `Validator` class and then apply `WithValidation` .

Example :
```csharp
public class CreateCommandValidator : IValidator<CreateCommand>
{
    public CreateCommandValidator(){}
    
    public object Validate(CreateCommand input)
    {
        //validations
        
        return default;
    }

    //must be used
    public Task<object> ValidateAsync(CreateCommand input, CancellationToken cancellationToken)
    {
        //validations
        
        return Task.FromResult(default(object));
    }
}
```

2 . In the above code, in the section related to the corresponding `Validator` class, you can use the result of the `Validate` or `ValidateAsync` method, which is an `object`, inside the corresponding `CommandHandler`

To do this, simply create a `readonly` variable of type object named `_validationResult` in your `CommandHandler` .

Example :
```csharp
public class CreateCommand : ICommand<string> //any result type
{
    //some properties
}

public class CreateCommandHandler : ICommandHandler<CreateCommand, string>
{
    private readonly object _validationResult;

    public CreateCommandHandler(){}

    [WithValidation]
    [WithTransaction]
    public string Handle(CreateCommand command)
    {
       //logic
        
        return default;
    }

    [WithValidation]
    [WithTransaction]
    public Task<string> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
       //logic
        
       return Task.FromResult<string>(default);
    }
}
```

3 . Using `WithCleanCacheAttribute`

In the `Command` section, when you need to delete the `Cache` related to the desired entity after executing the logic of the relevant section, so that the corresponding `Cache` is created again in another request sent for the relevant `Query` section, you can use this `Attribute` according to the codes below .

Example :
```csharp
public class CreateCommand : ICommand<string> //any result type
{
    //some properties
}

public class CreateCommandHandler : ICommandHandler<CreateCommand, string>
{
    public CreateCommandHandler(){}

    [WithCleanCache(Keies = "Key1|Key2|...")]
    public string Handle(CreateCommand command)
    {
       //logic
        
        return default;
    }

    [WithCleanCache(Keies = "Key1|Key2|...")]
    public Task<string> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
       //logic
        
       return Task.FromResult<string>(default);
    }
}
```

4 . Using `WithPessimisticConcurrencyAttribute`

When you need to place the logic of your `Command` section, which is a `Critical Section`, inside a `lock` block so that only one or a specific number of `Threads` can access that `Critical` section, you can use this `Attribute`. For the `Handle` method, you must create a variable of type `object` in your `CommandHandler`, and for `HandleAsync`, you must create a variable of type `SemaphoreSlim` .

Example :
```csharp
//for sync method (handle)
public class CreateCommand : ICommand<string> //any result type
{
    //some properties
}

public class CreateCommandHandler : ICommandHandler<CreateCommand, string>
{
    private static object _lock = new();
    
    public CreateCommandHandler(){}

    [WithPessimisticConcurrency]
    public string Handle(CreateCommand command)
    {
       //logic
        
        return default;
    }
}

/*---------------------------------------------------------------*/

//for async method (handle async)

public class CreateCommand : ICommand<string> //any result type
{
    //some properties
}

public class CreateCommandHandler : ICommandHandler<CreateCommand, string>
{
    private static SemaphoreSlim _asyncLock = new(1, 1); //custom count of thread
    
    public CreateCommandHandler(){}

    [WithPessimisticConcurrency]
    public Task<string> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
       //logic
        
       return Task.FromResult<string>(default);
    }
}
```

## Position of Contracts
Any contract other than repository-related contracts created in the `Domain` layer should be created in this folder .

Example :
```
- CategoryUseCase
    - Caches
    - Commands
        - Create
            CreateCommand.cs
            CreateCommandHandler.cs
            CreateCommandValidator.cs
    - Contracts
        - Interfaces
            INotificationService.cs
        - Abstracts
    - DTOs
    - Events
    - Exceptions
    - Extensions
    - Queries
```

## Implementation of Event ( for rabbitmq tool )
To implement an `Event` manager class, follow the examples below .

Example :
```csharp
//define in [Domain] layer of consumer service
[EventConfig(Queue = "queue")]
public class UpdatedEvent : UpdateDomainEvent<string> //any type of identity key
{
    //payload
}

//define in [UseCase] layer of consumer service
public class UpdatedConsumerEventBusHandler : IConsumerEventBusHandler<UpdatedEvent>
{
    public UpdatedConsumerEventBusHandler(){}

    [TransactionConfig(Type = TransactionType.Command)] //or => Type = TransactionType.Query
    public void Handle(UpdatedEvent @event)
    {
        //logic
    }
    
    //must be used
    [TransactionConfig(Type = TransactionType.Command)] //or => Type = TransactionType.Query
    public Task HandleAsync(UpdatedEvent @event, CancellationToken cancellationToken)
    {
        //logic
        
        return Task.CompleteTask;
    }
}
```

In the implementation of the above instructions, a few points are necessary to mention :

1 . Using `WithMaxRetryAttribute`

This `Attribute` allows you to manage the retry attempts of the corresponding `Consumer` for processing the relevant `Message` or `Event` .

To use this `Attribute`, you can follow the instructions below .

Example :
```csharp
//for [Event] consuming
public class UpdatedConsumerEventBusHandler : IConsumerEventBusHandler<UpdatedEvent>
{
    public UpdatedConsumerEventBusHandler(){}

    [WithMaxRetry(Count = 100, HasAfterMaxRetryHandle = true)] //Count = 100 -> this message will be reprocessed a maximum of 100 times in case of an error
    [TransactionConfig(Type = TransactionType.Command)] //or => Type = TransactionType.Query
    public void Handle(UpdatedEvent @event)
    {
        //logic
    }
    
    [WithMaxRetry(Count = 100, HasAfterMaxRetryHandle = true)] //Count = 100 -> this message will be reprocessed a maximum of 100 times in case of an error
    [TransactionConfig(Type = TransactionType.Command)] //or => Type = TransactionType.Query
    public Task HandleAsync(UpdatedEvent @event, CancellationToken cancellationToken)
    {
        //logic
        
        return Task.CompleteTask;
    }
}
```

In using `WithMaxRetryAttribute`, there is a feature called `HasAfterMaxRetryHandle`, which indicates whether there is a need to separately manage the relevant message if it has been retried more than the allowed limit. If this feature is set to false, which is the default value of this variable, the relevant message will be removed from the corresponding `Queue` after the maximum attempt to process it .
If the desired message in the corresponding `Queue` reaches the maximum retry limit (in case of possible errors), to separately manage the processing of the relevant message, you must follow the instructions below .

Example :
```csharp
//for [Event] consuming
public class UpdatedConsumerEventBusHandler : IConsumerEventBusHandler<UpdatedEvent>
{
    public UpdatedConsumerEventBusHandler(){}

    [WithMaxRetry(Count = 100, HasAfterMaxRetryHandle = true)] //Count = 100 -> this message will be reprocessed a maximum of 100 times in case of an error
    [TransactionConfig(Type = TransactionType.Command)] //or => Type = TransactionType.Query
    public void Handle(UpdatedEvent @event)
    {
        //logic
    }
    
    //must be used
    [WithMaxRetry(Count = 100, HasAfterMaxRetryHandle = true)] //Count = 100 -> this message will be reprocessed a maximum of 100 times in case of an error
    [TransactionConfig(Type = TransactionType.Command)] //or => Type = TransactionType.Query
    public Task HandleAsync(UpdatedEvent @event, CancellationToken cancellationToken)
    {
        //logic
        
        return Task.CompleteTask;
    }
    
    //for handle max retry
    
    public void AfterMaxRetryHandle(UpdatedEvent @event)
    {
        //logic
    }
    
    //must be used
    public Task AfterMaxRetryHandleAsync(UpdatedEvent @event, CancellationToken cancellationToken)
    {
        //logic
        
        return Task.CompleteTask;
    }
}
```

2 . Using `WithCleanCacheAttribute`

As explained in the `Mediator` tool, here too, we can use this `Attribute` according to the previously mentioned instructions .

## Implementation of Event ( for kafka tool )
To implement an `Event` manager class, follow the examples below .

Example :
```csharp
//define in [Domain] layer of consumer service
[EventConfig(Topic = "Topic")]
public class UpdatedEvent : UpdateDomainEvent<string> //any type of identity key
{
    //payload
}

//define in [UseCase] layer of consumer service
public class UpdatedConsumerEventStreamHandler : IConsumerEventStreamHandler<UpdatedEvent>
{
    public UpdatedConsumerEventStreamHandler(){}

    [TransactionConfig(Type = TransactionType.Command)] //or => Type = TransactionType.Query
    public void Handle(UpdatedEvent @event)
    {
        //logic
    }
    
    //must be used
    [TransactionConfig(Type = TransactionType.Command)] //or => Type = TransactionType.Query
    public Task HandleAsync(UpdatedEvent @event, CancellationToken cancellationToken)
    {
        //logic
        
        return Task.CompleteTask;
    }
}
```

In the implementation of the above instructions, a few points are necessary to mention :

1 . Using `WithMaxRetryAttribute`

This `Attribute` allows you to manage the retry attempts of the corresponding `Consumer` for processing the relevant `Message` or `Event` .

To use this `Attribute`, you can follow the instructions below .

Example :
```csharp
//for [Event] consuming
public class UpdatedConsumerEventStreamHandler : IConsumerEventStreamHandler<UpdatedEvent>
{
    public UpdatedConsumerEventStreamHandler(){}

    [WithMaxRetry(Count = 100, HasAfterMaxRetryHandle = true)] //Count = 100 -> this message will be reprocessed a maximum of 100 times in case of an error
    [TransactionConfig(Type = TransactionType.Command)] //or => Type = TransactionType.Query
    public void Handle(UpdatedEvent @event)
    {
        //logic
    }
    
    //must be used
    [WithMaxRetry(Count = 100, HasAfterMaxRetryHandle = true)] //Count = 100 -> this message will be reprocessed a maximum of 100 times in case of an error
    [TransactionConfig(Type = TransactionType.Command)] //or => Type = TransactionType.Query
    public Task HandleAsync(UpdatedEvent @event, CancellationToken cancellationToken)
    {
        //logic
        
        return Task.CompleteTask;
    }
}
```

In using `WithMaxRetryAttribute`, there is a feature called `HasAfterMaxRetryHandle`, which indicates whether there is a need to separately manage the relevant message if it has been retried more than the allowed limit. If this feature is set to false, which is the default value of this variable, the relevant message will be removed from the corresponding `Queue` after the maximum attempt to process it .
If the desired message in the corresponding `Queue` reaches the maximum retry limit (in case of possible errors), to separately manage the processing of the relevant message, you must follow the instructions below .

Example :
```csharp
//for [Event] consuming
public class UpdatedConsumerEventStreamHandler : IConsumerEventStreamHandler<UpdatedEvent>
{
    public UpdatedConsumerEventStreamHandler(){}

    [WithMaxRetry(Count = 100, HasAfterMaxRetryHandle = true)] //Count = 100 -> this message will be reprocessed a maximum of 100 times in case of an error
    [TransactionConfig(Type = TransactionType.Command)] //or => Type = TransactionType.Query
    public void Handle(UpdatedEvent @event)
    {
        //logic
    }
    
    //must be used
    [WithMaxRetry(Count = 100, HasAfterMaxRetryHandle = true)] //Count = 100 -> this message will be reprocessed a maximum of 100 times in case of an error
    [TransactionConfig(Type = TransactionType.Command)] //or => Type = TransactionType.Query
    public Task HandleAsync(UpdatedEvent @event, CancellationToken cancellationToken)
    {
        //logic
        
        return Task.CompleteTask;
    }
    
    //for handle max retry
    
    public void AfterMaxRetryHandle(UpdatedEvent @event)
    {
        //logic
    }
    
    //must be used
    public Task AfterMaxRetryHandleAsync(UpdatedEvent @event, CancellationToken cancellationToken)
    {
        //logic
        
        return Task.CompleteTask;
    }
}
```

2 . Using `WithCleanCacheAttribute`

As explained in the `Mediator` tool, here too, we can use this `Attribute` according to the previously mentioned instructions .


## Position of Queries
The location for creating a `Query` manager is as follows :

Example :
```
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
        - ReadAllPagination
            ReadAllPaginationQuery.cs
            ReadAllPaginationQueryHandler.cs
            ReadAllPaginationQueryValidator.cs
```

## Implementation of Query
To implement a `Query` manager class, follow the examples below .

Example :
```csharp
public class ReadAllQuery : IQuery<Dto> //any result type
{
}

public class ReadAllQueryHandler : IQueryHandler<ReadAllQuery, Dto>
{
    public ReadAllQueryHandler(){}

    public Dto Handle(ReadAllQuery query)
    {
        //query
        
        return default;
    }
    
    //must be used
    public Task<Dto> HandleAsync(ReadAllQuery query, CancellationToken cancellationToken)
    {
        //query
        
        return Task.FromResult<Dto>(default);
    }
}
```

## Persistence Folder Structure Details
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