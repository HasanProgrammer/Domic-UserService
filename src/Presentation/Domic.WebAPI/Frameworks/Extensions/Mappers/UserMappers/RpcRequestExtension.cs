using Domic.Core.User.Grpc;
using Domic.Core.Infrastructure.Extensions;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Commons.Enumerations;
using Domic.UseCase.UserUseCase.AsyncCommands.Create;
using Domic.UseCase.UserUseCase.Commands.Active;
using Domic.UseCase.UserUseCase.Commands.CheckExist;
using Domic.UseCase.UserUseCase.Commands.Create;
using Domic.UseCase.UserUseCase.Commands.InActive;
using Domic.UseCase.UserUseCase.Commands.Update;
using Domic.UseCase.UserUseCase.Queries.ReadAllPaginated;
using Domic.UseCase.UserUseCase.Queries.ReadOne;

namespace Domic.WebAPI.Frameworks.Extensions.Mappers.UserMappers;

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
                Id = request.TargetId?.Value
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
                PageNumber   = request.PageNumber?.Value   ,
                CountPerPage = request.CountPerPage?.Value ,
                SearchText   = request.SearchText?.Value   ,
                Sort         = (Sort)request.Sort?.Value
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
    public static T ToCommand<T>(this CreateRequest request)
    {
        object Request = null;

        if (typeof(T) == typeof(CreateCommand))
        {
            Request = new CreateCommand {
                ImageUrl    = request.ImageUrl?.Value    ,
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
    public static T ToCommand<T>(this UpdateRequest request)
    {
        object Request = null;

        if (typeof(T) == typeof(UpdateCommand))
        {
            Request = new UpdateCommand {
                Id          = request.TargetId?.Value    ,
                ImageUrl    = request.ImageUrl?.Value    ,  
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
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToCommand<T>(this ActiveRequest request)
    {
        object Request = null;

        if (typeof(T) == typeof(ActiveCommand))
        {
            Request = new ActiveCommand {
                Id = request.TargetId?.Value
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
    public static T ToCommand<T>(this InActiveRequest request)
    {
        object Request = null;

        if (typeof(T) == typeof(InActiveCommand))
        {
            Request = new InActiveCommand {
                Id = request.TargetId?.Value
            };
        }

        return (T)Request;
    }
}