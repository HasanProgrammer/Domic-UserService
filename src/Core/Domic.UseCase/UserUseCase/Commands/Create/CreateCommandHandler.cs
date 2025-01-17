#pragma warning disable CS4014

using Domic.Common.ClassConsts;
using Domic.Core.Domain.Contracts.Interfaces;
using Domic.Core.UseCase.Attributes;
using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Domain.PermissionUser.Entities;
using Domic.Domain.RoleUser.Entities;
using Domic.Domain.User.Entities;
using Domic.Domain.PermissionUser.Contracts.Interfaces;
using Domic.Domain.RoleUser.Contracts.Interfaces;
using Domic.Domain.User.Contracts.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Domic.UseCase.UserUseCase.Commands.Create;

public class CreateCommandHandler : ICommandHandler<CreateCommand, string>
{
    private readonly IDateTime                        _dateTime;
    private readonly ISerializer                      _serializer;
    private readonly IUserCommandRepository           _userCommandRepository;
    private readonly IRoleUserCommandRepository       _roleUserCommandRepository;
    private readonly IPermissionUserCommandRepository _permissionUserCommandRepository;
    private readonly IGlobalUniqueIdGenerator         _globalUniqueIdGenerator;
    private readonly IIdentityUser                    _identityUser;

    public CreateCommandHandler(IUserCommandRepository userCommandRepository,
        IRoleUserCommandRepository roleUserCommandRepository, 
        IPermissionUserCommandRepository permissionUserCommandRepository, IDateTime dateTime, ISerializer serializer,
        IGlobalUniqueIdGenerator globalUniqueIdGenerator, [FromKeyedServices("Http2")] IIdentityUser identityUser
    )
    {
        _dateTime                        = dateTime;
        _serializer                      = serializer;
        _userCommandRepository           = userCommandRepository;
        _roleUserCommandRepository       = roleUserCommandRepository;
        _permissionUserCommandRepository = permissionUserCommandRepository;
        _globalUniqueIdGenerator         = globalUniqueIdGenerator;
        _identityUser                    = identityUser;
    }

    public Task BeforeHandleAsync(CreateCommand command, CancellationToken cancellationToken) => Task.CompletedTask;

    [WithValidation]
    [WithTransaction]
    [WithCleanCache(Keies = RedisCache.AllUsers)]
    public async Task<string> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        var newUser = new User(
            _dateTime                ,
            _identityUser            ,
            _serializer              ,
            _globalUniqueIdGenerator ,
            command.FirstName        ,
            command.LastName         ,
            command.Description      ,
            command.Username         ,
            command.Password         ,
            command.PhoneNumber      ,
            command.EMail            ,
            command.Roles            ,
            command.Permissions
        );
        
        var roleUsers = command.Roles.Select(role => new RoleUser(
            _globalUniqueIdGenerator, _dateTime, _identityUser, _serializer, newUser.Id, role
        ));
        
        var permissionUsers = command.Permissions.Select(permission => new PermissionUser(
            _globalUniqueIdGenerator, _dateTime, _identityUser, _serializer, newUser.Id, permission
        ));
        
        await _userCommandRepository.AddAsync(newUser, cancellationToken);
        await _roleUserCommandRepository.AddRangeAsync(roleUsers, cancellationToken);
        await _permissionUserCommandRepository.AddRangeAsync(permissionUsers, cancellationToken);
        
        return newUser.Id;
    }

    public Task AfterHandleAsync(CreateCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;
}