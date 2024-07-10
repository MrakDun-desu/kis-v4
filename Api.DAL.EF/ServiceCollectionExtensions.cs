using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.DAL.EF;

public static class ServiceCollectionExtensions {
    public static void AddEntityFrameworkDAL(this IServiceCollection serviceCollection,
        string connectionString) {
        serviceCollection.AddDbContext<KisDbContext>(options =>
            options.UseNpgsql(connectionString));

    }
}
