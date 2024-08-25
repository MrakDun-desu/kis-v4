using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace BL.EF.Tests.Services;

public class StoreItemServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable
{
    private readonly KisDbContext _dbContext;
    private readonly StoreItemService _saleItemService;

    public StoreItemServiceTests(KisDbContextFactory dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
        _saleItemService = new StoreItemService(_dbContext);
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
    public void Create_CreatesStoreItem_WhenDataIsValid()
    {
        var createModel = new StoreItemCreateModel(
            "Some store item",
            string.Empty,
            [],
            "ks",
            true,
            false
        );
        var createdId = _saleItemService.Create(createModel);

        var createdEntity = _dbContext.StoreItems.Find(createdId);
        var expectedEntity = new StoreItemEntity
        {
            Id = createdId,
            Name = createModel.Name,
            UnitName = createModel.UnitName,
            BarmanCanStock = createModel.BarmanCanStock,
            IsContainerItem = createModel.IsContainerItem
        };
        createdEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void ReadAll_ReadsAll()
    {
        var testStoreItem1 = new StoreItemEntity { Name = "Some store item" };
        var testStoreItem2 = new StoreItemEntity { Name = "Some store item 2" };
        _dbContext.StoreItems.Add(testStoreItem1);
        _dbContext.StoreItems.Add(testStoreItem2);
        _dbContext.SaveChanges();

        var readModels = _saleItemService.ReadAll();
        var mappedModels = _dbContext.StoreItems.Where(si => !si.Deleted).ToList().ToModels();

        readModels.Should().BeEquivalentTo(mappedModels);
    }

    [Fact]
    public void ReadAll_DoesntRead_Deleted()
    {
        var testStoreItem1 = new StoreItemEntity { Name = "Some store item" };
        var testStoreItem2 = new StoreItemEntity { Name = "Some store item 2", Deleted = true };
        _dbContext.StoreItems.Add(testStoreItem1);
        _dbContext.StoreItems.Add(testStoreItem2);
        _dbContext.SaveChanges();

        var readModels = _saleItemService.ReadAll();
        var mappedModels = _dbContext.StoreItems.Where(si => !si.Deleted).ToList().ToModels();

        readModels.Should().BeEquivalentTo(mappedModels);
    }

    [Fact]
    public void Update_UpdatesName_WhenExistingId()
    {
        const string oldName = "Some store item";
        const string newName = "Some store item 2";
        var testStoreItem1 = new StoreItemEntity { Name = oldName };
        var insertedEntity = _dbContext.StoreItems.Add(testStoreItem1);
        _dbContext.SaveChanges();
        var updateModel = new StoreItemUpdateModel(newName, null, null, null, null);

        var updateSuccess = _saleItemService.Update(insertedEntity.Entity.Id, updateModel);

        updateSuccess.Should().BeTrue();
        var updatedEntity = _dbContext.StoreItems.Find(insertedEntity.Entity.Id);
        var expectedEntity = insertedEntity.Entity with { Name = newName };
        updatedEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void Update_UpdatesImage_WhenExistingId()
    {
        const string oldImage = "Some store item";
        const string newImage = "Some store item 2";
        var testStoreItem1 = new StoreItemEntity { Name = "Test sale item", Image = oldImage };
        var insertedEntity = _dbContext.StoreItems.Add(testStoreItem1);
        _dbContext.SaveChanges();
        var updateModel = new StoreItemUpdateModel(null, newImage, null, null, null);

        var updateSuccess = _saleItemService.Update(insertedEntity.Entity.Id, updateModel);

        updateSuccess.Should().BeTrue();
        var updatedEntity = _dbContext.StoreItems.Find(insertedEntity.Entity.Id);
        var expectedEntity = insertedEntity.Entity with { Image = newImage };
        updatedEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void Update_UpdatesCategories_WhenExistingId()
    {
        var newCategory = _dbContext.ProductCategories.Add(new ProductCategoryEntity { Name = "test category 2" });
        var storeItem = new StoreItemEntity { Name = "Test sale item" };
        storeItem.Categories.Add(new ProductCategoryEntity { Name = "test category 1" });
        var insertedEntity = _dbContext.StoreItems.Add(storeItem);

        _dbContext.SaveChanges();
        var updateModel = new StoreItemUpdateModel(
            null,
            null,
            [new CategoryReadAllModel(newCategory.Entity.Id, "Category 1")],
            null,
            null
        );

        var updateSuccess = _saleItemService.Update(insertedEntity.Entity.Id, updateModel);

        updateSuccess.Should().BeTrue();
        var updatedEntity = _dbContext.StoreItems.Find(insertedEntity.Entity.Id);
        var expectedEntity = insertedEntity.Entity with { };
        expectedEntity.Categories.Clear();
        expectedEntity.Categories.Add(newCategory.Entity);
        updatedEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void Update_UpdatesUnitName_WhenExistingId()
    {
        var testStoreItem1 = new StoreItemEntity { Name = "Test name" };
        var insertedEntity = _dbContext.StoreItems.Add(testStoreItem1);
        _dbContext.SaveChanges();
        var updateModel = new StoreItemUpdateModel(null, null, null, "pieces", null);

        var updateSuccess = _saleItemService.Update(insertedEntity.Entity.Id, updateModel);

        updateSuccess.Should().BeTrue();
        var updatedEntity = _dbContext.StoreItems.Find(insertedEntity.Entity.Id);
        var expectedEntity = insertedEntity.Entity with { UnitName = updateModel.UnitName! };
        updatedEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void Update_UpdatesBarmanCanStock_WhenExistingId()
    {
        var testStoreItem1 = new StoreItemEntity { Name = "Test name" };
        var insertedEntity = _dbContext.StoreItems.Add(testStoreItem1);
        _dbContext.SaveChanges();
        var updateModel = new StoreItemUpdateModel(null, null, null, null, true);

        var updateSuccess = _saleItemService.Update(insertedEntity.Entity.Id, updateModel);

        updateSuccess.Should().BeTrue();
        var updatedEntity = _dbContext.StoreItems.Find(insertedEntity.Entity.Id);
        var expectedEntity = insertedEntity.Entity with { BarmanCanStock = updateModel.BarmanCanStock!.Value };
        updatedEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void Update_ReturnsFalse_WhenNotFound()
    {
        var updateModel = new StoreItemUpdateModel("Some store item", null, null, null, null);

        var updateSuccess = _saleItemService.Update(42, updateModel);

        updateSuccess.Should().BeFalse();
    }

    [Fact]
    public void Delete_Deletes_WhenExistingId()
    {
        var testStoreItem1 = new StoreItemEntity { Name = "Some store item" };
        var insertedEntity = _dbContext.StoreItems.Add(testStoreItem1);
        _dbContext.SaveChanges();

        var deleteSuccess = _saleItemService.Delete(insertedEntity.Entity.Id);

        deleteSuccess.Should().BeTrue();
        var deletedEntity = _dbContext.StoreItems.Find(insertedEntity.Entity.Id);
        deletedEntity!.Deleted.Should().BeTrue();
    }

    [Fact]
    public void Delete_ReturnsFalse_WhenNotFound()
    {
        var deleteSuccess = _saleItemService.Delete(42);

        deleteSuccess.Should().BeFalse();
    }
}