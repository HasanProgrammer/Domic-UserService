using Karami.Core.Grpc.Role;
using Karami.UseCase.RoleUseCase.Commands.Create;
using Karami.UseCase.RoleUseCase.Commands.Delete;
using Karami.UseCase.RoleUseCase.Commands.Update;
using Karami.UseCase.RoleUseCase.Queries.ReadAllPaginated;
using Karami.UseCase.RoleUseCase.Queries.ReadOne;

namespace Karami.WebAPI.Frameworks.Extensions.Mappers.RoleMappers;

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
                RoleId = request.TargetId?.Value
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
                Token = token,
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
    public static T ToCommand<T>(this UpdateRequest request, string token)
    {
        object Request = null;

        if (typeof(T) == typeof(UpdateCommand))
        {
            Request = new UpdateCommand {
                Token = token                   , 
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
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToCommand<T>(this DeleteRequest request, string token)
    {
        object Request = null;

        if (typeof(T) == typeof(DeleteCommand))
        {
            Request = new DeleteCommand {
                Token  = token,
                RoleId = request.TargetId?.Value
            };
        }

        return (T)Request;
    }
}