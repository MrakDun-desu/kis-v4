using KisV4.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace KisV4.BL.EF;

public static class ServiceCollectionExtensions {
    public static void AddEntityFrameworkBL(this IServiceCollection serviceCollection) {
        serviceCollection.Scan(static scan => scan.FromAssembliesOf(typeof(Mapper))
            .AddClasses(static classes => classes.AssignableTo<IScopedService>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }
}
