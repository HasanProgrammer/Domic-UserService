using Grpc.Core;
using Karami.Core.Common.ClassHelpers;
using Karami.Core.Grpc.Role;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.WebAPI.Extensions;
using Karami.UseCase.RoleUseCase.Commands.Create;
using Karami.UseCase.RoleUseCase.Commands.Delete;
using Karami.UseCase.RoleUseCase.Commands.Update;
using Karami.UseCase.RoleUseCase.DTOs.ViewModels;
using Karami.UseCase.RoleUseCase.Queries.ReadAllPaginated;
using Karami.UseCase.RoleUseCase.Queries.ReadOne;
using Karami.WebAPI.Frameworks.Extensions.Mappers.RoleMappers;

namespace Karami.WebAPI.EntryPoints.GRPCs;

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

        var result = await _mediator.DispatchAsync<RolesViewModel>(query, context.CancellationToken);
            
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

        var result = await _mediator.DispatchAsync<PaginatedCollection<RolesViewModel>>(query, context.CancellationToken);
        
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

    public override async Task<DeleteResponse> Delete(DeleteRequest request, ServerCallContext context)
    {
        var command = request.ToCommand<DeleteCommand>(context.GetHttpContext().GetTokenOfGrpcHeader());

        var result = await _mediator.DispatchAsync<string>(command, context.CancellationToken);
        
        return result.ToRpcResponse<DeleteResponse>(_configuration);
    }
}