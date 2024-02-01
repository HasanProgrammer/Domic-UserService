using Karami.Core.Domain.Attributes;
using Karami.Core.Domain.Constants;
using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Enumerations;

namespace Karami.Domain.Permission.Events;

[MessageBroker(ExchangeType = Exchange.FanOut, Exchange = Broker.User_Permission_Exchange, Queue = Broker.User_Permission_Queue)]
public class PermissionCreated : CreateDomainEvent<string>
{
    public string RoleId { get; init; }
    public string Name   { get; init; }
}