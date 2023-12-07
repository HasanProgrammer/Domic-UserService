namespace Karami.WebAPI.Frameworks.Extensions;

public static class IConfigurationBuilderExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="configurationBuilder"></param>
    /// <param name="hostEnvironment"></param>
    /// <returns></returns>
    public static IConfigurationBuilder AddJsonFiles(this IConfigurationBuilder configurationBuilder,
        IHostEnvironment hostEnvironment
    )
    {
        var service    = Path.Combine(hostEnvironment.ContentRootPath, "Configs", "Service.json");
        var jsonResult = Path.Combine(hostEnvironment.ContentRootPath, "Configs", "JsonResult.json");
        var license    = Path.Combine(hostEnvironment.ContentRootPath, "Configs", "License.json");
        var webService = Path.Combine(hostEnvironment.ContentRootPath, "Configs", "WebService.json");
        
        configurationBuilder.AddJsonFile(jsonResult , optional: true, reloadOnChange: true)
                            .AddJsonFile(license    , optional: true, reloadOnChange: true)
                            .AddJsonFile(webService , optional: true, reloadOnChange: true)
                            .AddJsonFile(service    , optional: true, reloadOnChange: true);

        return configurationBuilder;
    }
}