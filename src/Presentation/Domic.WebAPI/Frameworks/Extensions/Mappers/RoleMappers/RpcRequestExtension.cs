using Domic.Core.Role.Grpc;
using Domic.Domain.Commons.Enumerations;
using Domic.UseCase.RoleUseCase.Commands.Create;
using Domic.UseCase.RoleUseCase.Commands.Delete;
using Domic.UseCase.RoleUseCase.Commands.Update;
using Domic.UseCase.RoleUseCase.Queries.ReadAllPaginated;
using Domic.UseCase.RoleUseCase.Queries.ReadOne;

namespace Domic.WebAPI.Frameworks.Extensions.Mappers.RoleMappers;

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
                PageNumber   = request.PageNumber?.Value   ,
                CountPerPage = request.CountPerPage?.Value ,
                SearchText   = request.SearchText?.Value   ,
                Sort         = (Sort)request.Sort.Value 
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
                Name  = request.Name?.Value
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
                Id    = request.TargetId?.Value ,
                Name  = request.Name?.Value
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