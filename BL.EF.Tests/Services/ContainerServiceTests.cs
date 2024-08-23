using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.Extensions.Time.Testing;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;

namespace BL.EF.Tests.Services;

public class ContainerServiceTests : IClassFixture<KisDbContextFactory>,
    IDisposable, IAsyncDisposable {
    private readonly ContainerService _containerService;
    private readonly KisDbContext _dbContext;
    private readonly Mapper _mapper;
    private readonly FakeTimeProvider _timeProvider = new();

    public ContainerServiceTests(KisDbContextFactory dbContextFactory) {
        _dbContext = dbContextFactory.CreateDbContext();
        _mapper = new Mapper();
        _containerService = new ContainerService(_dbContext, _mapper, _timeProvider);
    }

    // [Fact]
    // public void ReadAll_DoesntReadDeleted() {
    //     var testStoreItem = new StoreItemEntity {
    //         Name = "Test store item"
    //     };
    //     var testContainer1 = new ContainerEntity {
    //         Name = "Test container 1",
    //         ContainedItem = testStoreItem,
    //     };
    //     var testContainer2 = new ContainerEntity {
    //         Name = "Test container 2",
    //         ContainedItem = testStoreItem,
    //         Deleted = true
    //     };
    //     _dbContext.Containers.Add(testContainer1);
    //     _dbContext.Containers.Add(testContainer2);
    //     _dbContext.SaveChanges();
    //
    //     var readModels = _containerService.ReadAll();
    //     var mappedModels =
    //         _mapper.ToModels(_dbContext.Containers.Where(c => !c.Deleted).ToList());
    //
    //     readModels.Should().BeEquivalentTo(mappedModels);
    // }
    //
    // [Fact]
    // public void Update_UpdatesName() {
    //     const string oldName = "Test container 1";
    //     const string newName = "New Name";
    //     var testStoreItem = new StoreItemEntity {
    //         Name = "Test store item"
    //     };
    //     var testContainer1 = new ContainerEntity {
    //         Name = oldName,
    //         ContainedItem = testStoreItem,
    //     };
    //     var addedContainer = _dbContext.Containers.Add(testContainer1);
    //     _dbContext.SaveChanges();
    //     var expectedEntity = addedContainer.Entity with { Name = newName };
    //     var updateModel = new ContainerUpdateModel(newName, null);
    //
    //     var updateSuccess = _containerService.Update(addedContainer.Entity.Id, updateModel);
    //
    //     updateSuccess.Should().BeTrue();
    //     var updatedEntity = _dbContext.Containers.Find(addedContainer.Entity.Id);
    //     updatedEntity.Should().BeEquivalentTo(expectedEntity);
    // }
    //
    // [Fact]
    // public void Update_UpdatesPipe_AndAddsOpenedDate_WhenFirstUpdated() {
    //     var testStoreItem = new StoreItemEntity {
    //         Name = "Test store item"
    //     };
    //     var testPipe = new PipeEntity { Name = "Test pipe" };
    //     var testContainer1 = new ContainerEntity {
    //         Name = "Test container 1",
    //         ContainedItem = testStoreItem,
    //     };
    //     var addedContainer = _dbContext.Containers.Add(testContainer1);
    //     var addedPipe = _dbContext.Pipes.Add(testPipe);
    //     _dbContext.SaveChanges();
    //     var pipeId = addedPipe.Entity.Id;
    //     var openedDate = DateTimeOffset.UtcNow;
    //     _timeProvider.SetUtcNow(openedDate);
    //     var expectedEntity = addedContainer.Entity with {
    //         PipeId = pipeId,
    //         OpenSince = openedDate,
    //         Pipe = addedPipe.Entity
    //     };
    //     var updateModel = new ContainerUpdateModel(null, pipeId);
    //
    //     var updateSuccess = _containerService.Update(addedContainer.Entity.Id, updateModel);
    //
    //     updateSuccess.Should().BeTrue();
    //     var updatedEntity = _dbContext.Containers.Find(addedContainer.Entity.Id);
    //     updatedEntity.Should().BeEquivalentTo(expectedEntity);
    // }
    //
    // [Fact]
    // public void Update_UpdatesPipe_AndDoesntChangeOpenedDate_WhenUpdatedLater() {
    //     var testStoreItem = new StoreItemEntity {
    //         Name = "Test store item"
    //     };
    //     var testPipe1 = new PipeEntity { Name = "Test pipe 1" };
    //     var testPipe2 = new PipeEntity { Name = "Test pipe 2" };
    //     var testContainer1 = new ContainerEntity {
    //         Name = "Test container 1",
    //         ContainedItem = testStoreItem,
    //     };
    //     var addedContainer = _dbContext.Containers.Add(testContainer1);
    //     var addedPipe1 = _dbContext.Pipes.Add(testPipe1);
    //     var addedPipe2 = _dbContext.Pipes.Add(testPipe2);
    //     _dbContext.SaveChanges();
    //     var pipeId1 = addedPipe1.Entity.Id;
    //     var pipeId2 = addedPipe2.Entity.Id;
    //     var openedDate = DateTimeOffset.UtcNow;
    //     _timeProvider.SetUtcNow(openedDate);
    //     var expectedEntity = addedContainer.Entity with {
    //         PipeId = pipeId1,
    //         OpenSince = openedDate,
    //         Pipe = addedPipe1.Entity
    //     };
    //     var updateModel = new ContainerUpdateModel(null, pipeId1);
    //
    //     var updateSuccess = _containerService.Update(addedContainer.Entity.Id, updateModel);
    //
    //     updateSuccess.Should().BeTrue();
    //     var updatedEntity = _dbContext.Containers.Find(addedContainer.Entity.Id);
    //     updatedEntity.Should().BeEquivalentTo(expectedEntity);
    //
    //     _timeProvider.SetUtcNow(openedDate.AddDays(1));
    //     expectedEntity.PipeId = pipeId2;
    //     expectedEntity.Pipe = addedPipe2.Entity;
    //     updateModel = new ContainerUpdateModel(null, pipeId2);
    //
    //     updateSuccess = _containerService.Update(addedContainer.Entity.Id, updateModel);
    //
    //     updateSuccess.Should().BeTrue();
    //     updatedEntity = _dbContext.Containers.Find(addedContainer.Entity.Id);
    //     updatedEntity.Should().BeEquivalentTo(expectedEntity);
    // }

    public void Dispose() {
        _dbContext.Dispose();
    }

    public async ValueTask DisposeAsync() {
        await _dbContext.DisposeAsync();
    }
}
