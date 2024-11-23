using BL.EF.Tests.Extensions;
using BL.EF.Tests.Fixtures;
using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace BL.EF.Tests.Services;

public class PipeServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable
{
    private readonly KisDbContext _referenceDbContext;
    private readonly KisDbContext _normalDbContext;
    private readonly PipeService _pipeService;

    public PipeServiceTests(KisDbContextFactory dbContextFactory)
    {
        (_referenceDbContext, _normalDbContext) = dbContextFactory.CreateDbContextAndReference();
        _pipeService = new PipeService(_normalDbContext);
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
    public void Create_CreatesPipe_WhenDataIsValid()
    {
        var createModel = new PipeCreateModel("Some pipe");
        var createdModel = _pipeService.Create(createModel);

        var createdEntity = _referenceDbContext.Pipes.Find(createdModel.Id);
        var expectedEntity = new PipeEntity { Id = createdModel.Id, Name = createModel.Name };
        createdEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void ReadAll_ReadsAll()
    {
        var testPipe1 = new PipeEntity { Name = "Some pipe" };
        var testPipe2 = new PipeEntity { Name = "Some pipe 2" };
        _referenceDbContext.Pipes.Add(testPipe1);
        _referenceDbContext.Pipes.Add(testPipe2);
        _referenceDbContext.SaveChanges();

        var readModels = _pipeService.ReadAll();
        var mappedModels = _referenceDbContext.Pipes.ToList().ToModels();

        readModels.Should().BeEquivalentTo(mappedModels);
    }

    [Fact]
    public void Update_UpdatesName_WhenExistingId()
    {
        const string oldName = "Some pipe";
        const string newName = "Some pipe 2";
        var testPipe1 = new PipeEntity { Name = oldName };
        var insertedEntity = _referenceDbContext.Pipes.Add(testPipe1);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();
        var updateModel = new PipeCreateModel(newName);

        var updateResult = _pipeService.Update(insertedEntity.Entity.Id, updateModel);

        updateResult.Should().HaveValue(new PipeListModel(testPipe1.Id, newName));
        var updatedEntity = _referenceDbContext.Pipes.Find(insertedEntity.Entity.Id);
        var expectedEntity = insertedEntity.Entity with { Name = newName };
        updatedEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void Update_ReturnsFalse_WhenNotFound()
    {
        var updateModel = new PipeCreateModel("Some pipe");

        var updateResult = _pipeService.Update(42, updateModel);

        updateResult.Should().BeNotFound();
    }

    [Fact]
    public void Delete_Deletes_WhenExistingId()
    {
        var testPipe1 = new PipeEntity { Name = "Some pipe" };
        var insertedEntity = _referenceDbContext.Pipes.Add(testPipe1);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();

        var deleteSuccess = _pipeService.Delete(insertedEntity.Entity.Id);

        deleteSuccess.Should().BeSuccess();
        var deletedEntity = _referenceDbContext.Pipes.Find(insertedEntity.Entity.Id);
        deletedEntity.Should().BeNull();
    }

    [Fact]
    public void Delete_ReturnsError_WhenHasContainers()
    {
        var testPipe1 = new PipeEntity { Name = "Some pipe" };
        var testContainer = new ContainerEntity
        {
            Name = "Some container",
            Pipe = testPipe1,
            Template = new ContainerTemplateEntity
            {
                ContainedItem = new StoreItemEntity { Name = "Some store item" },
                Amount = 10,
                Name = "Some container template"
            }
        };
        _referenceDbContext.Containers.Add(testContainer);
        var insertedEntity = _referenceDbContext.Pipes.Add(testPipe1);
        _referenceDbContext.SaveChanges();

        var deleteResult = _pipeService.Delete(insertedEntity.Entity.Id);

        deleteResult.Should().HaveValue(
            $"Pipe with id {testPipe1.Id} cannot be deleted, currently has a " +
                                         $"container active");
    }

    [Fact]
    public void Delete_ReturnsFalse_WhenNotFound()
    {
        var deleteSuccess = _pipeService.Delete(42);

        deleteSuccess.Should().BeNotFound();
    }
}