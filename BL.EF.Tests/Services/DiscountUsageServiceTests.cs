using BL.EF.Tests.Extensions;
using BL.EF.Tests.Fixtures;
using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace BL.EF.Tests.Services;

public class DiscountUsageServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable
{
    private readonly DiscountUsageService _discountUsageService;
    private readonly KisDbContext _referenceDbContext;
    private readonly KisDbContext _normalDbContext;

    public DiscountUsageServiceTests(KisDbContextFactory dbContextFactory)
    {
        (_referenceDbContext, _normalDbContext) = dbContextFactory.CreateDbContextAndReference();
        _discountUsageService = new DiscountUsageService(_normalDbContext);
    }

    public async ValueTask DisposeAsync()
    {
        await _referenceDbContext.DisposeAsync();
        await _normalDbContext.DisposeAsync();
    }

    public void Dispose()
    {
        _referenceDbContext.Dispose();
        _normalDbContext.Dispose();
    }

    [Fact]
    public void ReadAll_ReadsAll_WhenNoFilters()
    {
        // arrange
        var testDiscount1 = new DiscountEntity { Name = "Some discount" };
        var testUser1 = new UserAccountEntity { UserName = "Some user" };
        var testDiscount2 = new DiscountEntity { Name = "Some other discount" };
        var testUser2 = new UserAccountEntity { UserName = "Some other user" };
        var testDiscountUsage1 = new DiscountUsageEntity
        {
            Discount = testDiscount1,
            Timestamp = DateTimeOffset.UtcNow,
            User = testUser1
        };
        var testDiscountUsage2 = new DiscountUsageEntity
        {
            Discount = testDiscount2,
            Timestamp = DateTimeOffset.UtcNow,
            User = testUser2
        };
        _referenceDbContext.DiscountUsages.Add(testDiscountUsage1);
        _referenceDbContext.DiscountUsages.Add(testDiscountUsage2);
        _referenceDbContext.SaveChanges();

        // act
        var readResult = _discountUsageService.ReadAll(null, null, null, null);

        // assert
        readResult.Should().HaveValue(new Page<DiscountUsageListModel>(new List<DiscountUsageEntity>()
            {
                testDiscountUsage1,
                testDiscountUsage2
            }.ToModels(),
            new PageMeta(1, Constants.DefaultPageSize, 1, 2, 2, 1)));
    }

    [Fact]
    public void ReadAll_ReadsCorrectly_WhenFilteringByDiscount()
    {
        // arrange
        var testDiscount1 = new DiscountEntity { Name = "Some discount" };
        var testUser1 = new UserAccountEntity { UserName = "Some user" };
        var testDiscount2 = new DiscountEntity { Name = "Some other discount" };
        var testUser2 = new UserAccountEntity { UserName = "Some other user" };
        var testDiscountUsage1 = new DiscountUsageEntity
        {
            Discount = testDiscount1,
            Timestamp = DateTimeOffset.UtcNow,
            User = testUser1
        };
        var testDiscountUsage2 = new DiscountUsageEntity
        {
            Discount = testDiscount2,
            Timestamp = DateTimeOffset.UtcNow,
            User = testUser2
        };
        _referenceDbContext.DiscountUsages.Add(testDiscountUsage1);
        _referenceDbContext.DiscountUsages.Add(testDiscountUsage2);
        _referenceDbContext.SaveChanges();

        // act
        var readResult = _discountUsageService.ReadAll(null, null, testDiscount1.Id, null);

        // assert
        readResult.Should().HaveValue(new Page<DiscountUsageListModel>(new List<DiscountUsageEntity>()
            {
                testDiscountUsage1
            }.ToModels(),
            new PageMeta(1, Constants.DefaultPageSize, 1, 1, 1, 1)));
    }

