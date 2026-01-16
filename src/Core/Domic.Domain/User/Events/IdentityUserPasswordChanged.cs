using Domic.Core.Domain.Attributes;
using Domic.Core.Domain.Contracts.Abstracts;

namespace Domic.Domain.User.Events;

[EventConfig(Queue = "User_User_Command_Queue")]
public class IdentityUserPasswordChanged : UpdateDomainEvent<string>
{
    public string NewPassword { get; init; }
}