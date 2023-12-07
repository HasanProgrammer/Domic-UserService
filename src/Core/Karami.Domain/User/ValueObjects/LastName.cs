using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Exceptions;

namespace Karami.Domain.User.ValueObjects;

public class LastName : ValueObject
{
    public readonly string Value;

    /// <summary>
    /// 
    /// </summary>
    public LastName() {}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InValidValueObjectException"></exception>
    public LastName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("فیلد نام خانوادگی الزامی می باشد !");

        if (value.Length is > 80 or < 3)
            throw new DomainException("فیلد نام خانوادگی نباید بیشتر از 50 و کمتر از 3 عبارت داشته باشد !");

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}