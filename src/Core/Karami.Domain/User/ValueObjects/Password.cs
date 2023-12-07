using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Exceptions;
using Karami.Core.Domain.Extensions;

namespace Karami.Domain.User.ValueObjects;

public class Password : ValueObject
{
    public readonly string Value;

    /// <summary>
    /// 
    /// </summary>
    public Password() {}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InValidValueObjectException"></exception>
    public Password(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("فیلد رمز عبور الزامی می باشد !");

        if (value.Length < 8)
            throw new DomainException("فیلد رمز عبور نباید کمتر از 8 عبارت داشته باشد !");

        Value = value.HashAsync().Result;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}