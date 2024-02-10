
using Domic.Domain.User.Entities;
using Shouldly;
using Xunit;

namespace Core.Domain.Entities;

public class UserTests
{
    [Fact]
    public void Should_ReturnValueProps_WhenCreateUserBySameValuePropsThatPassedToConstructor()
    {
        //Arrange
        
        const string FirstName   = "Hasan";
        const string LastName    = "Domic";
        const string Description = "4gXDlJRPguRND4qZ0dhk0LvZ1TqgYCY0fqvVtZJiCwjLCW3fOEm1HfSYZjzdkaRDhklxbRCz3uwuLKlJmGG89oDl61f0DBhEMsi3r";
        const string Username    = "HasanProgrammer";
        const string Password    = "Hasan@123@313@@";
        const string PhoneNumber = "09026676147";
        const string Email       = "hasanDomic2020313@gmail.com";

        //Act

        User UserModel = new(default, Guid.NewGuid().ToString(), FirstName, LastName, Description, Username, Password, PhoneNumber, Email, null, null);

        //Assert
        
        UserModel.FirstName.Value.ShouldBe(FirstName);
        UserModel.LastName.Value.ShouldBe(LastName);
        UserModel.Description.Value.ShouldBe(Description);
        UserModel.Username.Value.ShouldBe(Username);
        UserModel.Password.Value.ShouldBe(Password);
    }
    
    [Fact]
    public void Should_RegisterEvent_WhenCreatedUserAtLeastOne()
    {
        //Arrange
        
        //Act

        //User UserModel = new UserBuilder().Build();

        //Assert
        
        // UserModel.GetEvents.ShouldNotBeNull();
        // UserModel.GetEvents.ShouldNotBeEmpty();
        // UserModel.GetEvents.Count.ShouldBeGreaterThanOrEqualTo(1);
    }
    
    [Fact]
    public void ShouldBe_Called_CheckUsernameAlreadyExistsAsync_Method_OneTime()
    {
        //Arrange

        //Act

        //new UserBuilder().Build();

        //Assert
    }
}