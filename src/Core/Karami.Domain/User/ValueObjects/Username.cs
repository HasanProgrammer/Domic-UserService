using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Exceptions;

namespace Karami.Domain.User.ValueObjects;

public class Username : ValueObject
{
    public readonly string Value;
    
    /// <summary>
    /// 
    /// </summary>
    public Username() {}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InValidValueObjectException"></exception>
    public Username(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("فیلد نام کاربری الزامی می باشد !");

        if (value.Length is > 30 or < 8)
            throw new DomainException("فیلد نام کاربری نباید بیشتر از 30 و کمتر از 8 عبارت داشته باشد !");

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}