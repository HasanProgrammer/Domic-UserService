using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Exceptions;

namespace Karami.Domain.User.ValueObjects;

public class Description : ValueObject
{
    public readonly string Value;

    /// <summary>
    /// 
    /// </summary>
    public Description() {}
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InValidValueObjectException"></exception>
    public Description(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("فیلد توضیحات الزامی می باشد !");

        if (value.Length is > 500 or < 100)
            throw new DomainException("فیلد توضیحات نباید بیشتر از 500 و کمتر از 100 عبارت داشته باشد !");

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}