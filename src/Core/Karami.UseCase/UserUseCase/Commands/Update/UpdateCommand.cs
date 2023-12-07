using Karami.Core.UseCase.Contracts.Interfaces;

namespace Karami.UseCase.UserUseCase.Commands.Update;

public class UpdateCommand : ICommand<string>
{
    public required string Token                    { get; set; }
    public required string Id                       { get; set; }
    public required string Username                 { get; set; }
    public required string Password                 { get; set; }
    public required string FirstName                { get; set; }
    public required string LastName                 { get; set; }
    public required string PhoneNumber              { get; set; }
    public required string EMail                    { get; set; }
    public required string Description              { get; set; }
    public required bool IsActive                   { get; set; }
    public required IEnumerable<string> Roles       { get; set; }
    public required IEnumerable<string> Permissions { get; set; }
}