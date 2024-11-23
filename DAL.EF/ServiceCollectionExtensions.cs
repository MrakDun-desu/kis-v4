using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KisV4.DAL.EF;

public static class ServiceCollectionExtensions
{
    public static void AddEntityFrameworkDAL(this IServiceCollection serviceCollection,
        string connectionString)
    {
        serviceCollection.AddDbContext<KisDbContext>(options =>
            options
                .UseLazyLoadingProxies()
                .UseNpgsql(connectionString)
            );
        // using lazy loading to ease the development. Can switch to eager with .Include() calls
        // for critical sections of the code
    }
}