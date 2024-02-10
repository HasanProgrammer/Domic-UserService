using Domic.Core.UseCase.Contracts.Interfaces;

namespace Domic.UseCase.UserUseCase.Commands.Create;

public class CreateCommand : ICommand<string>
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