    [Fact]
    public void ReadAll_ReadsCorrectly_WhenFilteringByUser()
    {
        // arrange
        var testDiscount1 = new DiscountEntity { Name = "Some discount" };
        var testUser1 = new UserAccountEntity { UserName = "Some user" };
        var testDiscount2 = new DiscountEntity { Name = "Some other discount" };
        var testUser2 = new UserAccountEntity { UserName = "Some other user" };
        var testDiscountUsage1 = new DiscountUsageEntity
        {
            Discount = testDiscount1,
            Timestamp = DateTimeOffset.UtcNow,
            User = testUser1
        };
        var testDiscountUsage2 = new DiscountUsageEntity
        {
            Discount = testDiscount2,
            Timestamp = DateTimeOffset.UtcNow,
            User = testUser2
        };
        _referenceDbContext.DiscountUsages.Add(testDiscountUsage1);
        _referenceDbContext.DiscountUsages.Add(testDiscountUsage2);
        _referenceDbContext.SaveChanges();

        // act
        var readResult = _discountUsageService.ReadAll(null, null, null, testUser1.Id);

        // assert
        readResult.Should().HaveValue(new Page<DiscountUsageListModel>(new List<DiscountUsageEntity>()
            {
                testDiscountUsage1
            }.ToModels(),
            new PageMeta(1, Constants.DefaultPageSize, 1, 1, 1, 1)));
    }

    [Fact]
    public void ReadAll_ReturnsErrors_WhenFilteringByNonexistentUser()
    {
        // arrange
        var testDiscount1 = new DiscountEntity { Name = "Some discount" };
        var testUser1 = new UserAccountEntity { UserName = "Some user" };
        var testDiscount2 = new DiscountEntity { Name = "Some other discount" };
        var testUser2 = new UserAccountEntity { UserName = "Some other user" };
        var testDiscountUsage1 = new DiscountUsageEntity
        {
            Discount = testDiscount1,
            Timestamp = DateTimeOffset.UtcNow,
            User = testUser1
        };
        var testDiscountUsage2 = new DiscountUsageEntity
        {
            Discount = testDiscount2,
            Timestamp = DateTimeOffset.UtcNow,
            User = testUser2
        };
        _referenceDbContext.DiscountUsages.Add(testDiscountUsage1);
        _referenceDbContext.DiscountUsages.Add(testDiscountUsage2);
        _referenceDbContext.SaveChanges();
        const int userId = 42;

        // act
        var readResult = _discountUsageService.ReadAll(null, null, null, userId);

        // assert
        readResult.Should().HaveValue(new Dictionary<string, string[]>
        {
            {
                nameof(userId),
                [$"User with id {userId} doesn't exist"]
            }
        });
    }

    [Fact]
    public void Read_ReturnsNotFound_WhenDiscountUsageNotFound()
    {
        // act
        var readResult = _discountUsageService.Read(42);

        // assert
        readResult.Should().BeNotFound();
    }

    [Fact]
    public void Read_ReadsCorrectly_WhenComplex()
    {
        var testCurrency = new CurrencyEntity { Name = "Czech crowns" };
        var testUser = new UserAccountEntity { UserName = "Some user" };
        var testSaleTransactionItem = new SaleTransactionItemEntity
        {
            ItemAmount = 1,
            SaleItem = new SaleItemEntity
            {
                Name = "Some sale item",
                Costs =
                {
                    new CurrencyCostEntity
                    {
                        Amount = 42,
                        Currency = testCurrency,
                        ValidSince = DateTimeOffset.UtcNow
                    }
                }
            },
            SaleTransaction = new SaleTransactionEntity
            {
                Timestamp = DateTimeOffset.UtcNow,
                ResponsibleUser = testUser
            }
        };
        var testUsageItem =
            new DiscountUsageItemEntity
            {
                Amount = -15,
                Currency = testCurrency,
                SaleTransactionItem = testSaleTransactionItem
            };
        var testDiscountUsage1 = new DiscountUsageEntity
        {
            Discount = new DiscountEntity { Name = "Some discount" },
            Timestamp = DateTimeOffset.UtcNow,
            User = new UserAccountEntity { UserName = "Some other user" },
            UsageItems =
            {
                testUsageItem
            }
        };
        _referenceDbContext.DiscountUsages.Add(testDiscountUsage1);
        _referenceDbContext.SaveChanges();

        // act
        var readResult = _discountUsageService.Read(testDiscountUsage1.Id);

        // assert
        readResult.IsT0.Should().BeTrue();
        readResult.AsT0.Should().BeEquivalentTo(_referenceDbContext.DiscountUsages.Find(testDiscountUsage1.Id).ToModel());
    }
}