using KisV4.Api.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Api.BL.EF;

public static class ServiceCollectionExtensions {
    public static void AddEntityFrameworkBL(this IServiceCollection serviceCollection) {
        serviceCollection.AddSingleton<Mapper>();
        serviceCollection.Scan(scan => scan.FromAssemblyOf<Mapper>()
            .AddClasses(classes => classes.AssignableTo<IScopedService>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }
}
