using Karami.Core.Domain.Attributes;
using Karami.Core.Domain.Constants;
using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Enumerations;

namespace Karami.Domain.Role.Events;

[MessageBroker(ExchangeType = Exchange.FanOut, Exchange = Broker.User_Role_Exchange, Queue = Broker.User_Role_Queue)]
public class RoleUpdated : UpdateDomainEvent<string>
{
    public string Name { get; init; }
}