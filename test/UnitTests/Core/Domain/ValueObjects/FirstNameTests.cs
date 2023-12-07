using System;
using Karami.Core.Domain.Exceptions;
using Karami.Domain.User.ValueObjects;
using Xunit;

namespace Core.Domain.ValueObjects;

public class FirstNameTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_ThrowInValidValueObjectException_WhenNullValue(string value)
    {
        //Act
        
        Action Action = () => new FirstName(value);

        //Assert

        DomainException Exception = Assert.Throws<DomainException>(Action);

        Assert.Equal("فیلد نام الزامی می باشد !", Exception.Message);
    }
    
    [Theory]
    [InlineData("We")]
    [InlineData("veElAw9zZfkHvZj3uuYIPRg4eqj4AiTVoiw2CLxTm7OQ9k6heK8")]
    public void Should_ThrowInValidValueObjectException_WhenValueIsBiggerThan50_And_SmallerThan3(string value)
    {
        //Act
        
        Action Action = () => new FirstName(value);

        //Assert

        DomainException Exception = Assert.Throws<DomainException>(Action);

        Assert.Equal("فیلد نام نباید بیشتر از 50 و کمتر از 3 عبارت داشته باشد !", Exception.Message);
    }
}