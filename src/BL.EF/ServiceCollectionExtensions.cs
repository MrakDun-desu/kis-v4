using FluentValidation;
using KisV4.Common.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace KisV4.BL.EF;

public static class ServiceCollectionExtensions {
    public static void AddEntityFrameworkBL(this IServiceCollection serviceCollection) {
        serviceCollection.Scan(static scan => scan.FromAssembliesOf(typeof(IAssemblyMarker))
            // services
            .AddClasses(static classes => classes.AssignableTo<IScopedService>())
            .AsImplementedInterfaces()
            .AsSelf()
            .WithScopedLifetime()
            // validators
            .AddClasses(classes => classes.AssignableTo<IValidator>())
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            // authorization handlers
            .AddClasses(static classes => classes.AssignableTo<IAuthorizationHandler>())
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            // authorization requirements
            .AddClasses(static classes => classes.AssignableTo<IAuthorizationRequirement>())
            .AsSelf()
            .WithSingletonLifetime()
        );
    }
}
