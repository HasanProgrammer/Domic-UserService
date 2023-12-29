#pragma warning disable CS8604

using System;
using Grpc.Net.Client;
using Karami.Core.UseCase.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation;

public class IntegrationTestBase : IDisposable
{
    public readonly GrpcChannel Channel;

    public readonly IJsonWebToken JsonWebToken;
    
    private readonly WebApplicationFactory<Program> _applicationFactory;

    public IntegrationTestBase()
    {
        _applicationFactory = new WebApplicationFactory<Program>(); // In Memory Host
        
        var client = _applicationFactory.CreateDefaultClient();

        Channel = GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions { HttpClient = client });

        JsonWebToken = _applicationFactory.Services.GetRequiredService<IJsonWebToken>();
    }

    public void Dispose() => _applicationFactory.Dispose();
}