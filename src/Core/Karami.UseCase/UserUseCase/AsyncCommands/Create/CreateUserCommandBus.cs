using Karami.Common.ClassConsts;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Abstracts;

namespace Karami.UseCase.UserUseCase.AsyncCommands.Create;

[Queueable(
    Exchange = MessageBroker.UserCommandExchange, 
    Route    = MessageBroker.UserCommandRoute, 
    Queue    = MessageBroker.UserCommandQueue
)]
public class CreateUserCommandBus : CreateAsyncCommand
{
    public required string Token                    { get; set; }
    public required string Username                 { get; set; }
    public required string Password                 { get; set; }
    public required string FirstName                { get; set; }
    public required string LastName                 { get; set; }
    public required string PhoneNumber              { get; set; }
    public required string EMail                    { get; set; }
    public required string Description              { get; set; }
    public required IEnumerable<string> Roles       { get; set; }
    public required IEnumerable<string> Permissions { get; set; }
}