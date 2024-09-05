using BL.EF.Tests.Extensions;
using FluentAssertions;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace BL.EF.Tests.Services;

public class CostServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable
{
    private readonly CostService _costService;
    private readonly KisDbContext _dbContext;

    public CostServiceTests(KisDbContextFactory dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
        _costService = new CostService(_dbContext);
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
    public void Create_Creates_WhenDataIsValid()
    {
        // arrange
        var testStoreItem = new StoreItemEntity
        {
            Name = "Test store item"
        };
        var testCurrency = new CurrencyEntity
        {
            Name = "Test currency"
        };
        _dbContext.StoreItems.Add(testStoreItem);
        _dbContext.Currencies.Add(testCurrency);
        _dbContext.SaveChanges();
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
        var createdEntity = _dbContext.CurrencyCosts.Find(id);
        var expectedEntity = new CurrencyCostEntity
        {
            Id = id,
            CurrencyId = testCurrency.Id,
            Currency = testCurrency,
            ProductId = testStoreItem.Id,
            Product = testStoreItem,
            Amount = currencyAmount,
            Description = costDescription,
            ValidSince = costValidSince.ToUniversalTime()
        };
        createdEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void Create_ReturnsErrors_WhenDataIsNotValid()
    {
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