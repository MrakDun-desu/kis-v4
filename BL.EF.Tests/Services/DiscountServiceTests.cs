using BL.EF.Tests.Extensions;
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

    [Fact]
    public void Read_ReturnsNotFound_WhenNotFound()
    {
        var readResult = _cashBoxService.Read(42);

        readResult.Should().BeNotFound();
    }

    [Fact]
    public void Read_ReadsCorrectly_WhenSimple()
    {
        var testDiscount = new DiscountEntity { Name = "Test discount box" };
        var insertedEntity = _dbContext.Discounts.Add(testDiscount);
        _dbContext.SaveChanges();
        var id = insertedEntity.Entity.Id;

        var readResult = _cashBoxService.Read(id);

        var expectedModel = insertedEntity.Entity.ToModel();
        readResult.Should().HaveValue(expectedModel);
    }

    [Fact]
    public void Read_ReadsCorrectly_WhenComplex()
    {
        var testDiscount = new DiscountEntity
        {
            Name = "Test discount box",
            DiscountUsages =
            {
                new DiscountUsageEntity
                {
                    Timestamp = DateTimeOffset.UtcNow,
                    User = new UserAccountEntity
                    {
                        UserName = "Some user"
                    }
                },
                new DiscountUsageEntity
                {
                    Timestamp = DateTimeOffset.UtcNow.AddDays(1),
                    User = new UserAccountEntity
                    {
                        UserName = "Some other user"
                    }
                }
            }
        };
        _dbContext.Discounts.Add(testDiscount);
        _dbContext.SaveChanges();

        var readResult = _cashBoxService.Read(testDiscount.Id);

        var expectedModel = testDiscount.ToModel();
        readResult.Should().HaveValue(expectedModel);
    }
}