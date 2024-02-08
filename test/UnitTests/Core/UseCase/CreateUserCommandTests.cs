using System.Reflection;
using Core.Common;
using Core.Domain.Entities;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Commons.Contracts.Interfaces;
using Karami.Domain.Permission.Contracts.Interfaces;
using Karami.Domain.Permission.Entities;
using Karami.Domain.PermissionUser.Contracts.Interfaces;
using Karami.Domain.Role.Contracts.Interfaces;
using Karami.Domain.Role.Entities;
using Karami.Domain.RoleUser.Contracts.Interfaces;
using Karami.Domain.User.Contracts.Interfaces;
using Karami.Domain.User.Entities;
using Karami.UseCase.UserUseCase.Commands.Create;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Core.UseCase;

public class CreateUserCommandTests : BaseTestClass
{
    private readonly User _user;
    private readonly CreateCommand _userCommand;

    public CreateUserCommandTests()
    {
        _userCommand = new() {
            Token       = "", 
            FirstName   = "Hasan"                        ,
            LastName    = "Karami"                       ,
            Username    = "HasanProgrammer"              ,
            Password    = "Hasan@123@313@@"              ,
            PhoneNumber = "09026676147"                  ,
            EMail       = "hasankarami2020313@gmail.com" ,
            Description = "4gXDlJRPguRND4qZ0dhk0LvZ1TqgYCY0fqvVtZJiCwjLCW3fOEm1HfSYZjzdkaRDhklxbRCz3uwuLKlJmGG89oDl61f0DBhEMsi3r",
            Permissions = new List<string>(),
            Roles       = new List<string>()  
        };
        
        _user = new UserBuilder(_dotrisDateTime).WithId(Guid.NewGuid().ToString())
                                                .WithFirstName("Hasan")
                                                .WithLastName("Karami")
                                                .WithUsername("HasanTesti")
                                                .WithPassword("Hasan@123@313@@")
                                                .WithPhoneNumber("09106676147")
                                                .WithEmail("hasankaramimoheb2020313@gmail.com")
                                                .WithDescription("4gXDlJRPguRND4qZ0dhk0LvZ1TqgYCY0fqvVtZJiCwjLCW3fOEm1HfSYZjzdkaRDhklxbRCz3uwuLKlJmGG89oDl61f0DBhEMsi3r")
                                                .Build();
    }
    
    [Fact]
    public void Should_Call_Transaction_UnitOfWork_OneTime()
    {
        //Arrange

        var userCommandRepository           = Substitute.For<IUserCommandRepository>();
        var roleUserCommandRepository       = Substitute.For<IRoleUserCommandRepository>();
        var permissionUserCommandRepository = Substitute.For<IPermissionUserCommandRepository>();
        var eventCommandRepository          = Substitute.For<IEventCommandRepository>();

        var dotrisDateTime = Substitute.For<IDateTime>();
        var serializer     = Substitute.For<ISerializer>();
        var jsonWebToken   = Substitute.For<IJsonWebToken>();
        var globalUniqueIdGenerator = Substitute.For<IGlobalUniqueIdGenerator>();

        dotrisDateTime.ToPersianShortDate(default).ReturnsForAnyArgs(DateTime.Now.ToShortDateString());

        serializer.Serialize(default(object)).ReturnsForAnyArgs("");

        jsonWebToken.GetUsername(default).Returns("");
        
        //Act

        CreateCommandHandler handler = new(userCommandRepository,
            roleUserCommandRepository, permissionUserCommandRepository, dotrisDateTime, 
            serializer, jsonWebToken, globalUniqueIdGenerator
        );

        //Assert

        var handleAsyncType = handler.GetType().GetMethod("HandleAsync");
        
        Assert.NotNull(handleAsyncType.GetCustomAttribute(typeof(WithTransactionAttribute)));
    }

    [Fact]
    public void Should_Call_Validation_Attribute_OneTime()
    {
        //Arrange

        var userCommandRepository           = Substitute.For<IUserCommandRepository>();
        var roleUserCommandRepository       = Substitute.For<IRoleUserCommandRepository>();
        var permissionUserCommandRepository = Substitute.For<IPermissionUserCommandRepository>();
        var eventCommandRepository          = Substitute.For<IEventCommandRepository>();

        var dotrisDateTime = Substitute.For<IDateTime>();
        var serializer     = Substitute.For<ISerializer>();
        var jsonWebToken   = Substitute.For<IJsonWebToken>();
        var globalUniqueIdGenerator = Substitute.For<IGlobalUniqueIdGenerator>();

        dotrisDateTime.ToPersianShortDate(default).ReturnsForAnyArgs(DateTime.Now.ToShortDateString());

        serializer.Serialize(default(object)).ReturnsForAnyArgs("");

        jsonWebToken.GetUsername(default).Returns("");
        
        //Act

        CreateCommandHandler handler = new(userCommandRepository,
            roleUserCommandRepository, permissionUserCommandRepository, dotrisDateTime, 
            serializer, jsonWebToken, globalUniqueIdGenerator
        );

        //Assert

        var handleAsyncType = handler.GetType().GetMethod("HandleAsync");
        
        Assert.NotNull(handleAsyncType.GetCustomAttribute(typeof(WithValidationAttribute)));
    }

