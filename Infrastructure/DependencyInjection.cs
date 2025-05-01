using System.Net;
using System.Reflection;
using Application.Services.Foundations;
using Contacts.Infrastructure.Persistance;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>();

        AddRepositories(services);
        AddFoundationServices(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        Assembly currentAssembly = typeof(DependencyInjection).Assembly;

        var repositories = currentAssembly.DefinedTypes
            .Where(type => !type.IsAbstract && !type.IsInterface && // Non-abstract, non-interface classes
                           type.GetInterfaces().Any(i => i.IsGenericType && // Implements a generic interface
                                                        i.GetGenericTypeDefinition() == typeof(IRepository<,>)))
            .ToList();

        foreach (var type in repositories)
        {
            // Find the specific IValidator<T, TId> interface implemented by this type
            var repositoryInterface = type.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<,>));

            // Register the type as a singleton for the specific closed generic interface
            services.AddScoped(repositoryInterface, type);
        }
    }

    private static void AddFoundationServices(IServiceCollection services)
    {
        Assembly currentAssembly = typeof(DependencyInjection).Assembly;

        var foundationServices = currentAssembly.DefinedTypes
            .Where(type => !type.IsAbstract && !type.IsInterface && // Non-abstract, non-interface classes
                           type.GetInterfaces().Any(i => i.IsGenericType && // Implements a generic interface
                                                        i.GetGenericTypeDefinition() == typeof(IBaseService<,>)))
            .ToList();

        foreach (var type in foundationServices)
        {
            // Find the specific IValidator<T, TId> interface implemented by this type
            var foundationServicesInterface = type.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseService<,>));

            // Register the type as a singleton for the specific closed generic interface
            services.AddScoped(foundationServicesInterface, type);
        }
    }
}