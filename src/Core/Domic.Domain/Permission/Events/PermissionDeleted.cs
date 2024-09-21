using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Constants;
using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Enumerations;

namespace Domic.Domain.Permission.Events;

[EventConfig(ExchangeType = Exchange.FanOut, Exchange = Broker.User_Permission_Exchange)]
public class PermissionDeleted : UpdateDomainEvent<string>
{
    
}