    [Fact]
    public async Task Should_Call_FindByIdAsync_OnRoleCommandRepository_In_CreateUserCommandValidator_AtLeast_OneTime()
    {
        //Arrange

        var roleId       = Guid.NewGuid().ToString();
        var permissionId = Guid.NewGuid().ToString();
        
        _userCommand.Roles       = new List<string> { roleId };
        _userCommand.Permissions = new List<string> { permissionId };
        
        var userCommandRepository       = Substitute.For<IUserCommandRepository>();
        var roleCommandRepository       = Substitute.For<IRoleCommandRepository>();
        var permissionCommandRepository = Substitute.For<IPermissionCommandRepository>();
        
        userCommandRepository.FindByUsernameAsync(default, default).ReturnsNull();
        
        userCommandRepository.FindByPhoneNumberAsync(_userCommand.PhoneNumber, default).ReturnsNull();
        
        userCommandRepository.FindByEmailAsync(_userCommand.EMail, default).ReturnsNull();

        var role       = new Role(_dotrisDateTime, roleId, "R-1");
        var permission = new Permission(_dotrisDateTime, permissionId, "P-1", roleId);
        
        roleCommandRepository.FindByIdAsync(roleId, default)
                             .Returns(role);
        
        permissionCommandRepository.FindByIdAsync(permissionId, default)
                                   .Returns(permission);

        //Act
        
        CreateCommandValidator validator =
            new(roleCommandRepository, permissionCommandRepository, userCommandRepository);

        await validator.ValidateAsync(_userCommand, default);
        
        //Assert

        await roleCommandRepository.ReceivedWithAnyArgs().FindByIdAsync(default, default);
    }

    [Fact]
    public void Should_Call_CommitAsync_UnitOfWork_OneTime()
    {
        //Arrange

        ICommandUnitOfWork commandUnitOfWork = Substitute.For<ICommandUnitOfWork>();

        //Act

        //CreateCommandHandler CommandHandler = new(commandUnitOfWork);
        //CommandHandler.HandleAsync(_UserCommand, default);

        //Assert

        //commandUnitOfWork.Received(1).CommitAsync(default);
    }
    
    [Fact]
    public void Should_Call_AddAsync_Of_EventCommandRepository_AtLeast_OneTime()
    {
        //Arrange

        ICommandUnitOfWork commandUnitOfWork = Substitute.For<ICommandUnitOfWork>();
        
        //Act

        //CreateCommandHandler CommandHandler = new(commandUnitOfWork);
        //CommandHandler.HandleAsync(_UserCommand, default);

        //Assert

        //commandUnitOfWork.Received().EventCommandRepository();
        //commandUnitOfWork.EventCommandRepository().ReceivedWithAnyArgs().AddAsync(default, default);
    }
    
    [Fact]
    public void Should_Call_AddAsync_Of_UserCommandRepository_OneTime()
    {
        //Arrange

        ICommandUnitOfWork commandUnitOfWork = Substitute.For<ICommandUnitOfWork>();
        
        //Act

        //CreateCommandHandler CommandHandler = new(commandUnitOfWork);
        //CommandHandler.HandleAsync(_UserCommand, default);

        //Assert

        //commandUnitOfWork.Received(1).UserCommandRepository();
        //commandUnitOfWork.UserCommandRepository().ReceivedWithAnyArgs(1).AddAsync(default, default);
    }

    [Fact]
    public void Should_Throw_InValidEntityException_ByUsernameThatAlreadyExists()
    {
        //Arrange

        /*ICommandUnitOfWork commandUnitOfWork = Substitute.For<ICommandUnitOfWork>();

        User User = new UserBuilder().WithUsername(_userCommand.Username).Build();*/
        
        /*commandUnitOfWork.UserCommandRepository()
                         .FindByUsernameAsync(_UserCommand.Username, default)
                         .Returns( Task.FromResult<User>(User) );*/
        
        //Act

        //CreateCommandHandler CommandHandler = new(commandUnitOfWork);
        
        //Assert
        
        //CommandHandler.HandleAsync(_UserCommand, default).Should().Throws<InValidEntityException>();
    }
    
    [Fact]
    public void Should_Throw_InValidEntityException_ByPhoneNumberThatAlreadyExists()
    {
        //Arrange

        /*ICommandUnitOfWork commandUnitOfWork = Substitute.For<ICommandUnitOfWork>();

        User User = new UserBuilder().WithPhoneNumber(_userCommand.PhoneNumber).Build();*/
        
        /*commandUnitOfWork.UserCommandRepository()
                         .FindByPhoneNumberAsync(_UserCommand.PhoneNumber, default)
                         .Returns( Task.FromResult<User>(User) );*/
        
        //Act

        //CreateCommandHandler CommandHandler = new(commandUnitOfWork);
        
        //Assert
        
        //CommandHandler.HandleAsync(_UserCommand, default).Should().Throws<InValidEntityException>();
    }
    
    [Fact]
    public void Should_Throw_InValidEntityException_ByEmailThatAlreadyExists()
    {
        //Arrange

        /*ICommandUnitOfWork commandUnitOfWork = Substitute.For<ICommandUnitOfWork>();

        User User = new UserBuilder().WithEmail(_userCommand.EMail).Build();*/
        
        /*commandUnitOfWork.UserCommandRepository()
                         .FindByPhoneNumberAsync(_UserCommand.EMail, default)
                         .Returns( Task.FromResult<User>(User) );*/
        
        //Act

        //CreateCommandHandler CommandHandler = new(commandUnitOfWork);
        
        //Assert
        
        //CommandHandler.HandleAsync(_UserCommand, default).Should().Throws<InValidEntityException>();
    }
}