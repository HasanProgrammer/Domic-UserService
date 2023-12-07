using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Exceptions;

namespace Karami.Domain.User.ValueObjects;

public class FirstName : ValueObject
{
    public readonly string Value;

    /// <summary>
    /// 
    /// </summary>
    public FirstName() {}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InValidValueObjectException"></exception>
    public FirstName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("فیلد نام الزامی می باشد !");

        if (value.Length is > 50 or < 3)
            throw new DomainException("فیلد نام نباید بیشتر از 50 و کمتر از 3 عبارت داشته باشد !");

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}