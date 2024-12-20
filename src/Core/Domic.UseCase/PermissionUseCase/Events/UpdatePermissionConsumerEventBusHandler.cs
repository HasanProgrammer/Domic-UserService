﻿using Domic.Core.Common.ClassConsts;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.Permission.Contracts.Interfaces;
using Domic.Domain.Permission.Events;

namespace Domic.UseCase.PermissionUseCase.Events;

public class UpdatePermissionConsumerEventBusHandler : IConsumerEventBusHandler<PermissionUpdated>
{
    private readonly IPermissionQueryRepository _permissionQueryRepository;

    public UpdatePermissionConsumerEventBusHandler(IPermissionQueryRepository permissionQueryRepository) 
        =>  _permissionQueryRepository = permissionQueryRepository;

    public void BeforeHandle(PermissionUpdated @event){}

    [TransactionConfig(Type = TransactionType.Query)]
    public void Handle(PermissionUpdated @event)
    {
        var targetPermission = _permissionQueryRepository.FindByIdAsync(@event.Id, default).Result;

        targetPermission.Name   = @event.Name;
        targetPermission.RoleId = @event.RoleId;

        _permissionQueryRepository.Change(targetPermission);
    }

    public void AfterHandle(PermissionUpdated @event){}
}