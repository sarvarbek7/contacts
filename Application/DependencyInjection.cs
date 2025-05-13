using System.Reflection;
using Application;
using Application.Validators;
using Contacts.Application.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services, 
        IConfiguration configuration)
    {
        Assembly currentAssembly = typeof(DependencyInjection).Assembly;

        services.AddValidators(currentAssembly);

        ConfigureOptions(services, configuration);
    }

    private static void ConfigureOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<List<HttpConfiguration>>(configuration.GetSection(HttpConfiguration.SectionName));
    }
}
