using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace BL.EF.Tests.Services;

public class SaleItemServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable
{
    private readonly KisDbContext _dbContext;
    private readonly SaleItemService _saleItemService;

    public SaleItemServiceTests(KisDbContextFactory dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
        _saleItemService = new SaleItemService(_dbContext);
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
    public void Create_CreatesSaleItem_WhenDataIsValid()
    {
        var createModel = new SaleItemCreateModel("Some saleItem", string.Empty, [], true);
        var createdId = _saleItemService.Create(createModel);

        var createdEntity = _dbContext.SaleItems.Find(createdId);
        var expectedEntity = new SaleItemEntity
        {
            Id = createdId,
            Name = createModel.Name,
            ShowOnWeb = createModel.ShowOnWeb
        };
        createdEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void ReadAll_ReadsAll()
    {
        var testSaleItem1 = new SaleItemEntity { Name = "Some saleItem" };
        var testSaleItem2 = new SaleItemEntity { Name = "Some saleItem 2" };
        _dbContext.SaleItems.Add(testSaleItem1);
        _dbContext.SaleItems.Add(testSaleItem2);
        _dbContext.SaveChanges();

        var readModels = _saleItemService.ReadAll();
        var mappedModels = _dbContext.SaleItems.Where(si => !si.Deleted).ToList().ToModels();

        readModels.Should().BeEquivalentTo(mappedModels);
    }

    [Fact]
    public void ReadAll_DoesntRead_Deleted()
    {
        var testSaleItem1 = new SaleItemEntity { Name = "Some saleItem" };
        var testSaleItem2 = new SaleItemEntity { Name = "Some saleItem 2", Deleted = true };
        _dbContext.SaleItems.Add(testSaleItem1);
        _dbContext.SaleItems.Add(testSaleItem2);
        _dbContext.SaveChanges();

        var readModels = _saleItemService.ReadAll();
        var mappedModels = _dbContext.SaleItems.Where(si => !si.Deleted).ToList().ToModels();

        readModels.Should().BeEquivalentTo(mappedModels);
    }

    // [Fact]
    // public void Update_UpdatesName_WhenExistingId()
    // {
    //     const string oldName = "Some saleItem";
    //     const string newName = "Some saleItem 2";
    //     var testSaleItem1 = new SaleItemEntity { Name = oldName };
    //     var insertedEntity = _dbContext.SaleItems.Add(testSaleItem1);
    //     _dbContext.SaveChanges();
    //     var updateModel = new SaleItemUpdateModel(newName, null, null, null);
    //
    //     var updateSuccess = _saleItemService.Update(insertedEntity.Entity.Id, updateModel);
    //
    //     updateSuccess.Should().BeTrue();
    //     var updatedEntity = _dbContext.SaleItems.Find(insertedEntity.Entity.Id);
    //     var expectedEntity = insertedEntity.Entity with { Name = newName };
    //     updatedEntity.Should().BeEquivalentTo(expectedEntity);
    // }
    //
    // [Fact]
    // public void Update_UpdatesImage_WhenExistingId()
    // {
    //     const string oldImage = "Some saleItem";
    //     const string newImage = "Some saleItem 2";
    //     var testSaleItem1 = new SaleItemEntity { Name = "Test sale item", Image = oldImage };
    //     var insertedEntity = _dbContext.SaleItems.Add(testSaleItem1);
    //     _dbContext.SaveChanges();
    //     var updateModel = new SaleItemUpdateModel(null, newImage, null, null);
    //
    //     var updateSuccess = _saleItemService.Update(insertedEntity.Entity.Id, updateModel);
    //
    //     updateSuccess.Should().BeTrue();
    //     var updatedEntity = _dbContext.SaleItems.Find(insertedEntity.Entity.Id);
    //     var expectedEntity = insertedEntity.Entity with { Image = newImage };
    //     updatedEntity.Should().BeEquivalentTo(expectedEntity);
    // }
    //
    // [Fact]
    // public void Update_UpdatesCategories_WhenExistingId()
    // {
    //     var newCategory = _dbContext.ProductCategories.Add(new ProductCategoryEntity { Name = "test category 2" });
    //     var saleItem = new SaleItemEntity { Name = "Test sale item" };
    //     saleItem.Categories.Add(new ProductCategoryEntity { Name = "test category 1" });
    //     var insertedEntity = _dbContext.SaleItems.Add(saleItem);
    //
    //     _dbContext.SaveChanges();
    //     var updateModel = new SaleItemUpdateModel(null, null,
    //         [new CategoryListModel(newCategory.Entity.Id, "Category 1")], null);
    //
    //     var updateSuccess = _saleItemService.Update(insertedEntity.Entity.Id, updateModel);
    //
    //     updateSuccess.Should().BeTrue();
    //     var updatedEntity = _dbContext.SaleItems.Find(insertedEntity.Entity.Id);
    //     var expectedEntity = insertedEntity.Entity with { };
    //     expectedEntity.Categories.Clear();
    //     expectedEntity.Categories.Add(newCategory.Entity);
    //     updatedEntity.Should().BeEquivalentTo(expectedEntity);
    // }
    //
    // [Fact]
    // public void Update_UpdatesShowOnWeb_WhenExistingId()
    // {
    //     var testSaleItem1 = new SaleItemEntity { Name = "Test name" };
    //     var insertedEntity = _dbContext.SaleItems.Add(testSaleItem1);
    //     _dbContext.SaveChanges();
    //     var updateModel = new SaleItemUpdateModel(null, null, null, true);
    //
    //     var updateSuccess = _saleItemService.Update(insertedEntity.Entity.Id, updateModel);
    //
    //     updateSuccess.Should().BeTrue();
    //     var updatedEntity = _dbContext.SaleItems.Find(insertedEntity.Entity.Id);
    //     var expectedEntity = insertedEntity.Entity with { ShowOnWeb = true };
    //     updatedEntity.Should().BeEquivalentTo(expectedEntity);
    // }
    //
    // [Fact]
    // public void Update_ReturnsFalse_WhenNotFound()
    // {
    //     var updateModel = new SaleItemUpdateModel("Some saleItem", null, null, null);
    //
    //     var updateSuccess = _saleItemService.Update(42, updateModel);
    //
    //     updateSuccess.Should().BeFalse();
    // }

    [Fact]
    public void Delete_Deletes_WhenExistingId()
    {
        var testSaleItem1 = new SaleItemEntity { Name = "Some saleItem" };
        var insertedEntity = _dbContext.SaleItems.Add(testSaleItem1);
        _dbContext.SaveChanges();

        var deleteSuccess = _saleItemService.Delete(insertedEntity.Entity.Id);

        deleteSuccess.Should().BeTrue();
        var deletedEntity = _dbContext.SaleItems.Find(insertedEntity.Entity.Id);
        deletedEntity!.Deleted.Should().BeTrue();
    }

    [Fact]
    public void Delete_ReturnsFalse_WhenNotFound()
    {
        var deleteSuccess = _saleItemService.Delete(42);

        deleteSuccess.Should().BeFalse();
    }
}