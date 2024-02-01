﻿using System.Data;
using Karami.Core.Common.ClassConsts;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.Domain.Enumerations;
using Karami.Core.Domain.Extensions;
using Karami.Core.UseCase.Attributes;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Domain.PermissionUser.Contracts.Interfaces;
using Karami.Domain.PermissionUser.Entities;
using Karami.Domain.RoleUser.Contracts.Interfaces;
using Karami.Domain.RoleUser.Entities;
using Karami.Domain.User.Contracts.Interfaces;
using Karami.Domain.User.Events;

namespace Karami.UseCase.UserUseCase.Events;

public class UpdateUserConsumerEventBusHandler : IConsumerEventBusHandler<UserUpdated>
{
    private readonly IUserQueryRepository           _userQueryRepository;
    private readonly IRoleUserQueryRepository       _roleUserQueryRepository;
    private readonly IPermissionUserQueryRepository _permissionUserQueryRepository;
    private readonly IGlobalUniqueIdGenerator       _globalUniqueIdGenerator;

    public UpdateUserConsumerEventBusHandler(IUserQueryRepository userQueryRepository, 
        IRoleUserQueryRepository roleUserQueryRepository, IPermissionUserQueryRepository permissionUserQueryRepository, 
        IGlobalUniqueIdGenerator globalUniqueIdGenerator
    )
    {
        _userQueryRepository           = userQueryRepository;
        _roleUserQueryRepository       = roleUserQueryRepository;
        _permissionUserQueryRepository = permissionUserQueryRepository;
        _globalUniqueIdGenerator       = globalUniqueIdGenerator;
    }

    [WithTransaction(IsolationLevel = IsolationLevel.ReadUncommitted)]
    [WithCleanCache(Keies = Cache.Users)]
    public void Handle(UserUpdated @event)
    {
        var targetUser = _userQueryRepository.FindByIdAsync(@event.Id, default).Result;

        targetUser.FirstName             = @event.FirstName;
        targetUser.LastName              = @event.LastName;
        targetUser.Username              = @event.Username;
        targetUser.Description           = @event.Description;
        targetUser.PhoneNumber           = @event.PhoneNumber;
        targetUser.Email                 = @event.Email;
        targetUser.IsActive              = @event.IsActive ? IsActive.Active : IsActive.InActive;
        targetUser.UpdatedBy             = @event.UpdatedBy;
        targetUser.UpdatedRole           = @event.UpdatedRole;
        targetUser.UpdatedAt_EnglishDate = @event.UpdatedAt_EnglishDate;
        targetUser.UpdatedAt_PersianDate = @event.UpdatedAt_PersianDate;
                
        if(targetUser.Password is not null)
            targetUser.Password = @event.Password.HashAsync(default).Result;
                    
        _userQueryRepository.Change(targetUser);
                    
        _roleUserBuilder(@event.UpdatedBy, @event.UpdatedRole, 
            @event.UpdatedAt_EnglishDate, @event.UpdatedAt_PersianDate, targetUser.Id, @event.Roles
        );
        
        _permissionUserBuilder(@event.UpdatedBy, @event.UpdatedRole, 
            @event.UpdatedAt_EnglishDate, @event.UpdatedAt_PersianDate, targetUser.Id, @event.Permissions
        );
    }
    
    /*---------------------------------------------------------------*/
    
    private void _roleUserBuilder(string createdBy, string createdRole, DateTime englishCreatedAt, 
        string persianCreatedAt, string userId, IEnumerable<string> roleIds
    )
    {
        var roleUsers =
            _roleUserQueryRepository.FindAllByUserIdAsync(userId, default).Result;
        
        //1 . Remove already user roles
        _roleUserQueryRepository.RemoveRange(roleUsers);
        
        //2 . Assign new roles for user
        foreach (var roleId in roleIds)
        {
            var newRoleUser = new RoleUserQuery {
                Id          = _globalUniqueIdGenerator.GetRandom(),
                UserId      = userId,
                RoleId      = roleId,
                CreatedBy   = createdBy,
                CreatedRole = createdRole,
                CreatedAt_EnglishDate = englishCreatedAt,
                CreatedAt_PersianDate = persianCreatedAt
            };

            _roleUserQueryRepository.Add(newRoleUser);
        }
    }
    
    private void _permissionUserBuilder(string createdBy, string createdRole, DateTime englishCreatedAt, 
        string persianCreatedAt, string userId, IEnumerable<string> permissionIds
    )
    {
        var permissionUsers =
            _roleUserQueryRepository.FindAllByUserIdAsync(userId, default).Result;
        
        //1 . Remove already user permissions
        _permissionUserQueryRepository.RemoveRange(permissionUsers);
        
        //2 . Assign new permissions for user
        foreach (var permissionId in permissionIds)
        {
            var newPermissionUser = new PermissionUserQuery {
                Id           = _globalUniqueIdGenerator.GetRandom(),
                UserId       = userId,
                PermissionId = permissionId,
                CreatedBy    = createdBy,
                CreatedRole  = createdRole,
                CreatedAt_EnglishDate = englishCreatedAt,
                CreatedAt_PersianDate = persianCreatedAt
            };

            _permissionUserQueryRepository.Add(newPermissionUser);
        }
    }
}