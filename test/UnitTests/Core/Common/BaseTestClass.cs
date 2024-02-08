using Karami.Core.Domain.Contracts.Interfaces;
using NSubstitute;

namespace Core.Common;

public class BaseTestClass
{
    protected readonly IDateTime _dotrisDateTime;

    public BaseTestClass()
    {
        var dotrisDateTime = Substitute.For<IDateTime>();

        dotrisDateTime.ToPersianShortDate(default).ReturnsForAnyArgs(DateTime.Now.ToShortDateString());

        _dotrisDateTime = dotrisDateTime;
    }
}