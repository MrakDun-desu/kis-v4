using KisV4.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace BL.EF.Tests.Fixtures;

public class KisDbContextFactory : IAsyncLifetime {
    private readonly PostgreSqlContainer _databaseContainer = new PostgreSqlBuilder("postgres:18")
        .WithUsername("postgres")
        .WithPassword("B6MchiUp69z")
        .WithDatabase("kis_v4")
        .Build();

    public async Task InitializeAsync() {
        await _databaseContainer.StartAsync();
    }

    public async Task DisposeAsync() {
        await _databaseContainer.DisposeAsync();
    }

    public (KisDbContext, KisDbContext) CreateDbContextAndReference() {
        var optionsBuilder = new DbContextOptionsBuilder<KisDbContext>()
            .UseNpgsql(_databaseContainer.GetConnectionString());
        var refContext = new KisDbContext(optionsBuilder.Options);
        // delete and create database between every dbcontext creation so the test cases are perfectly isolated from each other
        refContext.Database.EnsureDeleted();
        refContext.Database.EnsureCreated();

        var normalContext = new KisDbContext(optionsBuilder.Options);

        return (refContext, normalContext);
    }
}
