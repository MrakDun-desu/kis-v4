using BL.EF.Tests.Extensions;
using BL.EF.Tests.Fixtures;
using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace BL.EF.Tests.Services;

[Collection(DockerDatabaseTests.Name)]
public class CategoryServiceTests : IDisposable, IAsyncDisposable {
    private readonly CategoryService _categoryService;
    private readonly KisDbContext _referenceDbContext;
    private readonly KisDbContext _normalDbContext;

    public CategoryServiceTests(KisDbContextFactory dbContextFactory) {
        (_referenceDbContext, _normalDbContext) = dbContextFactory.CreateDbContextAndReference();
        _categoryService = new CategoryService(_normalDbContext);
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
    public void ReadAll_ReadsAll() {
        // arrange
        var testCategory1 = new ProductCategoryEntity { Name = "Some category" };
        var testCategory2 = new ProductCategoryEntity { Name = "Some category 2" };
        _referenceDbContext.ProductCategories.Add(testCategory1);
        _referenceDbContext.ProductCategories.Add(testCategory2);
        _referenceDbContext.SaveChanges();

        // act
        var readModels = _categoryService.ReadAll();

        // assert
        var mappedModels = _referenceDbContext.ProductCategories.ToList().ToModels();
        readModels.Should().BeEquivalentTo(mappedModels);
    }

    [Fact]
    public void Create_CreatesCategory_WhenDataIsValid() {
        // arrange
        var createModel = new CategoryCreateModel("Some category");

        // act
        var createdModel = _categoryService.Create(createModel);

        // assert
        var createdEntity = _referenceDbContext.ProductCategories.Find(createdModel.Id);
        var expectedEntity = new ProductCategoryEntity { Id = createdModel.Id, Name = createModel.Name };
        createdEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void Update_UpdatesName_WhenExistingId() {
        // arrange
        const string oldName = "Some category";
        const string newName = "Some category 2";
        var testCategory1 = new ProductCategoryEntity { Name = oldName };
        _referenceDbContext.ProductCategories.Add(testCategory1);
        _referenceDbContext.SaveChanges();
        var updateModel = new CategoryCreateModel(newName);
        _referenceDbContext.ChangeTracker.Clear();

        // act
        var updateResult = _categoryService.Update(testCategory1.Id, updateModel);

        // assert
        var updatedEntity = _referenceDbContext.ProductCategories.Find(testCategory1.Id);
        var expectedEntity = testCategory1 with { Name = newName };
        updatedEntity.Should().BeEquivalentTo(expectedEntity);
        updateResult.Should().HaveValue(expectedEntity.ToModel());
    }

    [Fact]
    public void Update_ReturnsNotFound_WhenNotFound() {
        // arrange
        var updateModel = new CategoryCreateModel("Some category");

        // act
        var updateResult = _categoryService.Update(42, updateModel);

        // assert
        updateResult.Should().BeNotFound();
    }

    [Fact]
    public void Delete_Deletes_WhenExistingId() {
        // arrange
        var testCategory1 = new ProductCategoryEntity { Name = "Some category" };
        var insertedEntity = _referenceDbContext.ProductCategories.Add(testCategory1);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();

        // act
        _categoryService.Delete(insertedEntity.Entity.Id);

        // assert
        var deletedEntity = _referenceDbContext.ProductCategories.Find(insertedEntity.Entity.Id);
        deletedEntity.Should().BeNull();
    }

    [Fact]
    public void Delete_Deletes_WhenProductsAreInCategory() {
        // arrange
        var saleItem = new SaleItemEntity {
            Deleted = false,
            Name = "Some sale item"
        };
        var testCategory1 = new ProductCategoryEntity {
            Name = "Some category",
            Products =
            {
                saleItem
            }
        };
        var insertedEntity = _referenceDbContext.ProductCategories.Add(testCategory1);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();

        // act
        _categoryService.Delete(insertedEntity.Entity.Id);

        // assert
        var deletedEntity = _referenceDbContext.ProductCategories.Find(insertedEntity.Entity.Id);
        deletedEntity.Should().BeNull();
        saleItem = _referenceDbContext.SaleItems
            .Include(si => si.Categories)
            .First(si => si.Id == saleItem.Id);
        saleItem.Categories.Should().BeEmpty();
    }
}
