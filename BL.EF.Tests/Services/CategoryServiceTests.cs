using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace BL.EF.Tests.Services;

public class CategoryServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable
{
    private readonly CategoryService _categoryService;
    private readonly KisDbContext _dbContext;

    public CategoryServiceTests(KisDbContextFactory dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
        _categoryService = new CategoryService(_dbContext);
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
    public void ReadAll_ReadsAll()
    {
        // arrange
        var testCategory1 = new ProductCategoryEntity { Name = "Some category" };
        var testCategory2 = new ProductCategoryEntity { Name = "Some category 2" };
        _dbContext.ProductCategories.Add(testCategory1);
        _dbContext.ProductCategories.Add(testCategory2);
        _dbContext.SaveChanges();

        // act
        var readModels = _categoryService.ReadAll();

        // assert
        var mappedModels = _dbContext.ProductCategories.ToList().ToModels();
        readModels.Should().BeEquivalentTo(mappedModels);
    }

    [Fact]
    public void Create_CreatesCategory_WhenDataIsValid()
    {
        // arrange
        var createModel = new CategoryCreateModel("Some category");

        // act
        var createdModel = _categoryService.Create(createModel);

        // assert
        var createdEntity = _dbContext.ProductCategories.Find(createdModel.Id);
        var expectedEntity = new ProductCategoryEntity { Id = createdModel.Id, Name = createModel.Name };
        createdEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void Update_UpdatesName_WhenExistingId()
    {
        // arrange
        const string oldName = "Some category";
        const string newName = "Some category 2";
        var testCategory1 = new ProductCategoryEntity { Name = oldName };
        var insertedEntity = _dbContext.ProductCategories.Add(testCategory1);
        _dbContext.SaveChanges();
        var updateModel = new CategoryUpdateModel(insertedEntity.Entity.Id, newName);
        _dbContext.ChangeTracker.Clear();

        // act
        var updateSuccess = _categoryService.Update(updateModel);

        // assert
        updateSuccess.Should().BeTrue();
        var updatedEntity = _dbContext.ProductCategories.Find(insertedEntity.Entity.Id);
        var expectedEntity = insertedEntity.Entity with { Name = newName };
        updatedEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void Update_ReturnsFalse_WhenNotFound()
    {
        // arrange
        var updateModel = new CategoryUpdateModel(42, "Some category");

        // act
        var updateSuccess = _categoryService.Update(updateModel);

        // assert
        updateSuccess.Should().BeFalse();
    }

    [Fact]
    public void Delete_Deletes_WhenExistingId()
    {
        // arrange
        var testCategory1 = new ProductCategoryEntity { Name = "Some category" };
        var insertedEntity = _dbContext.ProductCategories.Add(testCategory1);
        _dbContext.SaveChanges();

        // act
        var deleteSuccess = _categoryService.Delete(insertedEntity.Entity.Id);

        // assert
        deleteSuccess.Should().BeTrue();
        var deletedEntity = _dbContext.ProductCategories.Find(insertedEntity.Entity.Id);
        deletedEntity.Should().BeNull();
    }

    [Fact]
    public void Delete_Deletes_WhenProductsAreInCategory()
    {
        // arrange
        var saleItem = new SaleItemEntity
        {
            Deleted = false,
            Name = "Some sale item"
        };
        var testCategory1 = new ProductCategoryEntity
        {
            Name = "Some category",
            Products =
            {
                saleItem
            }
        };
        var insertedEntity = _dbContext.ProductCategories.Add(testCategory1);
        _dbContext.SaveChanges();

        // act
        var deleteSuccess = _categoryService.Delete(insertedEntity.Entity.Id);

        // assert
        deleteSuccess.Should().BeTrue();
        var deletedEntity = _dbContext.ProductCategories.Find(insertedEntity.Entity.Id);
        deletedEntity.Should().BeNull();
        saleItem.Categories.Should().BeEmpty();
    }

    [Fact]
    public void Delete_ReturnsFalse_WhenNotFound()
    {
        // act
        var deleteSuccess = _categoryService.Delete(42);

        // assert
        deleteSuccess.Should().BeFalse();
    }
}