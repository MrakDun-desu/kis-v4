using BL.EF.Tests.Fixtures;
using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace BL.EF.Tests.Services;

public class ModifierServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable
{
    private readonly KisDbContext _referenceDbContext;
    private readonly KisDbContext _normalDbContext;
    private readonly ModifierService _modifierService;

    public ModifierServiceTests(KisDbContextFactory dbContextFactory)
    {
        (_referenceDbContext, _normalDbContext) = dbContextFactory.CreateDbContextAndReference();
        _modifierService = new ModifierService(_normalDbContext);
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

    // [Fact]
    // public void Create_CreatesModifier_WhenDataIsValid()
    // {
    //     var modifier = _dbContext.SaleItems.Add(new SaleItemEntity { Name = "Test sale item" });
    //     _dbContext.SaveChanges();
    //     var createModel = new ModifierCreateModel(modifier.Entity.Id, "Some modifier", string.Empty, [], true);
    //     var createdId = _modifierService.Create(createModel);
    //
    //     var createdEntity = _dbContext.Modifiers.Find(createdId);
    //     var expectedEntity = new ModifierEntity
    //     {
    //         ModificationTargetId = createModel.ModificationTargetId,
    //         ModificationTarget = modifier.Entity,
    //         Id = createdId,
    //         Name = createModel.Name,
    //         ShowOnWeb = createModel.ShowOnWeb
    //     };
    //     createdEntity.Should().BeEquivalentTo(expectedEntity);
    // }
    //
    // [Fact]
    // public void ReadAll_ReadsAll()
    // {
    //     var saleItem = new SaleItemEntity { Name = "Test sale item" };
    //     saleItem.AvailableModifiers.Add(new ModifierEntity { Name = "Some modifier" });
    //     saleItem.AvailableModifiers.Add(new ModifierEntity { Name = "Some modifier 2" });
    //     var insertedSaleItem = _dbContext.Add(saleItem);
    //     _dbContext.SaveChanges();
    //
    //     var readModels = _modifierService.ReadAll();
    //     var mappedModels = _dbContext.Modifiers.Where(si => !si.Deleted).ToList().ToModels();
    //
    //     readModels.Should().BeEquivalentTo(mappedModels);
    // }
    //
    // [Fact]
    // public void ReadAll_DoesntRead_Deleted()
    // {
    //     var saleItem = new SaleItemEntity { Name = "Test sale item" };
    //     saleItem.AvailableModifiers.Add(new ModifierEntity { Name = "Some modifier" });
    //     saleItem.AvailableModifiers.Add(new ModifierEntity { Name = "Some modifier 2", Deleted = true });
    //     var insertedSaleItem = _dbContext.Add(saleItem);
    //     _dbContext.SaveChanges();
    //
    //     var readModels = _modifierService.ReadAll();
    //     var mappedModels = _dbContext.Modifiers.Where(si => !si.Deleted).ToList().ToModels();
    //
    //     readModels.Should().BeEquivalentTo(mappedModels);
    // }
    //
    // [Fact]
    // public void Update_UpdatesName_WhenExistingId()
    // {
    //     const string oldName = "Some modifier";
    //     const string newName = "Some modifier 2";
    //     var saleItem = new SaleItemEntity { Name = "Test sale item" };
    //     var testModifier1 = new ModifierEntity { Name = oldName, ModificationTarget = saleItem };
    //     var insertedEntity = _dbContext.Modifiers.Add(testModifier1);
    //     _dbContext.SaveChanges();
    //     var updateModel = new ModifierUpdateModel(newName, null, null, null);
    //
    //     var updateSuccess = _modifierService.Update(insertedEntity.Entity.Id, updateModel);
    //
    //     updateSuccess.Should().BeTrue();
    //     var updatedEntity = _dbContext.Modifiers.Find(insertedEntity.Entity.Id);
    //     var expectedEntity = insertedEntity.Entity with { Name = newName };
    //     updatedEntity.Should().BeEquivalentTo(expectedEntity);
    // }
    //
    // [Fact]
    // public void Update_UpdatesImage_WhenExistingId()
    // {
    //     const string oldImage = "Some modifier";
    //     const string newImage = "Some modifier 2";
    //     var saleItem = new SaleItemEntity { Name = "Test sale item" };
    //     var testModifier1 = new ModifierEntity
    //         { Name = "Test sale item", Image = oldImage, ModificationTarget = saleItem };
    //     var insertedEntity = _dbContext.Modifiers.Add(testModifier1);
    //     _dbContext.SaveChanges();
    //     var updateModel = new ModifierUpdateModel(null, newImage, null, null);
    //
    //     var updateSuccess = _modifierService.Update(insertedEntity.Entity.Id, updateModel);
    //
    //     updateSuccess.Should().BeTrue();
    //     var updatedEntity = _dbContext.Modifiers.Find(insertedEntity.Entity.Id);
    //     var expectedEntity = insertedEntity.Entity with { Image = newImage };
    //     updatedEntity.Should().BeEquivalentTo(expectedEntity);
    // }
    //
    // [Fact]
    // public void Update_UpdatesCategories_WhenExistingId()
    // {
    //     var newCategory = _dbContext.ProductCategories.Add(new ProductCategoryEntity { Name = "test category 2" });
    //     var saleItem = new SaleItemEntity { Name = "Test sale item" };
    //     var modifier = new ModifierEntity { Name = "Test modifier", ModificationTarget = saleItem };
    //     modifier.Categories.Add(new ProductCategoryEntity { Name = "test category 1" });
    //     var insertedEntity = _dbContext.Modifiers.Add(modifier);
    //
    //     _dbContext.SaveChanges();
    //     var updateModel = new ModifierUpdateModel(null, null,
    //         [new CategoryListModel(newCategory.Entity.Id, "Category 1")], null);
    //
    //     var updateSuccess = _modifierService.Update(insertedEntity.Entity.Id, updateModel);
    //
    //     updateSuccess.Should().BeTrue();
    //     var updatedEntity = _dbContext.Modifiers.Find(insertedEntity.Entity.Id);
    //     var expectedEntity = insertedEntity.Entity with { };
    //     expectedEntity.Categories.Clear();
    //     expectedEntity.Categories.Add(newCategory.Entity);
    //     updatedEntity.Should().BeEquivalentTo(expectedEntity);
    // }
    //
    // [Fact]
    // public void Update_UpdatesShowOnWeb_WhenExistingId()
    // {
    //     var saleItem = new SaleItemEntity { Name = "Test sale item" };
    //     var testModifier1 = new ModifierEntity { Name = "Test name", ModificationTarget = saleItem };
    //     var insertedEntity = _dbContext.Modifiers.Add(testModifier1);
    //     _dbContext.SaveChanges();
    //     var updateModel = new ModifierUpdateModel(null, null, null, true);
    //
    //     var updateSuccess = _modifierService.Update(insertedEntity.Entity.Id, updateModel);
    //
    //     updateSuccess.Should().BeTrue();
    //     var updatedEntity = _dbContext.Modifiers.Find(insertedEntity.Entity.Id);
    //     var expectedEntity = insertedEntity.Entity with { ShowOnWeb = true };
    //     updatedEntity.Should().BeEquivalentTo(expectedEntity);
    // }
    //
    // [Fact]
    // public void Update_ReturnsFalse_WhenNotFound()
    // {
    //     var updateModel = new ModifierUpdateModel("Some modifier", null, null, null);
    //
    //     var updateSuccess = _modifierService.Update(42, updateModel);
    //
    //     updateSuccess.Should().BeFalse();
    // }
    //
    // [Fact]
    // public void Delete_Deletes_WhenExistingId()
    // {
    //     var saleItem = new SaleItemEntity { Name = "Test sale item" };
    //     var testModifier1 = new ModifierEntity { Name = "Some modifier", ModificationTarget = saleItem };
    //     var insertedEntity = _dbContext.Modifiers.Add(testModifier1);
    //     _dbContext.SaveChanges();
    //
    //     var deleteSuccess = _modifierService.Delete(insertedEntity.Entity.Id);
    //
    //     deleteSuccess.Should().BeTrue();
    //     var deletedEntity = _dbContext.Modifiers.Find(insertedEntity.Entity.Id);
    //     deletedEntity!.Deleted.Should().BeTrue();
    // }
    //
    // [Fact]
    // public void Delete_ReturnsFalse_WhenNotFound()
    // {
    //     var deleteSuccess = _modifierService.Delete(42);
    //
    //     deleteSuccess.Should().BeFalse();
    // }
}