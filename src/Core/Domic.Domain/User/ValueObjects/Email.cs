using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Exceptions;
using Domic.Core.Domain.Extensions;

namespace Domic.Domain.User.ValueObjects;

public class Email : ValueObject
{
    public readonly string Value;

    /// <summary>
    /// 
    /// </summary>
    public Email() {}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InValidValueObjectException"></exception>
    public Email(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new DomainException("فیلد پست الکترونیکی الزامی می باشد !");
        
        if(!value.IsValidEmail())
            throw new DomainException("فیلد پست الکترونیکی ارسالی معتبر نمی باشد !");

        Value = value;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}