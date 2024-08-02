using System.Net.NetworkInformation;
using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BL.EF.Tests.Services;

public class CostServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable {
    private readonly CostService _costService;
    private readonly KisDbContext _dbContext;
    private readonly Mapper _mapper;

    public CostServiceTests(KisDbContextFactory dbContextFactory) {
        _dbContext = dbContextFactory.CreateDbContext();
        _mapper = new Mapper();
        _costService = new CostService(_dbContext, _mapper);
    }

    public void Dispose() {
        _dbContext.Dispose();
    }

    public async ValueTask DisposeAsync() {
        await _dbContext.DisposeAsync();
    }

    [Fact]
    public void Create_Creates_WhenDataIsValid() {
        // Given
        var testStoreItem = new StoreItemEntity {
            Name = "Test store item"
        };
        var testCurrency = new CurrencyEntity {
            Name = "Test currency"
        };
        var addedStoreItem = _dbContext.StoreItems.Add(testStoreItem);
        var addedCurrency = _dbContext.Currencies.Add(testCurrency);
        _dbContext.SaveChanges();

        // When
        const decimal currencyAmount = 42;
        const string costDescription = "Testing cost";
        var costValidSince = DateTimeOffset.UtcNow;
        var createModel = new CostCreateModel(
            addedStoreItem.Entity.Id,
            addedCurrency.Entity.Id,
            costValidSince,
            currencyAmount,
            costDescription
            );
        var createdId = _costService.Create(createModel);

        // Then
        var createdEntity = _dbContext.CurrencyCosts.Find(createdId);
        var expectedEntity = new CurrencyCostEntity {
            Id = createdId,
            CurrencyId = addedCurrency.Entity.Id,
            Currency = addedCurrency.Entity,
            ProductId = addedStoreItem.Entity.Id,
            Product = addedStoreItem.Entity,
            Amount = currencyAmount,
            Description = costDescription,
            ValidSince = costValidSince,
        };
        createdEntity.Should().BeEquivalentTo(expectedEntity);
    }
}
