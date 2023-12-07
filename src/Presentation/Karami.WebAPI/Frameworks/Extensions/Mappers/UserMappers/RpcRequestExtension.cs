using Karami.Core.Grpc.User;
using Karami.Core.Infrastructure.Extensions;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.UseCase.UserUseCase.AsyncCommands.Create;
using Karami.UseCase.UserUseCase.Commands.Active;
using Karami.UseCase.UserUseCase.Commands.CheckExist;
using Karami.UseCase.UserUseCase.Commands.Create;
using Karami.UseCase.UserUseCase.Commands.InActive;
using Karami.UseCase.UserUseCase.Commands.Update;
using Karami.UseCase.UserUseCase.Queries.ReadAllPaginated;
using Karami.UseCase.UserUseCase.Queries.ReadOne;

namespace Karami.WebAPI.Frameworks.Extensions.Mappers.UserMappers;

//Query
public static partial class RpcRequestExtension
{
    public static T ToQuery<T>(this CheckExistRequest request)
    {
        object Request = null;

        if (typeof(T) == typeof(CheckExistCommand))
        {
            Request = new CheckExistCommand {
                UserId = request.TargetId?.Value
            };
        }
        
        return (T)Request;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToQuery<T>(this ReadOneRequest request)
    {
        object Request = null;

        if (typeof(T) == typeof(ReadOneQuery))
        {
            Request = new ReadOneQuery {
                UserId = request.TargetId?.Value
            };
        }
        
        return (T)Request;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToQuery<T>(this ReadAllPaginatedRequest request)
    {
        object Request = null;

        if (typeof(T) == typeof(ReadAllPaginatedQuery))
        {
            Request = new ReadAllPaginatedQuery {
                PageNumber   = request.PageNumber?.Value ,
                CountPerPage = request.CountPerPage?.Value 
            };
        }
        
        return (T)Request;
    }
}

//Command
public partial class RpcRequestExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToCommand<T>(this CreateRequest request, string token)
    {
        object Request = null;

        if (typeof(T) == typeof(CreateCommand))
        {
            Request = new CreateCommand {
                Token       = token                      , 
                Username    = request.Username?.Value    ,
                Password    = request.Password?.Value    ,
                FirstName   = request.FirstName?.Value   ,
                LastName    = request.LastName?.Value    ,
                PhoneNumber = request.PhoneNumber?.Value ,
                EMail       = request.Email?.Value       ,
                Description = request.Description?.Value ,
                Roles       = request.Roles?.Value.DeSerialize<List<string>>() ,
                Permissions = request.Permissions?.Value.DeSerialize<List<string>>()
            };
        }

        return (T)Request;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToAsyncCommand<T>(this CreateRequest request, string token) where T : IAsyncCommand
    {
        object Request = null;

        if (typeof(T) == typeof(CreateUserCommandBus))
        {
            Request = new CreateUserCommandBus {
                Token       = token                      , 
                Username    = request.Username?.Value    ,
                Password    = request.Password?.Value    ,
                FirstName   = request.FirstName?.Value   ,
                LastName    = request.LastName?.Value    ,
                PhoneNumber = request.PhoneNumber?.Value ,
                EMail       = request.Email?.Value       ,
                Description = request.Description?.Value ,
                Roles       = request.Roles?.Value.DeSerialize<List<string>>() ,
                Permissions = request.Permissions?.Value.DeSerialize<List<string>>()
            };
        }

        return (T)Request;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToCommand<T>(this UpdateRequest request, string token)
    {
        object Request = null;

        if (typeof(T) == typeof(UpdateCommand))
        {
            Request = new UpdateCommand {
                Token       = token                      ,
                Id          = request.TargetId?.Value    ,
                FirstName   = request.FirstName?.Value   ,
                LastName    = request.LastName?.Value    ,
                Description = request.Description?.Value ,
                PhoneNumber = request.PhoneNumber?.Value ,
                EMail       = request.Email?.Value       ,
                Username    = request.Username?.Value    ,
                Password    = request.Password?.Value    ,
                IsActive    = request.IsActive           , 
                Roles       = request.Roles?.Value.DeSerialize<List<string>>() ,
                Permissions = request.Permissions?.Value.DeSerialize<List<string>>()
            };
        }

        return (T)Request;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToCommand<T>(this ActiveRequest request, string token)
    {
        object Request = null;

        if (typeof(T) == typeof(ActiveCommand))
        {
            Request = new ActiveCommand {
                Id    = request.TargetId?.Value ,
                Token = token
            };
        }

        return (T)Request;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToCommand<T>(this InActiveRequest request, string token)
    {
        object Request = null;

        if (typeof(T) == typeof(InActiveCommand))
        {
            Request = new InActiveCommand {
                Id    = request.TargetId?.Value ,
                Token = token
            };
        }

        return (T)Request;
    }
}