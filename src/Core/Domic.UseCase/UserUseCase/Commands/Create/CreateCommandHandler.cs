#pragma warning disable CS4014

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
        IGlobalUniqueIdGenerator globalUniqueIdGenerator, [FromKeyedServices("http1")] IIdentityUser identityUser
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
    public async Task<string> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        string userId = _globalUniqueIdGenerator.GetRandom();
        
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
        
        await _userCommandRepository.AddAsync(newUser, cancellationToken);
        
        await _roleUserBuilderAsync(userId, command.Roles, cancellationToken);
        await _permissionUserBuilderAsync(userId, command.Permissions, cancellationToken);

        return userId;
    }

    public Task AfterHandleAsync(CreateCommand command, CancellationToken cancellationToken)
        => Task.CompletedTask;

    /*---------------------------------------------------------------*/

    private async Task _roleUserBuilderAsync(string userId, IEnumerable<string> roleIds, 
        CancellationToken cancellationToken
    )
    {
        foreach (var roleId in roleIds)
        {
            var newRoleUser = new RoleUser(
                _globalUniqueIdGenerator, _dateTime, _identityUser, _serializer, userId, roleId
            );

            await _roleUserCommandRepository.AddAsync(newRoleUser, cancellationToken);
        }
    }
    
    private async Task _permissionUserBuilderAsync(string userId, IEnumerable<string> permissionIds, 
        CancellationToken cancellationToken
    )
    {
        foreach (var permissionId in permissionIds)
        {
            var newPermissionUser = new PermissionUser(
                _globalUniqueIdGenerator, _dateTime, _identityUser, _serializer, userId, permissionId
            );

            await _permissionUserCommandRepository.AddAsync(newPermissionUser, cancellationToken);
        }
    }
}