using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KisV4.DAL.EF;

public static class ServiceCollectionExtensions {
    public static void AddEntityFrameworkDAL(this IServiceCollection serviceCollection,
        string connectionString) {
        serviceCollection.AddDbContext<KisDbContext>(options =>
            options.UseNpgsql(connectionString));

    }
}
