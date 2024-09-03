using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace BL.EF.Tests.Services;

public class PipeServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable
{
    private readonly KisDbContext _dbContext;
    private readonly PipeService _pipeService;

    public PipeServiceTests(KisDbContextFactory dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
        _pipeService = new PipeService(_dbContext);
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
    public void Create_CreatesPipe_WhenDataIsValid()
    {
        var createModel = new PipeCreateModel("Some pipe");
        var createdId = _pipeService.Create(createModel);

        var createdEntity = _dbContext.Pipes.Find(createdId);
        var expectedEntity = new PipeEntity { Id = createdId, Name = createModel.Name };
        createdEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void ReadAll_ReadsAll()
    {
        var testPipe1 = new PipeEntity { Name = "Some pipe" };
        var testPipe2 = new PipeEntity { Name = "Some pipe 2" };
        _dbContext.Pipes.Add(testPipe1);
        _dbContext.Pipes.Add(testPipe2);
        _dbContext.SaveChanges();

        var readModels = _pipeService.ReadAll();
        var mappedModels = _dbContext.Pipes.ToList().ToModels();

        readModels.Should().BeEquivalentTo(mappedModels);
    }

    // [Fact]
    // public void Update_UpdatesName_WhenExistingId()
    // {
    //     const string oldName = "Some pipe";
    //     const string newName = "Some pipe 2";
    //     var testPipe1 = new PipeEntity { Name = oldName };
    //     var insertedEntity = _dbContext.Pipes.Add(testPipe1);
    //     _dbContext.SaveChanges();
    //     var updateModel = new PipeUpdateModel(newName);
    //
    //     var updateSuccess = _pipeService.Update(insertedEntity.Entity.Id, updateModel);
    //
    //     updateSuccess.Should().BeTrue();
    //     var updatedEntity = _dbContext.Pipes.Find(insertedEntity.Entity.Id);
    //     var expectedEntity = insertedEntity.Entity with { Name = newName };
    //     updatedEntity.Should().BeEquivalentTo(expectedEntity);
    // }
    //
    // [Fact]
    // public void Update_ReturnsFalse_WhenNotFound()
    // {
    //     var updateModel = new PipeUpdateModel("Some pipe");
    //
    //     var updateSuccess = _pipeService.Update(42, updateModel);
    //
    //     updateSuccess.Should().BeFalse();
    // }

    [Fact]
    public void Delete_Deletes_WhenExistingId()
    {
        var testPipe1 = new PipeEntity { Name = "Some pipe" };
        var insertedEntity = _dbContext.Pipes.Add(testPipe1);
        _dbContext.SaveChanges();

        var deleteSuccess = _pipeService.Delete(insertedEntity.Entity.Id);

        deleteSuccess.Should().BeTrue();
        var deletedEntity = _dbContext.Pipes.Find(insertedEntity.Entity.Id);
        deletedEntity.Should().BeNull();
    }

    [Fact]
    public void Delete_ReturnsFalse_WhenNotFound()
    {
        var deleteSuccess = _pipeService.Delete(42);

        deleteSuccess.Should().BeFalse();
    }
}