using System.Reflection;
using Application.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services, 
        IConfiguration configuration)
    {
        AddValidators(services);
    }

    private static void AddValidators(IServiceCollection services)
    {
        Assembly currentAssembly = typeof(DependencyInjection).Assembly;

        var validatorTypes = currentAssembly.DefinedTypes
            .Where(type => !type.IsAbstract && !type.IsInterface && // Non-abstract, non-interface classes
                           type.GetInterfaces().Any(i => i.IsGenericType && // Implements a generic interface
                                                        i.GetGenericTypeDefinition() == typeof(IValidator<,>)))
            .ToList();

        foreach (var type in validatorTypes)
        {
            // Find the specific IValidator<T, TId> interface implemented by this type
            var validatorInterface = type.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<,>));

            // Register the type as a singleton for the specific closed generic interface
            services.AddSingleton(validatorInterface, type);
        }
    }
}