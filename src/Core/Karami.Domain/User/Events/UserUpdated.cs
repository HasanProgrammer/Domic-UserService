using Karami.Core.Domain.Attributes;
using Karami.Core.Domain.Constants;
using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Core.Domain.Enumerations;

namespace Karami.Domain.User.Events;

[MessageBroker(ExchangeType = Exchange.FanOut, Exchange = Broker.User_User_Exchange, Queue = Broker.User_User_Queue)]
public class UserUpdated : UpdateDomainEvent<string>
{
    public string Username                 { get; init; }
    public string Password                 { get; init; }
    public string FirstName                { get; init; }
    public string LastName                 { get; init; }
    public string Description              { get; init; }
    public string PhoneNumber              { get; init; }
    public string Email                    { get; init; }
    public bool IsActive                   { get; init; }
    public IEnumerable<string> Roles       { get; init; }
    public IEnumerable<string> Permissions { get; init; }
}