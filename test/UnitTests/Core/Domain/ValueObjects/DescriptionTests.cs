using System;
using Karami.Core.Domain.Exceptions;
using Karami.Domain.User.ValueObjects;
using Xunit;

namespace Core.Domain.ValueObjects;

public class DescriptionTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_ThrowInValidValueObjectException_WhenNullValue(string value)
    {
        //Act
        
        Action Action = () => new Description(value);

        //Assert

        DomainException Exception = Assert.Throws<DomainException>(Action);

        Assert.Equal("فیلد توضیحات الزامی می باشد !", Exception.Message);
    }
    
    [Theory]
    [InlineData("WeSeygU6YzVycuuam33ZVgokIUjUpdEgMe1cwgoPSet0sfDyM0FEkO3MCQe7afwuoiaIcrYnYiH4n1XWpsSpKio9XalwAV2Vmh2")]
    [InlineData("aRSVhohSmBn7J0NwN39cLS912P3ayAyripIwixPhF7UU27HmBYqV27uHOksqIUbkTfIlD4zOf50PBR6fjs74771FmlyafnSF4UneRXXoFWGsVvdRNjGCfJw3XZBdfQe8ey9kkQoYKbRUx2DAdgVFPTWNMK4P0bm9ZrNSxOquY0rCKacGbRpAaXfUWNd1pn7vxo7S5CifqfRE6Ax2UHSruQTrtbkNRHHQ7dCoMat7hWUi6g4GRsOghkZRKFfTNeH5wYMSvr0sRHSnu33AUdmplIaVQUYOtBKE7pLJTZHcUmC8I4WBLjVL4fiSKLYE2rZivFVKI8IqhDrSJ6PimI0Hc88DI1XHSBsoCp9t7a2gis8PXsNPdLfRoxc03uAKP1B9hZtfDjOmhxitQO9uoFv5XfnhbQPnUmNNgFvrtvaEiTB5xWYGqB0UpYHF5YPPGLeuYG8oDJQE9atZgRPhQo7mYsn1wyAqcKMtkZNVOMoMRbNxCHXjOYdMz")]
    public void Should_ThrowInValidValueObjectException_WhenValueIsBiggerThan500_And_SmallerThan100(string value)
    {
        //Act
        
        Action Action = () => new Description(value);

        //Assert

        DomainException Exception = Assert.Throws<DomainException>(Action);

        Assert.Equal("فیلد توضیحات نباید بیشتر از 500 و کمتر از 100 عبارت داشته باشد !", Exception.Message);
    }
}