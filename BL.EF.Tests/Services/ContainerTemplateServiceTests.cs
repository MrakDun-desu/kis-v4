using BL.EF.Tests.Extensions;
using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace BL.EF.Tests.Services;

public class ContainerTemplateServiceTests : IClassFixture<KisDbContextFactory>,
    IDisposable,
    IAsyncDisposable
{
    private readonly KisDbContext _dbContext;
    private readonly ContainerTemplateService _templateService;

    public ContainerTemplateServiceTests(KisDbContextFactory dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
        _templateService = new ContainerTemplateService(_dbContext);
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
    public void ReadAll_ReadsAll_WhenNoFilters()
    {
        // arrange
        var testStoreItem1 = new StoreItemEntity
        {
            IsContainerItem = true,
            Name = "Some container item"
        };
        var testStoreItem2 = new StoreItemEntity
        {
            IsContainerItem = true,
            Name = "Some other container item"
        };
        var testTemplate1 = new ContainerTemplateEntity
        {
            Amount = 10,
            ContainedItem = testStoreItem1,
            Name = "Some container"
        };
        var testTemplate2 = new ContainerTemplateEntity
        {
            Amount = 10,
            ContainedItem = testStoreItem2,
            Name = "Some container",
            Deleted = true
        };
        _dbContext.ContainerTemplates.Add(testTemplate1);
        _dbContext.ContainerTemplates.Add(testTemplate2);
        _dbContext.SaveChanges();

        // act
        var readResult = _templateService.ReadAll(null, null);

        // assert
        var expectedModels = _dbContext.ContainerTemplates
            .Include(ct => ct.ContainedItem)
            .ToList().ToModels();

        readResult.Should().HaveValue(expectedModels);
    }

    [Fact]
    public void ReadAll_ReadsCorrectly_WhenFilteringByDeleted()
    {
        // arrange
        var testStoreItem1 = new StoreItemEntity
        {
            IsContainerItem = true,
            Name = "Some container item"
        };
        var testStoreItem2 = new StoreItemEntity
        {
            IsContainerItem = true,
            Name = "Some other container item"
        };
        var testTemplate1 = new ContainerTemplateEntity
        {
            Amount = 10,
            ContainedItem = testStoreItem1,
            Name = "Some container"
        };
        var testTemplate2 = new ContainerTemplateEntity
        {
            Amount = 10,
            ContainedItem = testStoreItem2,
            Name = "Some container",
            Deleted = true
        };
        _dbContext.ContainerTemplates.Add(testTemplate1);
        _dbContext.ContainerTemplates.Add(testTemplate2);
        _dbContext.SaveChanges();

        // act
        var readResult = _templateService.ReadAll(true, null);

        // assert
        var expectedModels = _dbContext.ContainerTemplates
            .Where(ct => ct.Deleted)
            .Include(ct => ct.ContainedItem)
            .ToList().ToModels();

        readResult.Should().HaveValue(expectedModels);
    }

    [Fact]
    public void ReadAll_ReadsCorrectly_WhenFilteringByStoreItemId()
    {
        // arrange
        var testStoreItem1 = new StoreItemEntity
        {
            IsContainerItem = true,
            Name = "Some container item"
        };
        var testStoreItem2 = new StoreItemEntity
        {
            IsContainerItem = true,
            Name = "Some other container item"
        };
        var testTemplate1 = new ContainerTemplateEntity
        {
            Amount = 10,
            ContainedItem = testStoreItem1,
            Name = "Some container"
        };
        var testTemplate2 = new ContainerTemplateEntity
        {
            Amount = 10,
            ContainedItem = testStoreItem2,
            Name = "Some container",
            Deleted = true
        };
        _dbContext.ContainerTemplates.Add(testTemplate1);
        _dbContext.ContainerTemplates.Add(testTemplate2);
        _dbContext.SaveChanges();

        // act
        var readResult = _templateService.ReadAll(null, testStoreItem1.Id);

        // assert
        var expectedModels = _dbContext.ContainerTemplates
            .Where(ct => ct.ContainedItemId == testStoreItem1.Id)
            .Include(ct => ct.ContainedItem)
            .ToList().ToModels();

        readResult.Should().HaveValue(expectedModels);
    }

    [Fact]
    public void ReadAll_ReturnsErrors_WhenFilteringByNonexistentStoreItem()
    {
        // arrange
        const int containedItemId = 42;

        // act
        var readResult = _templateService.ReadAll(null, containedItemId);

        // assert
        readResult.Should().HaveValue(new Dictionary<string, string[]>
        {
            { nameof(containedItemId), [$"Store item with id {containedItemId} doesn't exist"] }
        });
    }

    [Fact]
    public void Create_Creates_WhenDataIsValid()
    {
        // arrange
        var testStoreItem = new StoreItemEntity
        {
            Name = "Some store item",
            IsContainerItem = true,
            Deleted = false
        };
        _dbContext.StoreItems.Add(testStoreItem);
        _dbContext.SaveChanges();
        var createModel = new ContainerTemplateCreateModel(
            "Some container template",
            testStoreItem.Id,
            10
        );

        // act
        var creationResult = _templateService.Create(createModel);

        // assert
        creationResult.IsT0.Should().BeTrue();
        var createdModel = creationResult.AsT0;
        var createdEntity = _dbContext.ContainerTemplates.Find(createdModel.Id);
        var expectedEntity = new ContainerTemplateEntity
        {
            Id = createdModel.Id,
            Name = createModel.Name,
            Amount = createModel.Amount,
            ContainedItem = testStoreItem,
            ContainedItemId = testStoreItem.Id
        };

        createdEntity.Should().BeEquivalentTo(expectedEntity);
        createdModel.Should().BeEquivalentTo(expectedEntity.ToModel());
    }

    [Fact]
    public void Create_ReturnsErrors_WhenStoreItemIsInvalid()
    {
        // arrange
        var testStoreItem = new StoreItemEntity
        {
            Name = "Some store item",
            IsContainerItem = false,
            Deleted = true
        };
        _dbContext.StoreItems.Add(testStoreItem);
        _dbContext.SaveChanges();
        var createModel = new ContainerTemplateCreateModel(
            "Some container template",
            testStoreItem.Id,
            10
        );

        // act
        var creationResult = _templateService.Create(createModel);

        // assert
        creationResult.Should().HaveValue(new Dictionary<string, string[]>
        {
            {
                nameof(createModel.ContainedItemId),
                [
                    $"Store item with id {createModel.ContainedItemId} has been marked as deleted",
                    $"Store item with id {createModel.ContainedItemId} is not a container item"
                ]
            }
        });
    }

    // [Fact]
    // public void Update_Updates_WhenDataIsValid()
    // {
    //     // arrange
    //     var testStoreItem1 = new StoreItemEntity
    //     {
    //         Name = "Some store item",
    //         IsContainerItem = true
    //     };
    //     var testStoreItem2 = new StoreItemEntity
    //     {
    //         Name = "Some store item",
    //         IsContainerItem = true
    //     };
    //     var testTemplate = new ContainerTemplateEntity
    //     {
    //         Name = "Some container item",
    //         Amount = 10,
    //         ContainedItem = testStoreItem1,
    //         Deleted = true
    //     };
    //     _dbContext.ContainerTemplates.Add(testTemplate);
    //     _dbContext.StoreItems.Add(testStoreItem2);
    //     _dbContext.SaveChanges();
    //     _dbContext.ChangeTracker.Clear();
    //     var updateModel = new ContainerTemplateUpdateModel(
    //         testTemplate.Id,
    //         "Some container template",
    //         testStoreItem2.Id,
    //         15
    //     );
    //
    //     // act
    //     var updateResult = _templateService.Update(updateModel);
    //
    //     // assert
    //     updateResult.Should().BeSuccess();
    //     var updatedEntity = _dbContext.ContainerTemplates.Find(testTemplate.Id);
    //     var expectedEntity = testTemplate with
    //     {
    //         Name = updateModel.Name,
    //         Amount = updateModel.Amount,
    //         ContainedItem = testStoreItem2,
    //         ContainedItemId = testStoreItem2.Id,
    //         Deleted = false
    //     };
    //     updatedEntity.Should().BeEquivalentTo(expectedEntity);
    // }
    //
    // [Fact]
    // public void Update_ReturnsNotFound_WhenTemplateIsNotFound()
    // {
    //     // arrange
    //     var updateModel = new ContainerTemplateUpdateModel(
    //         42,
    //         "Some container template",
    //         42,
    //         15
    //     );
    //
    //     // act
    //     var updateResult = _templateService.Update(updateModel);
    //
    //     // assert
    //     updateResult.Should().BeNotFound();
    // }
    //
    // [Fact]
    // public void Update_ReturnsErrors_WhenStoreItemIsInvalid()
    // {
    //     // arrange
    //     var testStoreItem1 = new StoreItemEntity
    //     {
    //         Name = "Some store item",
    //         IsContainerItem = true
    //     };
    //     var testStoreItem2 = new StoreItemEntity
    //     {
    //         Name = "Some store item",
    //         IsContainerItem = false,
    //         Deleted = true
    //     };
    //     var testTemplate = new ContainerTemplateEntity
    //     {
    //         Name = "Some container item",
    //         Amount = 10,
    //         ContainedItem = testStoreItem1,
    //         Deleted = true
    //     };
    //     _dbContext.ContainerTemplates.Add(testTemplate);
    //     _dbContext.StoreItems.Add(testStoreItem2);
    //     _dbContext.SaveChanges();
    //     _dbContext.ChangeTracker.Clear();
    //     var updateModel = new ContainerTemplateUpdateModel(
    //         testTemplate.Id,
    //         "Some container template",
    //         testStoreItem2.Id,
    //         15
    //     );
    //
    //     // act
    //     var updateResult = _templateService.Update(updateModel);
    //
    //     // assert
    //     updateResult.Should().HaveValue(new Dictionary<string, string[]>
    //     {
    //         {
    //             nameof(updateModel.ContainedItemId),
    //             [
    //                 $"Store item with id {updateModel.ContainedItemId} has been marked as deleted",
    //                 $"Store item with id {updateModel.ContainedItemId} is not a container item"
    //             ]
    //         }
    //     });
    // }
    //
    // [Fact]
    // public void Delete_Deletes_WhenExistingId()
    // {
    //     // arrange
    //     var testStoreItem1 = new StoreItemEntity
    //     {
    //         Name = "Some store item",
    //         IsContainerItem = true
    //     };
    //     var testTemplate = new ContainerTemplateEntity
    //     {
    //         Name = "Some container item",
    //         Amount = 10,
    //         ContainedItem = testStoreItem1,
    //         Deleted = false
    //     };
    //     _dbContext.ContainerTemplates.Add(testTemplate);
    //     _dbContext.SaveChanges();
    //     _dbContext.ChangeTracker.Clear();
    //
    //     // act
    //     var deleteResult = _templateService.Delete(testTemplate.Id);
    //
    //     // assert
    //     deleteResult.Should().BeSuccess();
    //     var deletedEntity = _dbContext.ContainerTemplates.Find(testTemplate.Id);
    //     var expectedEntity = testTemplate with { Deleted = true };
    //     deletedEntity.Should().BeEquivalentTo(expectedEntity, opts =>
    //         opts.Excluding(ct => ct.ContainedItem));
    // }
    //
    // [Fact]
    // public void Delete_ReturnsNotFound_WhenNotFound()
    // {
    //     // act
    //     var deleteResult = _templateService.Delete(42);
    //
    //     // assert
    //     deleteResult.Should().BeNotFound();
    // }
}