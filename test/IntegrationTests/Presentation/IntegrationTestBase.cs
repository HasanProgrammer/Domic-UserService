#pragma warning disable CS8604

using System;
using System.Net.Http;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Presentation;

public class IntegrationTestBase : IDisposable
{
    private readonly WebApplicationFactory<Program> _Factory;

    public readonly GrpcChannel Channel;

    public IntegrationTestBase()
    {
        _Factory = new WebApplicationFactory<Program>(); // In Memory Host

        HttpClient Client = _Factory.CreateDefaultClient();
        
        Channel = GrpcChannel.ForAddress(Client.BaseAddress, new GrpcChannelOptions {
            HttpClient = Client
        });
    }

    public void Dispose() => _Factory.Dispose();
}