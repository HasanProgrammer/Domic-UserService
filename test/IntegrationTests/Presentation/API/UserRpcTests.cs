using System.Threading.Tasks;
using FluentAssertions;
using Grpc.Core;
using Karami.Core.Common.ClassConsts;
using Karami.Core.Grpc.User;
using Karami.Core.UseCase.DTOs;
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

        var token = _testBase.JsonWebToken.Generate(
            new TokenParameterDto {
                Key      = "1996_1375_1996",
                Issuer   = "Dotris",
                Audience = "Dotris",
                Expires  = 60
            }
        );

        Metadata metadata = new() { { Header.Token, token } };

        CreateRequest request = new() {
            FirstName   = new String { Value = firstName }   ,
            LastName    = new String { Value = lastName }    ,
            Description = new String { Value = description } ,
            Username    = new String { Value = username }    ,
            Password    = new String { Value = password }    ,
            PhoneNumber = new String { Value = phoneNumber } ,
            Email       = new String { Value = email }
        };

        var userClient = new UserService.UserServiceClient(_testBase.Channel);

        //Act

        var result = await userClient.CreateAsync(request, metadata);

        //Assert

        result.Code.Should().Be(200);
    }
}