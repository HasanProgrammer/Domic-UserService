using Grpc.Core;
using Karami.Core.Common.ClassHelpers;
using Karami.Core.Grpc.User;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.WebAPI.Extensions;
using Karami.UseCase.UserUseCase.AsyncCommands.Create;
using Karami.UseCase.UserUseCase.Commands.Active;
using Karami.UseCase.UserUseCase.Commands.CheckExist;
using Karami.UseCase.UserUseCase.Commands.Create;
using Karami.UseCase.UserUseCase.Commands.InActive;
using Karami.UseCase.UserUseCase.Commands.Update;
using Karami.UseCase.UserUseCase.DTOs.ViewModels;
using Karami.UseCase.UserUseCase.Queries.ReadAllPaginated;
using Karami.UseCase.UserUseCase.Queries.ReadOne;
using Karami.WebAPI.Frameworks.Extensions.Mappers.UserMappers;

namespace Karami.WebAPI.EntryPoints.GRPCs;

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

        return result.ToRpcResponse<CheckExistResponse>(_configuration);
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

        var result = await _mediator.DispatchAsync<UsersViewModel>(query, context.CancellationToken);
        
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

        var result = await _mediator.DispatchAsync<PaginatedCollection<UsersViewModel>>(query, context.CancellationToken);
        
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
        /*var command = request.ToAsyncCommand<CreateUserCommandBus>(context.GetHttpContext().GetTokenOfGrpcHeader());

        await _mediator.DispatchAsFireAndForgetAsync(command, context.CancellationToken);
        
        return new CreateResponse {
            Code    = _configuration.GetValue<int>("StatusCode:SuccessCreate")    ,
            Message = _configuration.GetValue<string>("Message:FA:SuccessCreate") ,
            Body    = new CreateResponseBody { UserId = "" }
        };*/
        
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
    public override async Task<ActiveResponse> Active(ActiveRequest request, ServerCallContext context)
    {
        var command = request.ToCommand<ActiveCommand>(context.GetHttpContext().GetTokenOfGrpcHeader());

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
        var command = request.ToCommand<InActiveCommand>(context.GetHttpContext().GetTokenOfGrpcHeader());

        var result = await _mediator.DispatchAsync<string>(command, context.CancellationToken);
        
        return result.ToRpcResponse<InActiveResponse>(_configuration);
    }
}