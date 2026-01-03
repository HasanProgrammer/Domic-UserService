using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Constants;
using Domic.Core.Domain.Contracts.Abstracts;

namespace Domic.Domain.User.Events;

[EventConfig(Queue = Broker.User_User_Queue)]
public class EmailVerifyCodeSended : CreateDomainEvent<string>
{
    public string EmailAddress { get; set; }
    public string VerifyCode { get; set; }
}