using KisV4.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace BL.EF.Tests.Fixtures;

public class KisDbContextFactory : IDbContextFactory<KisDbContext>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _databaseContainer = new PostgreSqlBuilder()
        .WithUsername("postgres")
        .WithPassword("B6MchiUp69z")
        .WithDatabase("kis_v4")
        .Build();

    public async Task InitializeAsync()
    {
        await _databaseContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _databaseContainer.StopAsync();
    }

    public KisDbContext CreateDbContextAndResetDb()
    {
        var dbContext = CreateDbContext();
        // delete and create database between every dbcontext creation so the test cases are perfectly isolated from each other
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        return dbContext;
    }
    
    public KisDbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<KisDbContext>();
        optionsBuilder.UseNpgsql(_databaseContainer.GetConnectionString());
        var dbContext = new KisDbContext(optionsBuilder.Options);
        return dbContext;
    }
}