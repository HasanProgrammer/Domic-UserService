using Karami.Common.ClassConsts;
using Karami.Core.Common.ClassConsts;
using Karami.Core.Domain.Entities;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.Role.Entities;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Extensions;
using Karami.Core.UseCase.Attributes;
using Karami.Domain.Role.Contracts.Interfaces;

using Action = Karami.Core.Common.ClassConsts.Action;

namespace Karami.UseCase.RoleUseCase.Commands.Create;

public class CreateCommandHandler : ICommandHandler<CreateCommand, string>
{
    private readonly IDotrisDateTime         _dotrisDateTime;
    private readonly ISerializer             _serializer;
    private readonly IJsonWebToken               _jsonWebToken;
    private readonly IRoleCommandRepository  _roleCommandRepository;
    private readonly IEventCommandRepository _eventCommandRepository;

    public CreateCommandHandler(IRoleCommandRepository roleCommandRepository, 
        IEventCommandRepository eventCommandRepository, 
        IDotrisDateTime dotrisDateTime,
        ISerializer serializer, 
        IJsonWebToken jsonWebToken
    )
    {
        _dotrisDateTime         = dotrisDateTime;
        _serializer             = serializer;
        _jsonWebToken = jsonWebToken;
        _roleCommandRepository  = roleCommandRepository;
        _eventCommandRepository = eventCommandRepository;
    }

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        var role = new Role(_dotrisDateTime, Guid.NewGuid().ToString(), command.Name);

        #region OutBox

        var events = role.GetEvents.ToEntityOfEvent(_dotrisDateTime, _serializer, Service.UserService,
            Table.RoleTable, Action.Create, _jsonWebToken.GetUsername(command.Token)
        );

        foreach (Event @event in events)
            await _eventCommandRepository.AddAsync(@event, cancellationToken);
        
        role.ClearEvents();

        #endregion

        await _roleCommandRepository.AddAsync(role, cancellationToken);

        return role.Id;
    }
}