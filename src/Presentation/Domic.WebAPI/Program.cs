using Domic.Core.Infrastructure.Extensions;
using Domic.Core.WebAPI.Extensions;
using Domic.Infrastructure.Extensions.C;
using Domic.WebAPI.EntryPoints.GRPCs;
using Domic.WebAPI.Frameworks.Extensions;

using C_SQLContext = Domic.Persistence.Contexts.C.SQLContext;
using Q_SQLContext = Domic.Persistence.Contexts.Q.SQLContext;

/*-------------------------------------------------------------------*/

WebApplicationBuilder builder = WebApplication.CreateBuilder();

#region Configs

builder.WebHost.ConfigureAppConfiguration((context, builder) => builder.AddJsonFiles(context.HostingEnvironment));

#endregion

/*-------------------------------------------------------------------*/

#region ServiceContainer

builder.RegisterHelpers();
builder.RegisterELK();
builder.RegisterEntityFrameworkCoreCommand<C_SQLContext, string>();
builder.RegisterEntityFrameworkCoreQuery<Q_SQLContext>();
builder.RegisterCommandRepositories();
builder.RegisterQueryRepositories();
builder.RegisterCommandQueryUseCases();
builder.RegisterDistributedCaching();
builder.RegisterGrpcServer();
builder.RegisterMessageBroker();
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
application.Services.AutoMigration<Q_SQLContext>();

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