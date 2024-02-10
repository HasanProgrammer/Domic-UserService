using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Exceptions;
using Domic.Core.Domain.Extensions;

namespace Domic.Domain.User.ValueObjects;

public class PhoneNumber : ValueObject
{
    public readonly string Value;
    
    /// <summary>
    /// 
    /// </summary>
    public PhoneNumber() {}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InValidValueObjectException"></exception>
    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("فیلد شماره تماس الزامی می باشد !");
        
        if(!value.IsValidMobileNumber())
            throw new DomainException("فیلد شماره تماس ارسالی معتبر نمی باشد !");

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}