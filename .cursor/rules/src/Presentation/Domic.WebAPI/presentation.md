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