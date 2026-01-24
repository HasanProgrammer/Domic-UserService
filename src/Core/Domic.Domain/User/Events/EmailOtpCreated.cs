using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Enumerations;

namespace Domic.Domain.User.Events;

[EventConfig(ExchangeType = Exchange.FanOut, Exchange = "User_EmailOtp_Exchange")]
public class EmailOtpCreated : CreateDomainEvent<string>
{
    public string UserId { get; init; }
    public string EmailAddress { get; set; }
    public string MessageContent { get; init; }
}