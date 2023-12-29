using System.Threading.Tasks;
using FluentAssertions;
using Grpc.Core;
using Karami.Core.Common.ClassConsts;
using Karami.Core.Grpc.User;
using Shouldly;
using Xunit;

namespace Presentation.API;

public class UserRpcTests : IClassFixture<IntegrationTestBase>
{
    private readonly IntegrationTestBase _testBase;

    public UserRpcTests(IntegrationTestBase TestBase)
    {
        _testBase = TestBase;
    }

    [Theory]
    [InlineData("Hasan", "Karami",
        "4gXDlJRPguRND4qZ0dhk0LvZ1TqgYCY0fqvVtZJiCwjLCW3fOEm1HfSYZjzdkaRDhklxbRCz3uwuLKlJmGG89oDl61f0DBhEMsi3r",
        "HasanProgrammer", "Hasan@123@313@@", "09026676147", "hasankarami2020313@gmail.com"
    )]
    public async Task Should_CreateUser_WhenCallCreateMethodOfUserRpc(string firstName, string lastName, 
        string description, string username, string password, string phoneNumber, string email
    )
    {
        //Arrange

        Metadata metadata = new() { { Header.Token, _testBase.GenerateToken() } };

        CreateRequest request = new() {
            FirstName   = new String { Value = firstName }   ,
            LastName    = new String { Value = lastName }    ,
            Description = new String { Value = description } ,
            Username    = new String { Value = username }    ,
            Password    = new String { Value = password }    ,
            PhoneNumber = new String { Value = phoneNumber } ,
            Email       = new String { Value = email }
        };

        var userGrpcClient = new UserService.UserServiceClient(_testBase.Channel);

        //Act

        var result = await userGrpcClient.CreateAsync(request, metadata);

        //Assert

        result.ShouldNotBeNull();
        result.Code.Should().Be(200);
    }
}