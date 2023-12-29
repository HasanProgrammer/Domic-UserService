#pragma warning disable CS8604

using System;
using System.Collections.Generic;
using System.Security.Claims;
using Grpc.Net.Client;
using Karami.Core.UseCase.Contracts.Interfaces;
using Karami.Core.UseCase.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using Environment = Karami.Core.Common.ClassConsts.Environment;

namespace Presentation;

public class IntegrationTestBase : IDisposable
{
    public readonly GrpcChannel Channel;

    private readonly WebApplicationFactory<Program> _applicationFactory;

    public IntegrationTestBase()
    {
        // In Memory Host
        _applicationFactory =
            new WebApplicationFactory<Program>().WithWebHostBuilder(builder => 
                builder.UseEnvironment(Environment.Testing)
            );

        var client = _applicationFactory.CreateDefaultClient();

        Channel = GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions { HttpClient = client });
    }

    public string GenerateToken()
    {
        var jwtToken = _applicationFactory.Services.GetRequiredService<IJsonWebToken>();
        
        var secretPayload = new TokenParameterDto {
            Key      = "1996_1375_1996",
            Issuer   = "Dotris",
            Audience = "Dotris",
            Expires  = 60
        };
        
        var claims = new List<KeyValuePair<string, string>> {
            new("UserId", ""),
            new(ClaimTypes.Name, ""),
            new(ClaimTypes.Role, ""),
            new(ClaimTypes.Role, ""),
            new(ClaimTypes.Role, ""),
            new("Permission", ""),
            new("Permission", ""),
            new("Permission", ""),
        };
        
        return jwtToken.Generate(secretPayload, claims.ToArray());
    }

    public void Dispose() => _applicationFactory.Dispose();
}