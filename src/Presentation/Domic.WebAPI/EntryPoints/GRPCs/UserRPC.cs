using Grpc.Core;
using Domic.Core.Common.ClassHelpers;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.User.Grpc;
using Domic.UseCase.UserUseCase.Commands.Active;
using Domic.UseCase.UserUseCase.Commands.CheckExist;
using Domic.UseCase.UserUseCase.Commands.Create;
using Domic.UseCase.UserUseCase.Commands.InActive;
using Domic.UseCase.UserUseCase.Commands.Update;
using Domic.UseCase.UserUseCase.DTOs.ViewModels;
using Domic.UseCase.UserUseCase.Queries.ReadAllPaginated;
using Domic.UseCase.UserUseCase.Queries.ReadOne;
using Domic.WebAPI.Frameworks.Extensions.Mappers.UserMappers;

namespace Domic.WebAPI.EntryPoints.GRPCs;

public class UserRPC : UserService.UserServiceBase
{
    private readonly IMediator      _mediator;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="configuration"></param>
    public UserRPC(IMediator mediator, IConfiguration configuration)
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
    public override async Task<CheckExistResponse> CheckExist(CheckExistRequest request, ServerCallContext context)
    {
        var query = request.ToQuery<CheckExistCommand>();

        var result = await _mediator.DispatchAsync<bool>(query, context.CancellationToken);

        return result.ToRpcResponse<CheckExistResponse>();
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

        var result = await _mediator.DispatchAsync<UsersDto>(query, context.CancellationToken);
        
        return result.ToRpcResponse<ReadOneResponse>(_configuration);
    }

    //todo: shoud be used [ReadAllPaginated] insted [ReadAllPaginate] in gRPC
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

        var result = await _mediator.DispatchAsync<PaginatedCollection<UsersDto>>(query, context.CancellationToken);
        
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
    public override async Task<ActiveResponse> Active(ActiveRequest request, ServerCallContext context)
    {
        var command = request.ToCommand<ActiveCommand>();

        var result = await _mediator.DispatchAsync<string>(command, context.CancellationToken);
        
        return result.ToRpcResponse<ActiveResponse>(_configuration);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<InActiveResponse> InActive(InActiveRequest request, ServerCallContext context)
    {
        var command = request.ToCommand<InActiveCommand>();

        var result = await _mediator.DispatchAsync<string>(command, context.CancellationToken);
        
        return result.ToRpcResponse<InActiveResponse>(_configuration);
    }
}