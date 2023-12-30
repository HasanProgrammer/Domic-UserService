using System.Data;
using Karami.Core.Common.ClassConsts;
using Karami.Domain.Commons.Contracts.Interfaces;
using Karami.Infrastructure.Implementations.Domain.Repositories.C;
using Karami.Infrastructure.Implementations.Domain.Repositories.Q;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using CommandSqlContext = Karami.Persistence.Contexts.C.SQLContext;
using QuerySqlContext   = Karami.Persistence.Contexts.Q.SQLContext;

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
    }
}