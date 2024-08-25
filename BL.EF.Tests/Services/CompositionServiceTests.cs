using FluentAssertions;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace BL.EF.Tests.Services;

public class CompositionServiceTests : IClassFixture<KisDbContextFactory>, IDisposable,
    IAsyncDisposable
{
    private readonly CompositionService _compositionService;
    private readonly KisDbContext _dbContext;

    public CompositionServiceTests(KisDbContextFactory dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
        _compositionService = new CompositionService(_dbContext);
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
    public void Create_DoesntCreate_WhenAmountIs0()
    {
        var createModel = new CompositionCreateModel(1, 1, 0);

        _compositionService.Create(createModel);

        var createdEntity = _dbContext.Compositions.Find(1, 1);
        createdEntity.Should().BeNull();
    }

    [Fact]
    public void Create_Creates_WhenAmountIsMoreThan0()
    {
        var saleItemEntity = new SaleItemEntity { Name = "Test sale item" };
        var storeItemEntity = new StoreItemEntity { Name = "Test store item" };
        var insertedSaleItem = _dbContext.SaleItems.Add(saleItemEntity);
        var insertedStoreItem = _dbContext.StoreItems.Add(storeItemEntity);
        _dbContext.SaveChanges();
        var saleItemId = insertedSaleItem.Entity.Id;
        var storeItemId = insertedStoreItem.Entity.Id;
        var createModel = new CompositionCreateModel(saleItemId, storeItemId, 42);

        _compositionService.Create(createModel);

        var createdEntity = _dbContext.Compositions.Find(saleItemId, storeItemId);
        createdEntity.Should().Be(new CompositionEntity
        {
            Amount = createModel.Amount,
            SaleItemId = saleItemId,
            StoreItemId = storeItemId,
            SaleItem = insertedSaleItem.Entity,
            StoreItem = insertedStoreItem.Entity
        });
    }

    [Fact]
    public void Create_DeletesExisting_WhenAmountIs0()
    {
        var saleItemEntity = new SaleItemEntity { Name = "Test sale item" };
        var storeItemEntity = new StoreItemEntity { Name = "Test store item" };
        var compositionEntity = new CompositionEntity
        {
            Amount = 42, SaleItem = saleItemEntity, StoreItem = storeItemEntity
        };
        var insertedSaleItem = _dbContext.SaleItems.Add(saleItemEntity);
        var insertedStoreItem = _dbContext.StoreItems.Add(storeItemEntity);
        _dbContext.Compositions.Add(compositionEntity);
        _dbContext.SaveChanges();
        var saleItemId = insertedSaleItem.Entity.Id;
        var storeItemId = insertedStoreItem.Entity.Id;
        var createModel = new CompositionCreateModel(saleItemId, storeItemId, 0);

        _compositionService.Create(createModel);

        var createdEntity = _dbContext.Compositions.Find(saleItemId, storeItemId);
        createdEntity.Should().BeNull();
    }

    [Fact]
    public void Create_UpdatesExisting_WhenAmountIsMoreThan0()
    {
        var saleItemEntity = new SaleItemEntity { Name = "Test sale item" };
        var storeItemEntity = new StoreItemEntity { Name = "Test store item" };
        var compositionEntity = new CompositionEntity
        {
            Amount = 42, SaleItem = saleItemEntity, StoreItem = storeItemEntity
        };
        var insertedSaleItem = _dbContext.SaleItems.Add(saleItemEntity);
        var insertedStoreItem = _dbContext.StoreItems.Add(storeItemEntity);
        _dbContext.Compositions.Add(compositionEntity);
        _dbContext.SaveChanges();
        var saleItemId = insertedSaleItem.Entity.Id;
        var storeItemId = insertedStoreItem.Entity.Id;
        var createModel = new CompositionCreateModel(saleItemId, storeItemId, 52);

        _compositionService.Create(createModel);

        var createdEntity = _dbContext.Compositions.Find(saleItemId, storeItemId);
        createdEntity.Should().Be(new CompositionEntity
        {
            Amount = createModel.Amount,
            SaleItemId = saleItemId,
            StoreItemId = storeItemId,
            SaleItem = insertedSaleItem.Entity,
            StoreItem = insertedStoreItem.Entity
        });
    }
}