using KisV4.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace KisV4.BL.EF;

public static class ServiceCollectionExtensions
{
    public static void AddEntityFrameworkBL(this IServiceCollection serviceCollection)
    {
        serviceCollection.Scan(scan => scan.FromCallingAssembly()
            .AddClasses(classes => classes.AssignableTo<IScopedService>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }
}