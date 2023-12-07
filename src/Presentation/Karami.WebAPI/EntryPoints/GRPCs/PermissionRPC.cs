using Grpc.Core;
using Karami.Core.Common.ClassHelpers;
using Karami.Core.Grpc.Permission;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.WebAPI.Extensions;
using Karami.UseCase.PermissionUseCase.Commands.Create;
using Karami.UseCase.PermissionUseCase.Commands.Delete;
using Karami.UseCase.PermissionUseCase.Commands.Update;
using Karami.UseCase.PermissionUseCase.DTOs.ViewModels;
using Karami.UseCase.PermissionUseCase.Queries.ReadAllPaginated;
using Karami.UseCase.PermissionUseCase.Queries.ReadOne;
using Karami.WebAPI.Frameworks.Extensions.Mappers.PermissionMappers;

namespace Karami.WebAPI.EntryPoints.GRPCs;

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
        
        var result = await _mediator.DispatchAsync<PermissionsViewModel>(query, context.CancellationToken);
        
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

        var result =
            await _mediator.DispatchAsync<PaginatedCollection<PermissionsViewModel>>(query, context.CancellationToken);
        
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
        var command = request.ToCommand<UpdateCommand>(context.GetHttpContext().GetTokenOfGrpcHeader());

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
        var command = request.ToCommand<DeleteCommand>(context.GetHttpContext().GetTokenOfGrpcHeader());

        var result = await _mediator.DispatchAsync<string>(command, context.CancellationToken);
        
        return result.ToRpcResponse<DeleteResponse>(_configuration);
    }
}