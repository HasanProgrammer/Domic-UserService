using Karami.Core.Domain.Attributes;
using Karami.Core.Domain.Constants;
using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Enumerations;

namespace Karami.Domain.User.Events;

[MessageBroker(ExchangeType = Exchange.FanOut, Exchange = Broker.User_User_Exchange, Queue = Broker.User_User_Queue)]
public class UserActived : UpdateDomainEvent
{
    public string Id            { get; init; }
    public string OwnerUsername { get; init; }
}