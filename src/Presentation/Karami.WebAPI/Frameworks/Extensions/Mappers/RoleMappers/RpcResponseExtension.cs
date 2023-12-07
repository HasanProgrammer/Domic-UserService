using Karami.Core.Common.ClassHelpers;
using Karami.Core.Grpc.Role;
using Karami.Core.Infrastructure.Extensions;
using Karami.UseCase.RoleUseCase.DTOs.ViewModels;

namespace Karami.WebAPI.Frameworks.Extensions.Mappers.RoleMappers;

//Query
public static partial class RpcResponseExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <param name="configuration"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ToRpcResponse<T>(this RolesViewModel model, IConfiguration configuration)
    {
        object Response = null;

        if (typeof(T) == typeof(ReadOneResponse))
        {
            Response = new ReadOneResponse {
                Code    = configuration.GetValue<int>("StatusCode:SuccessFetchData")    ,
                Message = configuration.GetValue<string>("Message:FA:SuccessFetchData") ,
                Body    = new ReadOneResponseBody {
                    Role = model.Serialize()
                }
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
    public static T ToRpcResponse<T>(this PaginatedCollection<RolesViewModel> models, IConfiguration configuration)
    {
        object Response = null;

        if (typeof(T) == typeof(ReadAllPaginatedResponse))
        {
            Response = new ReadAllPaginatedResponse {
                Code    = configuration.GetValue<int>("StatusCode:SuccessFetchData")    ,
                Message = configuration.GetValue<string>("Message:FA:SuccessFetchData") ,
                Body    = new ReadAllPaginatedResponseBody {
                    Roles = models.Serialize()
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
                Code    = configuration.GetValue<int>("StatusCode:SuccessCreate")    ,
                Message = configuration.GetValue<string>("Message:FA:SuccessCreate") ,
                Body    = new CreateResponseBody { RoleId = response }
            };
        }
        else if (typeof(T) == typeof(UpdateResponse))
        {
            Response = new UpdateResponse {
                Code    = configuration.GetValue<int>("StatusCode:SuccessUpdate")    ,
                Message = configuration.GetValue<string>("Message:FA:SuccessUpdate") ,
                Body    = new UpdateResponseBody { RoleId = response }
            };
        }
        else if (typeof(T) == typeof(DeleteResponse))
        {
            Response = new DeleteResponse {
                Code    = configuration.GetValue<int>("StatusCode:SuccessDelete")    ,
                Message = configuration.GetValue<string>("Message:FA:SuccessDelete") ,
                Body    = new DeleteResponseBody { RoleId = response }
            };
        }

        return (T)Response;
    }
}