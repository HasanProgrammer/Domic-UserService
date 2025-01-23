using Grpc.Core;
using Domic.Core.Common.ClassHelpers;
using Domic.Core.Role.Grpc;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.WebAPI.Extensions;
using Domic.UseCase.RoleUseCase.Commands.Create;
using Domic.UseCase.RoleUseCase.Commands.Delete;
using Domic.UseCase.RoleUseCase.Commands.Update;
using Domic.UseCase.RoleUseCase.DTOs;
using Domic.UseCase.RoleUseCase.Queries.ReadAllPaginated;
using Domic.UseCase.RoleUseCase.Queries.ReadOne;
using Domic.WebAPI.Frameworks.Extensions.Mappers.RoleMappers;

namespace Domic.WebAPI.EntryPoints.GRPCs;

public class RoleRPC : RoleService.RoleServiceBase
{
    private readonly IMediator      _mediator;
    private readonly IConfiguration _configuration;

    public RoleRPC(IMediator mediator, IConfiguration configuration)
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

        var result = await _mediator.DispatchAsync<RoleDto>(query, context.CancellationToken);
            
        return result.ToRpcResponse<ReadOneResponse>(_configuration);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<ReadAllPaginatedResponse> ReadAllPaginate(ReadAllPaginatedRequest request,
        ServerCallContext context
    )
    {
        var query = request.ToQuery<ReadAllPaginatedQuery>();

        var result = await _mediator.DispatchAsync<PaginatedCollection<RoleDto>>(query, context.CancellationToken);
        
        return result.ToRpcResponse<ReadAllPaginatedResponse>(_configuration);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<CreateResponse> Create(CreateRequest request, ServerCallContext context)
    {
        var command = request.ToCommand<CreateCommand>(context.GetHttpContext().GetTokenOfGrpcHeader());

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