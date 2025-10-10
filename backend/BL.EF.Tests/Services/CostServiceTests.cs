using BL.EF.Tests.Extensions;
using BL.EF.Tests.Fixtures;
using FluentAssertions;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace BL.EF.Tests.Services;

[Collection(DockerDatabaseTests.Name)]
public class CostServiceTests : IDisposable, IAsyncDisposable {
    private readonly CostService _costService;
    private readonly KisDbContext _referenceDbContext;
    private readonly KisDbContext _normalDbContext;

    public CostServiceTests(KisDbContextFactory dbContextFactory) {
        (_referenceDbContext, _normalDbContext) = dbContextFactory.CreateDbContextAndReference();
        _costService = new CostService(_normalDbContext);
        AssertionOptions.AssertEquivalencyUsing(static options =>
            options.Using<DateTimeOffset>(ctx =>
                ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1))).WhenTypeIs<DateTimeOffset>()
        );
    }

    public async ValueTask DisposeAsync() {
        GC.SuppressFinalize(this);
        await _referenceDbContext.DisposeAsync();
        await _normalDbContext.DisposeAsync();
    }

    public void Dispose() {
        GC.SuppressFinalize(this);
        _referenceDbContext.Dispose();
        _normalDbContext.Dispose();
    }

    [Fact]
    public void Create_Creates_WhenDataIsValid() {
        // arrange
        var testStoreItem = new StoreItemEntity {
            Name = "Test store item"
        };
        var testCurrency = new CurrencyEntity {
            Name = "Test currency"
        };
        _referenceDbContext.StoreItems.Add(testStoreItem);
        _referenceDbContext.Currencies.Add(testCurrency);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();
        const decimal currencyAmount = 42;
        const string costDescription = "Testing cost";
        var costValidSince = DateTimeOffset.Now;
        var createModel = new CostCreateModel(
            testStoreItem.Id,
            testCurrency.Id,
            costValidSince,
            currencyAmount,
            costDescription
        );

        // act
        var creationResult = _costService.Create(createModel);

        // assert
        creationResult.IsT0.Should().BeTrue();
        var id = creationResult.AsT0.Id;
        var createdEntity = _referenceDbContext.CurrencyCosts
            .Include(c => c.Product)
            .Include(c => c.Currency)
            .First(c => c.Id == id);
        var expectedEntity = new CurrencyCostEntity {
            Id = id,
            CurrencyId = testCurrency.Id,
            Currency = testCurrency,
            ProductId = testStoreItem.Id,
            Product = testStoreItem,
            Amount = currencyAmount,
            Description = costDescription,
            ValidSince = costValidSince.ToUniversalTime()
        };
        createdEntity.Should().BeEquivalentTo(expectedEntity, opts =>
            opts.Excluding(entity => entity.Product!.Costs)
            );
    }

    [Fact]
    public void Create_ReturnsErrors_WhenDataIsNotValid() {
        // arrange
        var createModel = new CostCreateModel(
            42,
            42,
            DateTimeOffset.Now,
            42.42M,
            "Some new cost"
        );

        // act
        var creationResult = _costService.Create(createModel);

        // assert
        creationResult.Should().HaveValue(new Dictionary<string, string[]>
        {
            {
                nameof(createModel.ProductId),
                [$"Product with id {createModel.ProductId} doesn't exist"]
            },
            {
                nameof(createModel.CurrencyId),
                [$"Currency with id {createModel.CurrencyId} doesn't exist"]
            }
        });
    }
}
