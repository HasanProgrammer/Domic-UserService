using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Constants;
using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Enumerations;

namespace Domic.Domain.Role.Events;

[MessageBroker(ExchangeType = Exchange.FanOut, Exchange = Broker.User_Role_Exchange, Queue = Broker.User_Role_Queue)]
public class RoleCreated : CreateDomainEvent<string>
{
    public string Name { get; init; }
}