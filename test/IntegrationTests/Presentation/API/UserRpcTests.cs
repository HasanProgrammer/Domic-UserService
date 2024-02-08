using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Grpc.Core;
using Karami.Core.Common.ClassConsts;
using Karami.Core.Grpc.User;
using Karami.Core.Infrastructure.Extensions;
using Karami.Domain.Role.Entities;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

using String     = Karami.Core.Grpc.User.String;
using Permission = Karami.Domain.Permission.Entities.Permission;

namespace Presentation.API;

public class UserRpcTests : IClassFixture<IntegrationTestBase>
{
    private readonly IntegrationTestBase _testBase;

    public UserRpcTests(IntegrationTestBase TestBase)
    {
        _testBase = TestBase;
    }

    [Theory]
    [InlineData("Hasan", "Karami",
        "4gXDlJRPguRND4qZ0dhk0LvZ1TqgYCY0fqvVtZJiCwjLCW3fOEm1HfSYZjzdkaRDhklxbRCz3uwuLKlJmGG89oDl61f0DBhEMsi3r",
        "HasanProgrammer", "Hasan@123@313@@", "09026676147", "hasankarami2020313@gmail.com", 
        new [] { "SuperAdmin"  },
        new [] { "User.Create" }
    )]
    public async Task ShouldBe_CreatedUser_WhenCallCreateMethodOfUserRpc(string firstName, string lastName, 
        string description, string username, string password, string phoneNumber, string email, string[] roles,
        string[] permissions
    )
    {
        //Arrange
        
        var commandSqlContext = _testBase.CommandSqlContext;

        var accessLevelData = await _initRolesAndPermissionsAsync(roles, permissions);

        Metadata metadata = new() {
            { "License", "36c160d95a09ace4f20f172938ccd3f9c7fe1a08a35605d112cf9fe3a56f964d" },
            { Header.Token, _testBase.GenerateToken() },
        };

        CreateRequest request = new() {
            FirstName   = new String { Value = firstName }   ,
            LastName    = new String { Value = lastName }    ,
            Description = new String { Value = description } ,
            Username    = new String { Value = username }    ,
            Password    = new String { Value = password }    ,
            PhoneNumber = new String { Value = phoneNumber } ,
            Email       = new String { Value = email }       ,
            Roles       = new String { Value = accessLevelData.roles.Serialize() } ,
            Permissions = new String { Value = accessLevelData.permissions.Serialize() }
        };

        var userGrpcClient = new UserService.UserServiceClient(_testBase.Channel);

        //Act

        var result = await userGrpcClient.CreateAsync(request, headers: metadata);
        
        var isCurrentUserAdded =
            await commandSqlContext.Users.AnyAsync(user => user.FirstName.Value == firstName);

        var allCurrentUserRolesAdded =
            await commandSqlContext.Users.AsNoTracking()
                                         .SelectMany(user =>
                                             user.RoleUsers.Select(roleUser => roleUser.Role.Name.Value)
                                         )
                                         .ToListAsync();

        var isCurrentUserRolesAdded = allCurrentUserRolesAdded.All(role => roles.Contains(role));

        var allCurrentUserPermissionsAdded =
            await commandSqlContext.Users.AsNoTracking()
                                         .SelectMany(user => 
                                             user.PermissionUsers.Select(permissionUser => 
                                                 permissionUser.Permission.Name.Value
                                            )
                                         )
                                         .ToListAsync();
        
        var isCurrentUserPermissionsAdded =
            allCurrentUserPermissionsAdded.All(permission => permissions.Contains(permission));
        
        //Assert

        result.ShouldNotBeNull();
        result.Code.Should().Be(201);
        
        isCurrentUserAdded.ShouldBeTrue();
        isCurrentUserRolesAdded.ShouldBeTrue();
        isCurrentUserPermissionsAdded.ShouldBeTrue();
    }

    /*---------------------------------------------------------------*/

    private async Task<(List<string> roles, List<string> permissions)> 
        _initRolesAndPermissionsAsync(string[] roles, string[] permissions)
    {
        List<string> roleIds       = new();
        List<string> permissionIds = new();

        foreach (var role in roles)
        {
            var guidRoleId = Guid.NewGuid().ToString();
            
            roleIds.Add(guidRoleId);
            
            _testBase.CommandSqlContext.Roles.Add(
                new Role(_testBase.DomicDateTime, "", "", guidRoleId, role)
            );

            foreach (var permission in permissions)
            {
                var guidPermissionId = Guid.NewGuid().ToString();
                
                permissionIds.Add(guidPermissionId);
                
                _testBase.CommandSqlContext.Permissions.Add(
                    new Permission(_testBase.DomicDateTime, guidPermissionId, "", "", permission, guidRoleId)
                );
            }
        }

        await _testBase.CommandSqlContext.SaveChangesAsync();
        
        return (roleIds, permissionIds);
    }
}