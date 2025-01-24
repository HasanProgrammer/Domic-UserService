using Grpc.Core;
using Domic.Core.Common.ClassHelpers;
using Domic.Core.Permission.Grpc;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.UseCase.PermissionUseCase.Commands.Create;
using Domic.UseCase.PermissionUseCase.Commands.Delete;
using Domic.UseCase.PermissionUseCase.Commands.Update;
using Domic.UseCase.PermissionUseCase.DTOs;
using Domic.UseCase.PermissionUseCase.Queries.ReadAllBasedOnRolesPaginated;
using Domic.UseCase.PermissionUseCase.Queries.ReadAllPaginated;
using Domic.UseCase.PermissionUseCase.Queries.ReadOne;
using Domic.WebAPI.Frameworks.Extensions.Mappers.PermissionMappers;

namespace Domic.WebAPI.EntryPoints.GRPCs;

public class PermissionRPC : PermissionService.PermissionServiceBase
{
    private readonly IMediator      _mediator;
    private readonly IConfiguration _configuration;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="configuration"></param>
    public PermissionRPC(IMediator mediator, IConfiguration configuration)
    {
        _mediator      = mediator;
        _configuration = configuration;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<ReadOneResponse> ReadOne(ReadOneRequest request, ServerCallContext context)
    {
        var query = request.ToQuery<ReadOneQuery>();
        
        var result = await _mediator.DispatchAsync<PermissionDto>(query, context.CancellationToken);
        
        return result.ToRpcResponse<ReadOneResponse>(_configuration);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<ReadAllPaginatedResponse> ReadAllPaginated(ReadAllPaginatedRequest request,
        ServerCallContext context
    )
    {
        var query = request.ToQuery<ReadAllPaginatedQuery>();

        var result =
            await _mediator.DispatchAsync<PaginatedCollection<PermissionDto>>(query, context.CancellationToken);
        
        return result.ToRpcResponse<ReadAllPaginatedResponse>(_configuration);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<ReadAllBasedOnRolesPaginatedResponse> ReadAllBasedOnRolesPaginated(ReadAllBasedOnRolesPaginatedRequest request,
        ServerCallContext context
    )
    {
        var query = request.ToQuery<ReadAllBasedOnRolesPaginatedQuery>();
    
        var result =
            await _mediator.DispatchAsync<PaginatedCollection<PermissionDto>>(query, context.CancellationToken);
        
        return result.ToRpcResponse<ReadAllBasedOnRolesPaginatedResponse>(_configuration);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<CreateResponse> Create(CreateRequest request, ServerCallContext context)
    {
        var command = request.ToCommand<CreateCommand>();

        var result = await _mediator.DispatchAsync<string>(command, context.CancellationToken);
        
        return result.ToRpcResponse<CreateResponse>(_configuration);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<UpdateResponse> Update(UpdateRequest request, ServerCallContext context)
    {
        var command = request.ToCommand<UpdateCommand>();

        var result = await _mediator.DispatchAsync<string>(command, context.CancellationToken);
        
        return result.ToRpcResponse<UpdateResponse>(_configuration);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<DeleteResponse> Delete(DeleteRequest request, ServerCallContext context)
    {
        var command = request.ToCommand<DeleteCommand>();

        var result = await _mediator.DispatchAsync<string>(command, context.CancellationToken);
        
        return result.ToRpcResponse<DeleteResponse>(_configuration);
    }
}