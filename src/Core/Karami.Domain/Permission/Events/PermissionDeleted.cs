using Karami.Core.Domain.Attributes;
using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Enumerations;

namespace Karami.Domain.Permission.Events;

[MessageBroker(ExchangeType = Exchange.FanOut, Exchange = "User_Permission_Exchange")]
public class PermissionDeleted : DeleteDomainEvent
{
    public string Id { get; set; }
}