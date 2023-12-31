using Karami.Core.Infrastructure.Extensions;
using Karami.Core.WebAPI.Extensions;
using Karami.Infrastructure.Extensions.C;
using Karami.Infrastructure.Extensions.Q;
using Karami.WebAPI.EntryPoints.GRPCs;
using Karami.WebAPI.Frameworks.Extensions;

using C_SQLContext = Karami.Persistence.Contexts.C.SQLContext;
using Q_SQLContext = Karami.Persistence.Contexts.Q.SQLContext;

/*-------------------------------------------------------------------*/

WebApplicationBuilder builder = WebApplication.CreateBuilder();

#region Configs

builder.WebHost.ConfigureAppConfiguration((context, builder) => builder.AddJsonFiles(context.HostingEnvironment));

#endregion

/*-------------------------------------------------------------------*/

#region Service Container

builder.RegisterHelpers();
builder.RegisterCommandSqlServer<C_SQLContext>();
builder.RegisterQuerySqlServer<Q_SQLContext>();
builder.RegisterCommandRepositories();
builder.RegisterQueryRepositories();
builder.RegisterCommandQueryUseCases();
builder.RegisterMessageBroker();
builder.RegisterCaching();
builder.RegisterGrpcServer();
builder.RegisterEventsPublisher();
builder.RegisterEventsSubscriber();
builder.RegisterAsyncCommandsSubscriber();
builder.RegisterServices();

builder.Services.AddMvc();

#endregion

/*-------------------------------------------------------------------*/

WebApplication application = builder.Build();

/*-------------------------------------------------------------------*/

//Primary processing

application.Services.AutoMigration<C_SQLContext>(context => context.Seed());
application.Services.AutoMigration<Q_SQLContext>(context => context.Seed());

/*-------------------------------------------------------------------*/

#region Middleware

if (application.Environment.IsProduction())
{
    application.UseHsts();
    application.UseHttpsRedirection();
}

application.UseRouting();

application.UseEndpoints(endpoints => {
    
    endpoints.HealthCheck(application.Services);

    #region GRPC's Services

    endpoints.MapGrpcService<UserRPC>();
    endpoints.MapGrpcService<RoleRPC>();
    endpoints.MapGrpcService<PermissionRPC>();

    #endregion

});

#endregion

/*-------------------------------------------------------------------*/

//HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();

application.Run();

/*-------------------------------------------------------------------*/

//For Integration Test

public partial class Program {}