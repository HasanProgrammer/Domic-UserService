using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Constants;
using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Enumerations;

namespace Domic.Domain.Permission.Events;

[MessageBroker(ExchangeType = Exchange.FanOut, Exchange = Broker.User_Permission_Exchange, Queue = Broker.User_Permission_Queue)]
public class PermissionCreated : CreateDomainEvent<string>
{
    public string RoleId { get; init; }
    public string Name   { get; init; }
}