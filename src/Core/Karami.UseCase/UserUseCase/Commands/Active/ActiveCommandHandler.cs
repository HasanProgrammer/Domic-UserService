#pragma warning disable CS0649

using Karami.Common.ClassConsts;
using Karami.Core.Common.ClassConsts;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.Domain.Entities;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Extensions;
using Karami.Core.UseCase.Attributes;
using Karami.Domain.User.Contracts.Interfaces;
using Karami.Domain.User.Entities;

using Action = Karami.Core.Common.ClassConsts.Action;

namespace Karami.UseCase.UserUseCase.Commands.Active;

public class ActiveCommandHandler : ICommandHandler<ActiveCommand, string>
{
    private readonly object _validationResult;

    private readonly IDateTime               _dateTime;
    private readonly ISerializer             _serializer;
    private readonly IJsonWebToken           _jsonWebToken;
    private readonly IUserCommandRepository  _userCommandRepository;
    private readonly IEventCommandRepository _eventCommandRepository;

    public ActiveCommandHandler(IUserCommandRepository userCommandRepository, 
        IEventCommandRepository eventCommandRepository, IDateTime dateTime, ISerializer serializer, 
        IJsonWebToken jsonWebToken
    )
    {
        _dateTime               = dateTime;
        _serializer             = serializer;
        _jsonWebToken           = jsonWebToken;
        _userCommandRepository  = userCommandRepository;
        _eventCommandRepository = eventCommandRepository;
    }

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(ActiveCommand command, CancellationToken cancellationToken)
    {
        var targetUser  = _validationResult as User;
        var updatedBy   = _jsonWebToken.GetIdentityUserId(command.Token);
        var updatedRole = _serializer.Serialize( _jsonWebToken.GetRoles(command.Token) );
        
        targetUser.Active(_dateTime, updatedBy, updatedRole, _jsonWebToken.GetUsername(command.Token));

        _userCommandRepository.Change(targetUser);

        #region OutBox

        var events = targetUser.GetEvents.ToEntityOfEvent(_dateTime, _serializer, Service.UserService,
            Table.UserTable, Action.Update, _jsonWebToken.GetUsername(command.Token)
        );

        foreach (Event @event in events)
            await _eventCommandRepository.AddAsync(@event, cancellationToken);
        
        targetUser.ClearEvents();

        #endregion

        return targetUser.Id;
    }
}