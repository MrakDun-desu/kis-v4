using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace BL.EF.Tests.Services;

public class
    DiscountServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable
{
    private readonly DiscountService _cashBoxService;
    private readonly KisDbContext _dbContext;

    public DiscountServiceTests(KisDbContextFactory dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
        _cashBoxService = new DiscountService(_dbContext);
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    [Fact]
    public void ReadAll_ReadsAll()
    {
        var testDiscount1 = new DiscountEntity { Name = "Some discount box" };
        var testDiscount2 = new DiscountEntity { Name = "Some discount box 2" };
        _dbContext.Discounts.Add(testDiscount1);
        _dbContext.Discounts.Add(testDiscount2);
        _dbContext.SaveChanges();

        var readModels = _cashBoxService.ReadAll();
        var mappedModels =
            _dbContext.Discounts.ToList().ToModels();

        readModels.Should().BeEquivalentTo(mappedModels);
    }
}