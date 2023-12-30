#pragma warning disable CS8604

using System;
using System.Collections.Generic;
using System.Security.Claims;
using Grpc.Net.Client;
using Karami.Core.Domain.Contracts.Interfaces;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.UseCase.DTOs;
using Microsoft.Extensions.DependencyInjection;

using CommandSqlContext = Karami.Persistence.Contexts.C.SQLContext;
using QuerySqlContext   = Karami.Persistence.Contexts.Q.SQLContext;

namespace Presentation;

public class IntegrationTestBase : IDisposable
{
    public readonly GrpcChannel       Channel;
    public readonly IDotrisDateTime   DomicDateTime;
    public readonly CommandSqlContext CommandSqlContext;
    public readonly QuerySqlContext   QuerySqlContext;

    private readonly MyWebApplicationFactory<Program> _applicationFactory;

    public IntegrationTestBase()
    {
        _applicationFactory = new MyWebApplicationFactory<Program>();

        Channel = GrpcChannel.ForAddress(_applicationFactory.Server.BaseAddress, new GrpcChannelOptions {
            HttpHandler = _applicationFactory.Server.CreateHandler()
        });

        DomicDateTime     = _applicationFactory.Services.GetRequiredService<IDotrisDateTime>();
        CommandSqlContext = _applicationFactory.Services.GetRequiredService<CommandSqlContext>();
        QuerySqlContext   = _applicationFactory.Services.GetRequiredService<QuerySqlContext>();
    }

    public string GenerateToken()
    {
        var jwtToken = _applicationFactory.Services.GetRequiredService<IJsonWebToken>();
        
        var secretPayload = new TokenParameterDto {
            Key      = "19961375001375199600313",
            Issuer   = "Dotris",
            Audience = "Dotris",
            Expires  = 60
        };
        
        var claims = new List<KeyValuePair<string, string>> {
            new("UserId", Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, "HasanDeveloper"),
            new(ClaimTypes.Role, "SuperAdmin"),
            new("Permission", "User.Create")
        };
        
        return jwtToken.Generate(secretPayload, claims.ToArray());
    }

    public void Dispose() => _applicationFactory.Dispose();
}