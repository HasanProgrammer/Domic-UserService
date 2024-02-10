using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Constants;
using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Enumerations;

namespace Domic.Domain.User.Events;

[MessageBroker(ExchangeType = Exchange.FanOut, Exchange = Broker.User_User_Exchange, Queue = Broker.User_User_Queue)]
public class UserInActived : UpdateDomainEvent<string>
{
    public required string OwnerUsername { get; init; }
}