﻿using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Domic.Core.Common.ClassConsts;
using Domic.Domain.Commons.Contracts.Interfaces;
using Domic.Infrastructure.Implementations.Domain.Repositories.C;
using Domic.Infrastructure.Implementations.Domain.Repositories.Q;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using CommandSqlContext = Domic.Persistence.Contexts.C.SQLContext;
using QuerySqlContext   = Domic.Persistence.Contexts.Q.SQLContext;

namespace Presentation;

public class MyWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        System.Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", Environment.Testing);
        System.Environment.SetEnvironmentVariable("E-RabbitMQ-Host", "localhost");
        System.Environment.SetEnvironmentVariable("E-RabbitMQ-Port", "5672");
        System.Environment.SetEnvironmentVariable("E-RabbitMQ-Username", "guest");
        System.Environment.SetEnvironmentVariable("E-RabbitMQ-Password", "guest");
        System.Environment.SetEnvironmentVariable("I-RabbitMQ-Host", "localhost");
        System.Environment.SetEnvironmentVariable("I-RabbitMQ-Port", "5672");
        System.Environment.SetEnvironmentVariable("I-RabbitMQ-Username", "guest");
        System.Environment.SetEnvironmentVariable("I-RabbitMQ-Password", "guest");
        System.Environment.SetEnvironmentVariable("RedisConnectionString", "127.0.0.1:6379");

        builder.ConfigureServices(collection => {

            collection.Remove(new ServiceDescriptor(typeof(ICommandUnitOfWork), typeof(CommandUnitOfWork)));
            
            collection.Remove(new ServiceDescriptor(typeof(IQueryUnitOfWork), typeof(QueryUnitOfWork)));

            collection.AddScoped<ICommandUnitOfWork, IntegrationTestCommandUnitOfWork>();
            
            collection.AddScoped<IQueryUnitOfWork, IntegrationTestQueryUnitOfWork>();

        });
    }
    
    public class IntegrationTestCommandUnitOfWork : ICommandUnitOfWork
    {
        private readonly CommandSqlContext _context;

        public IntegrationTestCommandUnitOfWork(CommandSqlContext context) => _context = context;

        public void Transaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) {}

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Rollback() {}

        public void Dispose(){}

        public Task TransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public Task CommitAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task RollbackAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask DisposeAsync()
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class IntegrationTestQueryUnitOfWork : IQueryUnitOfWork
    {
        private readonly QuerySqlContext _context;

        public IntegrationTestQueryUnitOfWork(QuerySqlContext context) => _context = context;

        public void Transaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) {}

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Rollback() {}

        public void Dispose(){}

        public Task TransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public Task CommitAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task RollbackAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask DisposeAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}