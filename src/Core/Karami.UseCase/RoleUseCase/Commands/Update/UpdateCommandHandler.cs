#pragma warning disable CS0649

using Karami.Common.ClassConsts;
using Karami.Core.Common.ClassConsts;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.Domain.Entities;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Extensions;
using Karami.Core.UseCase.Attributes;
using Karami.Domain.Role.Contracts.Interfaces;
using Karami.Domain.Role.Entities;

using Action = Karami.Core.Common.ClassConsts.Action;

namespace Karami.UseCase.RoleUseCase.Commands.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand, string>
{
    private readonly object _validationResult;

    private readonly IDotrisDateTime         _dotrisDateTime;
    private readonly ISerializer             _serializer;
    private readonly IJsonWebToken               _jsonWebToken;
    private readonly IRoleCommandRepository  _roleCommandRepository;
    private readonly IEventCommandRepository _eventCommandRepository;

    public UpdateCommandHandler(IRoleCommandRepository roleCommandRepository,
        IEventCommandRepository eventCommandRepository, 
        IDotrisDateTime dotrisDateTime, 
        ISerializer serializer, 
        IJsonWebToken jsonWebToken
    )
    {
        _dotrisDateTime         = dotrisDateTime;
        _serializer             = serializer;
        _jsonWebToken               = jsonWebToken;
        _roleCommandRepository  = roleCommandRepository;
        _eventCommandRepository = eventCommandRepository;
    }

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(UpdateCommand command, CancellationToken cancellationToken)
    {
        var targetRole = _validationResult as Role;

        targetRole.Change(_dotrisDateTime, command.Name);

        #region OutBox

        var events = targetRole.GetEvents.ToEntityOfEvent(_dotrisDateTime, _serializer, Service.UserService,
            Table.RoleTable, Action.Update, _jsonWebToken.GetUsername(command.Token)
        );

        foreach (Event @event in events)
            await _eventCommandRepository.AddAsync(@event, cancellationToken);
        
        targetRole.ClearEvents();

        #endregion

        _roleCommandRepository.Change(targetRole);

        return targetRole.Id;
    }
}