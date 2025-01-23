using Domic.Core.Common.ClassExtensions;
using Domic.Core.Common.ClassHelpers;
using Domic.Core.User.Grpc;
using Domic.Core.Infrastructure.Extensions;
using Domic.UseCase.UserUseCase.DTOs.ViewModels;

namespace Domic.WebAPI.Frameworks.Extensions.Mappers.UserMappers;

//Query
public static partial class RpcResponseExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToRpcResponse<T>(this bool model)
    {
        object Response = null;

        if (typeof(T) == typeof(CheckExistResponse))
        {
            Response = new CheckExistResponse { Result = model };
        }

        return (T) Response;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <param name="configuration"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToRpcResponse<T>(this UsersDto model, IConfiguration configuration)
    {
        object Response = null;

        if (typeof(T) == typeof(ReadOneResponse))
        {
            Response = new ReadOneResponse {
                Code    = configuration.GetSuccessStatusCode()       ,
                Message = configuration.GetSuccessFetchDataMessage() ,
                Body = new ReadOneResponseBody { User = model.Serialize() }
            };
        }

        return (T) Response;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="models"></param>
    /// <param name="configuration"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToRpcResponse<T>(this PaginatedCollection<UsersDto> models, IConfiguration configuration)
    {
        object Response = null;

        if (typeof(T) == typeof(ReadAllPaginatedResponse))
        {
            Response = new ReadAllPaginatedResponse {
                Code    = configuration.GetSuccessStatusCode()       ,
                Message = configuration.GetSuccessFetchDataMessage() ,
                Body    = new ReadAllPaginatedResponseBody {
                    Users = models.Serialize()
                }
            };
        }

        return (T) Response;
    }
}

//Command
public partial class RpcResponseExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="response"></param>
    /// <param name="configuration"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToRpcResponse<T>(this string response, IConfiguration configuration)
    {
        object Response = null;

        if (typeof(T) == typeof(CreateResponse))
        {
            Response = new CreateResponse {
                Code    = configuration.GetSuccessCreateStatusCode() ,
                Message = configuration.GetSuccessCreateMessage()    ,
                Body    = new CreateResponseBody { UserId = response }
            };
        }
        else if (typeof(T) == typeof(UpdateResponse))
        {
            Response = new UpdateResponse {
                Code    = configuration.GetSuccessStatusCode()    ,
                Message = configuration.GetSuccessUpdateMessage() ,
                Body    = new UpdateResponseBody { UserId = response }
            };
        }
        else if (typeof(T) == typeof(ActiveResponse))
        {
            Response = new ActiveResponse {
                Code    = configuration.GetSuccessStatusCode()    ,
                Message = configuration.GetSuccessUpdateMessage() ,
                Body    = new ActiveResponseBody { UserId = response }
            };
        }
        else if (typeof(T) == typeof(InActiveResponse))
        {
            Response = new InActiveResponse {
                Code    = configuration.GetSuccessStatusCode()    ,
                Message = configuration.GetSuccessUpdateMessage() ,
                Body    = new InActiveResponseBody { UserId = response }
            };
        }

        return (T)Response;
    }
}