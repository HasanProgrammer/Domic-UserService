using Domic.Core.Common.ClassExtensions;
using Domic.Core.Common.ClassHelpers;
using Domic.Core.Permission.Grpc;
using Domic.Core.Infrastructure.Extensions;
using Domic.UseCase.PermissionUseCase.DTOs;

namespace Domic.WebAPI.Frameworks.Extensions.Mappers.PermissionMappers;

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
    public static T ToRpcResponse<T>(this PermissionDto model, IConfiguration configuration)
    {
        object Response = null;

        if (typeof(T) == typeof(ReadOneResponse))
        {
            Response = new ReadOneResponse {
                Code    = configuration.GetSuccessStatusCode()       ,
                Message = configuration.GetSuccessFetchDataMessage() ,
                Body    = new ReadOneResponseBody {
                    Permission = new PermissionObject {
                        Id       = model.Id     , 
                        Name     = model.Name   ,
                        RoleId   = model.RoleId ,
                        RoleName = model.RoleName
                    }
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
    public static T ToRpcResponse<T>(this PaginatedCollection<PermissionDto> models, IConfiguration configuration)
    {
        object Response = null;
    
        if (typeof(T) == typeof(ReadAllPaginatedResponse))
        {
            Response = new ReadAllPaginatedResponse {
                Code    = configuration.GetSuccessStatusCode()       ,
                Message = configuration.GetSuccessFetchDataMessage() ,
                Body    = new ReadAllPaginatedResponseBody {
                    Permissions = models.Serialize()
                }
            };
        }
        else if (typeof(T) == typeof(ReadAllBasedOnRolesPaginatedResponse))
        {
            Response = new ReadAllBasedOnRolesPaginatedResponse {
                Code    = configuration.GetSuccessStatusCode()       ,
                Message = configuration.GetSuccessFetchDataMessage() ,
                Body    = new ReadAllBasedOnRolesPaginatedResponseBody {
                    Permissions = models.Serialize()
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
                Body    = new CreateResponseBody { PermissionId = response }
            };
        }
        else if (typeof(T) == typeof(UpdateResponse))
        {
            Response = new UpdateResponse {
                Code    = configuration.GetSuccessStatusCode()    ,
                Message = configuration.GetSuccessUpdateMessage() ,
                Body    = new UpdateResponseBody { PermissionId = response }
            };
        }
        else if (typeof(T) == typeof(DeleteResponse))
        {
            Response = new DeleteResponse {
                Code    = configuration.GetSuccessStatusCode()    ,
                Message = configuration.GetSuccessDeleteMessage() ,
                Body    = new DeleteResponseBody { PermissionId = response }
            };
        }

        return (T)Response;
    }
}