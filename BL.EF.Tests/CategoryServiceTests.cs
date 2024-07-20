using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace BL.EF.Tests;

public class CategoryServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable {
    private readonly CategoryService _categoryService;
    private readonly KisDbContext _dbContext;
    private readonly Mapper _mapper;

    public CategoryServiceTests(KisDbContextFactory dbContextFactory) {
        _dbContext = dbContextFactory.CreateDbContext();
        _mapper = new Mapper();
        _categoryService = new CategoryService(_dbContext, _mapper);
    }

    [Fact]
    public void Create_CreatesCategory_WhenDataIsValid() {
        var createModel = new CategoryCreateModel("Some category");
        var createdId = _categoryService.Create(createModel);

        var createdEntity = _dbContext.ProductCategories.Find(createdId);
        var expectedEntity = new ProductCategoryEntity { Id = createdId, Name = createModel.Name };
        createdEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void ReadAll_ReadsAll() {
        var testCategory1 = new ProductCategoryEntity { Name = "Some category" };
        var testCategory2 = new ProductCategoryEntity { Name = "Some category 2" };
        _dbContext.ProductCategories.Add(testCategory1);
        _dbContext.ProductCategories.Add(testCategory2);
        _dbContext.SaveChanges();

        var readModels = _categoryService.ReadAll();
        var mappedModels = _mapper.ToModels(_dbContext.ProductCategories.ToList());

        readModels.Should().BeEquivalentTo(mappedModels);
    }

    [Fact]
    public void Update_UpdatesName_WhenExistingId() {
        const string oldName = "Some category";
        const string newName = "Some category 2";
        var testCategory1 = new ProductCategoryEntity { Name = oldName };
        var insertedEntity = _dbContext.ProductCategories.Add(testCategory1);
        _dbContext.SaveChanges();
        var updateModel = new CategoryUpdateModel(newName);

        var updateSuccess = _categoryService.Update(insertedEntity.Entity.Id, updateModel);

        updateSuccess.Should().BeTrue();
        var updatedEntity = _dbContext.ProductCategories.Find(insertedEntity.Entity.Id);
        var expectedEntity = insertedEntity.Entity with { Name = newName };
        updatedEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void Update_ReturnsFalse_WhenNotFound() {
        var updateModel = new CategoryUpdateModel("Some category");

        var updateSuccess = _categoryService.Update(42, updateModel);

        updateSuccess.Should().BeFalse();
    }

    [Fact]
    public void Delete_Deletes_WhenExistingId() {
        var testCategory1 = new ProductCategoryEntity { Name = "Some category" };
        var insertedEntity = _dbContext.ProductCategories.Add(testCategory1);
        _dbContext.SaveChanges();

        var deleteSuccess = _categoryService.Delete(insertedEntity.Entity.Id);

        deleteSuccess.Should().BeTrue();
        var deletedEntity = _dbContext.ProductCategories.Find(insertedEntity.Entity.Id);
        deletedEntity.Should().BeNull();
    }

    [Fact]
    public void Delete_ReturnsFalse_WhenNotFound() {
        var deleteSuccess = _categoryService.Delete(42);

        deleteSuccess.Should().BeFalse();
    }

    public void Dispose() {
        _dbContext.Dispose();
    }

    public async ValueTask DisposeAsync() {
        await _dbContext.DisposeAsync();
    }
}
