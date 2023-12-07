using FluentAssertions;
using Karami.Core.Grpc.User;
using Xunit;

namespace Presentation.API;

public class UserRpcTests : IClassFixture<IntegrationTestBase>
{
    private readonly IntegrationTestBase _TestBase;

    public UserRpcTests(IntegrationTestBase TestBase)
    {
        _TestBase = TestBase;
    }

    [Fact]
    public void Should_CreateUser_WhenCallCreateMethodOfUserRpc()
    {
        //Arrange

        CreateRequest Request = new() {
            FirstName   = new String { Value = "Hasan" }  ,
            LastName    = new String { Value = "Karami" } ,
            Description = new String { Value = "4gXDlJRPguRND4qZ0dhk0LvZ1TqgYCY0fqvVtZJiCwjLCW3fOEm1HfSYZjzdkaRDhklxbRCz3uwuLKlJmGG89oDl61f0DBhEMsi3r" } ,
            Username    = new String { Value = "HasanProgrammer" } ,
            Password    = new String { Value = "Hasan@123@313@@" } ,
            PhoneNumber = new String { Value = "09026676147" }     ,
            Email       = new String { Value = "hasankarami2020313@gmail.com" }
        };

        var UserClient = new UserService.UserServiceClient(_TestBase.Channel);

        //Act

        var Result = UserClient.Create(Request);

        //Assert

        Result.Code.Should().Be(200);
    }
}