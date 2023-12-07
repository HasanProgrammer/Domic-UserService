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

namespace Karami.UseCase.UserUseCase.Commands.InActive;

public class InActiveCommandHandler : ICommandHandler<InActiveCommand, string>
{
    private readonly object _validationResult;

    private readonly IDotrisDateTime         _dotrisDateTime;
    private readonly ISerializer             _serializer;
    private readonly IJsonWebToken           _jsonWebToken;
    private readonly IUserCommandRepository  _userCommandRepository;
    private readonly IEventCommandRepository _eventCommandRepository;

    public InActiveCommandHandler(IUserCommandRepository userCommandRepository,
        IEventCommandRepository eventCommandRepository, 
        IDotrisDateTime dotrisDateTime, 
        ISerializer serializer, 
        IJsonWebToken jsonWebToken
    )
    {
        _dotrisDateTime         = dotrisDateTime;
        _serializer             = serializer;
        _jsonWebToken           = jsonWebToken;
        _userCommandRepository  = userCommandRepository;
        _eventCommandRepository = eventCommandRepository;
    }

    [WithValidation]
    [WithTransaction]
    public async Task<string> HandleAsync(InActiveCommand command, CancellationToken cancellationToken)
    {
        var targetUser = _validationResult as User;
        
        targetUser.InActive(_dotrisDateTime, _jsonWebToken.GetUsername(command.Token));

        _userCommandRepository.Change(targetUser);

        #region OutBox

        var events = targetUser.GetEvents.ToEntityOfEvent(_dotrisDateTime, _serializer, Service.UserService,
            Table.UserTable, Action.Update, _jsonWebToken.GetUsername(command.Token)
        );

        foreach (Event @event in events)
            await _eventCommandRepository.AddAsync(@event, cancellationToken);
        
        targetUser.ClearEvents();

        #endregion

        return targetUser.Id;
    }
}