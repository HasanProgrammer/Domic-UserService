using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Constants;
using Domic.Core.Domain.Contracts.Abstracts;
using Domic.Core.Domain.Enumerations;

namespace Domic.Domain.User.Events;

[EventConfig(ExchangeType = Exchange.FanOut, Exchange = Broker.User_User_Exchange, Queue = Broker.User_User_Queue)]
public class UserUpdated : UpdateDomainEvent<string>
{
    public string Username                 { get; init; }
    public string Password                 { get; init; }
    public string FirstName                { get; init; }
    public string LastName                 { get; init; }
    public string Description              { get; init; }
    public string PhoneNumber              { get; init; }
    public string Email                    { get; init; }
    public IEnumerable<string> Roles       { get; init; }
    public IEnumerable<string> Permissions { get; init; }
}