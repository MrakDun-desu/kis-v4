using BL.EF.Tests.Extensions;
using BL.EF.Tests.Fixtures;
using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace BL.EF.Tests.Services;

public class CompositionServiceTests : IClassFixture<KisDbContextFactory>, IDisposable,
    IAsyncDisposable
{
    private readonly CompositionService _compositionService;
    private readonly KisDbContext _referenceDbContext;
    private readonly KisDbContext _normalDbContext;

    public CompositionServiceTests(KisDbContextFactory dbContextFactory)
    {
        (_referenceDbContext, _normalDbContext) = dbContextFactory.CreateDbContextAndReference();
        _compositionService = new CompositionService(_normalDbContext);
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
    public void Create_DoesntCreate_WhenAmountIs0()
    {
        // arrange
        var createModel = new CompositionCreateModel(1, 2, 0);

        // act
        var result = _compositionService.CreateOrUpdate(createModel);

        // assert
        var createdEntity = _referenceDbContext.Compositions.Find(createModel.SaleItemId, createModel.StoreItemId);
        createdEntity.Should().BeNull();
        result.Should().HaveValue(new Dictionary<string, string[]>
        {
            { nameof(createModel.SaleItemId), [$"Sale item with id {createModel.SaleItemId} doesn't exist"] },
            { nameof(createModel.StoreItemId), [$"Store item with id {createModel.StoreItemId} doesn't exist"] }
        });
    }

    [Fact]
    public void Create_Creates_WhenAmountIsMoreThan0()
    {
        // arrange
        var saleItemEntity = new SaleItemEntity { Name = "Test sale item" };
        var storeItemEntity = new StoreItemEntity { Name = "Test store item" };
        var insertedSaleItem = _referenceDbContext.SaleItems.Add(saleItemEntity);
        var insertedStoreItem = _referenceDbContext.StoreItems.Add(storeItemEntity);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();
        var saleItemId = insertedSaleItem.Entity.Id;
        var storeItemId = insertedStoreItem.Entity.Id;
        var createModel = new CompositionCreateModel(saleItemId, storeItemId, 42);

        // act
        var result = _compositionService.CreateOrUpdate(createModel);

        // assert
        var createdEntity = _referenceDbContext.Compositions.Find(saleItemId, storeItemId);
        var expectedEntity = new CompositionEntity
        {
            Amount = createModel.Amount,
            SaleItemId = saleItemId,
            StoreItemId = storeItemId,
            SaleItem = insertedSaleItem.Entity,
            StoreItem = insertedStoreItem.Entity
        };
        createdEntity.Should().BeEquivalentTo(expectedEntity, opts => 
                opts.Excluding(entity => entity.SaleItem)
                    .Excluding(entity => entity.StoreItem));
        result.Should().HaveValue(expectedEntity.ToModel());
    }

    [Fact]
    public void Create_DeletesExisting_WhenAmountIs0()
    {
        // arrange
        var saleItemEntity = new SaleItemEntity { Name = "Test sale item" };
        var storeItemEntity = new StoreItemEntity { Name = "Test store item" };
        var compositionEntity = new CompositionEntity
        {
            Amount = 42, SaleItem = saleItemEntity, StoreItem = storeItemEntity
        };
        var insertedSaleItem = _referenceDbContext.SaleItems.Add(saleItemEntity);
        var insertedStoreItem = _referenceDbContext.StoreItems.Add(storeItemEntity);
        _referenceDbContext.Compositions.Add(compositionEntity);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();
        var saleItemId = insertedSaleItem.Entity.Id;
        var storeItemId = insertedStoreItem.Entity.Id;
        var createModel = new CompositionCreateModel(saleItemId, storeItemId, 0);

        // act
        var result = _compositionService.CreateOrUpdate(createModel);

        // assert
        var createdEntity = _referenceDbContext.Compositions.Find(saleItemId, storeItemId);
        createdEntity.Should().BeNull();
        result.Should().BeSuccess();
    }

    [Fact]
    public void Create_UpdatesExisting_WhenAmountIsMoreThan0()
    {
        // arrange
        var saleItemEntity = new SaleItemEntity { Name = "Test sale item" };
        var storeItemEntity = new StoreItemEntity { Name = "Test store item" };
        var compositionEntity = new CompositionEntity
        {
            Amount = 42, SaleItem = saleItemEntity, StoreItem = storeItemEntity
        };
        var insertedSaleItem = _referenceDbContext.SaleItems.Add(saleItemEntity);
        var insertedStoreItem = _referenceDbContext.StoreItems.Add(storeItemEntity);
        _referenceDbContext.Compositions.Add(compositionEntity);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();
        var saleItemId = insertedSaleItem.Entity.Id;
        var storeItemId = insertedStoreItem.Entity.Id;
        var createModel = new CompositionCreateModel(saleItemId, storeItemId, 52);

        // act
        var result = _compositionService.CreateOrUpdate(createModel);

        // assert
        var createdEntity = _referenceDbContext.Compositions.Find(saleItemId, storeItemId);
        var expectedEntity = new CompositionEntity
        {
            Amount = createModel.Amount,
            SaleItemId = saleItemId,
            StoreItemId = storeItemId,
            SaleItem = saleItemEntity,
            StoreItem = storeItemEntity
        };
        createdEntity.Should().BeEquivalentTo(expectedEntity, opts =>
            opts
                .Excluding(entity => entity.SaleItem)
                .Excluding(entity => entity.StoreItem)
        );
        result.Should().HaveValue(expectedEntity.ToModel());
    }
}