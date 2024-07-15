using KisV4.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace KisV4.BL.EF;

public static class ServiceCollectionExtensions {
    public static void AddEntityFrameworkBL(this IServiceCollection serviceCollection) {
        serviceCollection.AddSingleton<Mapper>();
        serviceCollection.Scan(scan => scan.FromAssemblyOf<Mapper>()
            .AddClasses(classes => classes.AssignableTo<IScopedService>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }
}
