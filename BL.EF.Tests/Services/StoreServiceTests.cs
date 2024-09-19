using BL.EF.Tests.Fixtures;
using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace BL.EF.Tests.Services;

public class StoreServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable
{
    private readonly KisDbContext _dbContext;
    private readonly StoreService _storeService;

    public StoreServiceTests(KisDbContextFactory dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContextAndResetDb();
        _storeService = new StoreService(dbContextFactory.CreateDbContext());
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
    public void Create_CreatesStore_WhenDataIsValid()
    {
        var createModel = new StoreCreateModel("Some store");
        var createdId = _storeService.Create(createModel);

        var createdEntity = _dbContext.Stores.Find(createdId);
        var expectedEntity = new StoreEntity { Id = createdId, Name = createModel.Name };
        createdEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void ReadAll_ReadsAll()
    {
        var testStore1 = new StoreEntity { Name = "Some store" };
        var testStore2 = new StoreEntity { Name = "Some store 2" };
        _dbContext.Stores.Add(testStore1);
        _dbContext.Stores.Add(testStore2);
        _dbContext.SaveChanges();

        var readModels = _storeService.ReadAll();
        var mappedModels = _dbContext.Stores.ToList().ToModels();

        readModels.Should().BeEquivalentTo(mappedModels);
    }
    //
    // [Fact]
    // public void Update_UpdatesName_WhenExistingId()
    // {
    //     const string oldName = "Some store";
    //     const string newName = "Some store 2";
    //     var testStore1 = new StoreEntity { Name = oldName };
    //     var insertedEntity = _dbContext.Stores.Add(testStore1);
    //     _dbContext.SaveChanges();
    //     var updateModel = new StoreUpdateModel(newName);
    //
    //     var updateSuccess = _storeService.Update(insertedEntity.Entity.Id, updateModel);
    //
    //     updateSuccess.Should().BeTrue();
    //     var updatedEntity = _dbContext.Stores.Find(insertedEntity.Entity.Id);
    //     var expectedEntity = insertedEntity.Entity with { Name = newName };
    //     updatedEntity.Should().BeEquivalentTo(expectedEntity);
    // }
    //
    // [Fact]
    // public void Update_ReturnsFalse_WhenNotFound()
    // {
    //     var updateModel = new StoreUpdateModel("Some store");
    //
    //     var updateSuccess = _storeService.Update(42, updateModel);
    //
    //     updateSuccess.Should().BeFalse();
    // }

    [Fact]
    public void Delete_Deletes_WhenExistingId()
    {
        var testStore1 = new StoreEntity { Name = "Some store" };
        var insertedEntity = _dbContext.Stores.Add(testStore1);
        _dbContext.SaveChanges();

        var deleteSuccess = _storeService.Delete(insertedEntity.Entity.Id);

        deleteSuccess.Should().BeTrue();
        var deletedEntity = _dbContext.Stores.Find(insertedEntity.Entity.Id);
        deletedEntity.Should().BeNull();
    }

    [Fact]
    public void Delete_ReturnsFalse_WhenNotFound()
    {
        var deleteSuccess = _storeService.Delete(42);

        deleteSuccess.Should().BeFalse();
    }
}