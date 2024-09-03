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
        var addedStoreItem = _dbContext.StoreItems.Add(testStoreItem);
        var addedCurrency = _dbContext.Currencies.Add(testCurrency);
        _dbContext.SaveChanges();
        const decimal currencyAmount = 42;
        const string costDescription = "Testing cost";
        var costValidSince = DateTimeOffset.Now;
        var createModel = new CostCreateModel(
            addedStoreItem.Entity.Id,
            addedCurrency.Entity.Id,
            costValidSince,
            currencyAmount,
            costDescription
        );

        // act
        var createdModel = _costService.Create(createModel);

        // assert
        var createdEntity = _dbContext.CurrencyCosts.Find(createdModel.Id);
        var expectedEntity = new CurrencyCostEntity
        {
            Id = createdModel.Id,
            CurrencyId = addedCurrency.Entity.Id,
            Currency = addedCurrency.Entity,
            ProductId = addedStoreItem.Entity.Id,
            Product = addedStoreItem.Entity,
            Amount = currencyAmount,
            Description = costDescription,
            ValidSince = costValidSince.ToUniversalTime()
        };
        createdEntity.Should().BeEquivalentTo(expectedEntity);
    }
}