#pragma warning disable CS0649

using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.UseCase.Attributes;
using Karami.Domain.Role.Contracts.Interfaces;
using Karami.Domain.Role.Entities;

namespace Karami.UseCase.RoleUseCase.Commands.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand, string>
{
    private readonly object _validationResult;

    private readonly IDateTime               _dateTime;
    private readonly ISerializer             _serializer;
    private readonly IJsonWebToken           _jsonWebToken;
    private readonly IRoleCommandRepository  _roleCommandRepository;

    public UpdateCommandHandler(IRoleCommandRepository roleCommandRepository, IDateTime dateTime, 
        ISerializer serializer, IJsonWebToken jsonWebToken
    )
    {
        _dateTime               = dateTime;
        _serializer             = serializer;
        _jsonWebToken           = jsonWebToken;
        _roleCommandRepository  = roleCommandRepository;
    }

    [WithValidation]
    [WithTransaction]
    public Task<string> HandleAsync(UpdateCommand command, CancellationToken cancellationToken)
    {
        var targetRole = _validationResult as Role;
        var updateBy   = _jsonWebToken.GetIdentityUserId(command.Token);
        var updateRole = _serializer.Serialize( _jsonWebToken.GetRoles(command.Token) );

        targetRole.Change(_dateTime, updateBy, updateRole, command.Name);

        _roleCommandRepository.Change(targetRole);

        return Task.FromResult(targetRole.Id);
    }
}