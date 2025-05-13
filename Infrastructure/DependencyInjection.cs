using System.Reflection;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.ProcessingServices;
using Contacts.Infrastructure.Handlers;
using Contacts.Infrastructure.Persistance;
using Contacts.Infrastructure.ProcessingServices;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        Assembly currentAssembly = typeof(DependencyInjection).Assembly;
        services.AddDbContext<AppDbContext>();

        services.AddRepositories(currentAssembly);
        services.AddFoundationServices(currentAssembly);

        AddHttpClients(services);
        AddHandlers(services);
        AddProccessingServices(services);

        services.AddMemoryCache();
    }

    private static void AddProccessingServices(IServiceCollection services)
    {
        services.AddSingleton<IPasswordHashingService, PasswordHashingService>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
    }

    private static void AddHttpClients(IServiceCollection services)
    {
        services.ConfigureHttpClientDefaults(builder => builder.AddStandardResilienceHandler());
        
        services.AddHttpClient<IHrmClient, HrmClient>("hrm-http-client");

        services.AddHttpClient<IHrmProClient, HrmProClient>("hrm-pro-http-client", config => {
            config.DefaultRequestHeaders.Add("User-Agent", "ContactsApp/1.0");
        });
    }

    private static void AddHandlers(IServiceCollection services)
    {
        services.AddScoped<IPhoneNumberHandler, PhoneNumberHandler>();
        services.AddScoped<IUserHandler, UserHandler>();
        services.AddScoped<IAccountHandler, AccountHandler>();
        services.AddScoped<IAuthHandler, AuthHandler>();
        services.AddScoped<IHrmHandler, HrmHandler>();
    }
}