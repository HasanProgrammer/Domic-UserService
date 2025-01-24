using Domic.Core.Infrastructure.Extensions;
using Domic.Core.Permission.Grpc;
using Domic.UseCase.PermissionUseCase.Commands.Create;
using Domic.UseCase.PermissionUseCase.Commands.Delete;
using Domic.UseCase.PermissionUseCase.Commands.Update;
using Domic.UseCase.PermissionUseCase.Queries.ReadAllBasedOnRolesPaginated;
using Domic.UseCase.PermissionUseCase.Queries.ReadAllPaginated;
using Domic.UseCase.PermissionUseCase.Queries.ReadOne;

namespace Domic.WebAPI.Frameworks.Extensions.Mappers.PermissionMappers;

//Query
public static partial class RpcRequestExtension
{
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
                PageNumber   = request.PageNumber?.Value ,
                CountPerPage = request.CountPerPage?.Value 
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
    public static T ToQuery<T>(this ReadAllBasedOnRolesPaginatedRequest request)
    {
        object Request = null;

        if (typeof(T) == typeof(ReadAllBasedOnRolesPaginatedQuery))
        {
            Request = new ReadAllBasedOnRolesPaginatedQuery {
                PageNumber   = request.PageNumber?.Value   ,
                CountPerPage = request.CountPerPage?.Value ,
                Roles        = request.Roles != null ? request.Roles.Value.Split(",").ToList() : new List<string>()
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
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToCommand<T>(this CreateRequest request)
    {
        object Request = null;

        if (typeof(T) == typeof(CreateCommand))
        {
            Request = new CreateCommand {
                Name   = request.Name?.Value ,
                RoleId = request.RoleId?.Value
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
    public static T ToCommand<T>(this UpdateRequest request)
    {
        object Request = null;

        if (typeof(T) == typeof(UpdateCommand))
        {
            Request = new UpdateCommand {
                Id     = request.TargetId?.Value ,
                Name   = request.Name?.Value     ,
                RoleId = request.RoleId?.Value
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
    public static T ToCommand<T>(this DeleteRequest request)
    {
        object Request = null;

        if (typeof(T) == typeof(DeleteCommand))
        {
            Request = new DeleteCommand {
                Id = request.TargetId?.Value
            };
        }

        return (T)Request;
    }
}