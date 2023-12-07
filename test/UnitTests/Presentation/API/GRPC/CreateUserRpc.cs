#pragma warning disable CS4014

using Karami.Core.Grpc.User;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.UseCase.UserUseCase.Commands.Create;
using Karami.WebAPI.EntryPoints.GRPCs;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Xunit;

namespace Presentation.API.GRPC;

public class CreateUserRpc
{
    [Fact]
    public void Should_Call_DispatchAsync_Of_Mediator_OneTime()
    {
        //Arrange

        IMediator Mediator = Substitute.For<IMediator>();

        IConfiguration Configuration = Substitute.For<IConfiguration>();

        CreateRequest Request = new() {
            FirstName   = new String { Value = "" } ,
            LastName    = new String { Value = "" } ,
            Username    = new String { Value = "" } ,
            Password    = new String { Value = "" } ,
            PhoneNumber = new String { Value = "" } ,
            Email       = new String { Value = "" } ,
            Description = new String { Value = "" }
        };

        //Act

        new UserRPC(Mediator, Configuration).Create(Request, default);

        //Assert

        Mediator.ReceivedWithAnyArgs(1).DispatchAsync<string>( Arg.Any<CreateCommand>() , default);
    }
}