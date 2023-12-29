using System;
using Karami.Domain.User.Entities;
using Karami.Infrastructure.Implementations.Domain.Repositories;
using Karami.Infrastructure.Implementations.Domain.Repositories.C;
using Karami.Persistence.Contexts;
using Karami.Persistence.Contexts.C;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace Infrastructure.Domain;

public class UserRepositoryTests
{
    private readonly SQLContext _moqContext;

    public UserRepositoryTests()
    {
        _moqContext = Substitute.For<SQLContext>(
            new DbContextOptionsBuilder<SQLContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options
        );

        _moqContext.Users = _moqContext.Set<User>();
    }

    [Fact]
    public async void Should_CallAddAsyncOnUsersDbSet_WhenCallAddAsyncMethodOf_UserCommandRepository()
    {
        //Arrange

        UserCommandRepository Repository = new UserCommandRepository(_moqContext);

        //Act

        await Repository.AddAsync(default, default);

        //Assert

        await _moqContext.Users.ReceivedWithAnyArgs(1).AddAsync(default);
    }
    
    [Fact]
    public async void Should_CallUpdateOnUsersDbSet_WhenCallChangeAsyncMethodOf_UserCommandRepository()
    {
        //Arrange

        UserCommandRepository Repository = new UserCommandRepository(_moqContext);

        //Act

        Repository.Change( default );

        //Assert
        
        _moqContext.Users.ReceivedWithAnyArgs(1).Update( default );
    }
}