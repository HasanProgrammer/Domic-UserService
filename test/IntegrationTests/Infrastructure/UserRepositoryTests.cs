using System;
using System.Threading;
using System.Threading.Tasks;
using Karami.Infrastructure.Implementations.Domain.Repositories.C;
using Karami.Persistence.Contexts.C;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Infrastructure;

public class UserRepositoryTests
{
    private readonly SQLContext _Context;

    public UserRepositoryTests()
    {
        DbContextOptionsBuilder<SQLContext> builder = new DbContextOptionsBuilder<SQLContext>();
        
        builder.UseSqlServer("Server=.;Database=UserService;Trusted_Connection=true;");

        _Context = new SQLContext(builder.Options);
    }

    [Fact]
    public void Should_AddedUserToDatabase_WhenCallAddAsyncMethod_OfUserCommandRepository()
    {
        //Arrange

        CommandUnitOfWork commandUnitOfWork = new CommandUnitOfWork(_Context);

        //User NewUser = new User();

        //Act
        
        //UnitOfWork.UserCommandRepository().AddAsync()

        //Assert
    }
}