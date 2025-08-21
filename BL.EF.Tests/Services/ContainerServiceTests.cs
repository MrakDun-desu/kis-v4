using BL.EF.Tests.Extensions;
using BL.EF.Tests.Fixtures;
using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using KisV4.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;

namespace BL.EF.Tests.Services;

[Collection(DockerDatabaseTests.Name)]
public class ContainerServiceTests : IClassFixture<KisDbContextFactory>,
    IDisposable, IAsyncDisposable {
    private readonly ContainerService _containerService;
    private readonly KisDbContext _referenceDbContext;
    private readonly KisDbContext _normalDbContext;
    private readonly FakeTimeProvider _timeProvider = new();

    public ContainerServiceTests(KisDbContextFactory dbContextFactory) {
        (_referenceDbContext, _normalDbContext) = dbContextFactory.CreateDbContextAndReference();
        _containerService = new ContainerService(
            _normalDbContext,
            _timeProvider,
            new UserService(
                _normalDbContext,
                new CurrencyChangeService(_normalDbContext),
                new DiscountUsageService(_normalDbContext))
            );
        AssertionOptions.AssertEquivalencyUsing(options =>
            options.Using<DateTimeOffset>(ctx =>
                ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1))).WhenTypeIs<DateTimeOffset>()
        );
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
    public void ReadAll_ReadsAll_WhenNoFilters() {
        // arrange
        var testStoreItem = new StoreItemEntity {
            Name = "Test store item"
        };
        var testPipe = new PipeEntity {
            Name = "Some pipe"
        };
        var testContainerTemplate1 = new ContainerTemplateEntity {
            Amount = 10,
            ContainedItem = testStoreItem,
            Name = "Some container",
            Instances =
            {
                new ContainerEntity { Deleted = true },
                new ContainerEntity()
            }
        };
        var testContainerTemplate2 = new ContainerTemplateEntity {
            Amount = 10,
            ContainedItem = testStoreItem,
            Name = "Some other container",
            Instances =
            {
                new ContainerEntity { Pipe = testPipe },
                new ContainerEntity()
            }
        };
        _referenceDbContext.ContainerTemplates.Add(testContainerTemplate1);
        _referenceDbContext.ContainerTemplates.Add(testContainerTemplate2);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();

        // act
        var readPage =
            _containerService.ReadAll(null, null, null, null);

        // assert
        var expectedPage =
            _referenceDbContext.Containers
            .Include(c => c.StoreTransactionItems)
            .Include(c => c.Template).ThenInclude(ct => ct!.ContainedItem)
            .Include(c => c.Pipe)
                .Select(c => new ContainerIntermediateModel(c, 0))
                .Page(1, Constants.DefaultPageSize, Mapper.ToModels);

        readPage.Should().HaveValue(expectedPage.Value);
    }

    [Fact]
    public void ReadAll_ReadsCorrectly_WhenFilteringByDeleted() {
        // arrange
        var testStoreItem = new StoreItemEntity {
            Name = "Test store item"
        };
        var testPipe = new PipeEntity {
            Name = "Some pipe"
        };
        var testContainerTemplate1 = new ContainerTemplateEntity {
            Amount = 10,
            ContainedItem = testStoreItem,
            Name = "Some container",
            Instances =
            {
                new ContainerEntity { Deleted = true },
                new ContainerEntity()
            }
        };
        var testContainerTemplate2 = new ContainerTemplateEntity {
            Amount = 10,
            ContainedItem = testStoreItem,
            Name = "Some other container",
            Instances =
            {
                new ContainerEntity { Pipe = testPipe },
                new ContainerEntity()
            }
        };
        _referenceDbContext.ContainerTemplates.Add(testContainerTemplate1);
        _referenceDbContext.ContainerTemplates.Add(testContainerTemplate2);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();

        // act
        var readPage =
            _containerService.ReadAll(null, null, true, null);

        // assert
        var expectedPage =
            _referenceDbContext.Containers
            .Include(c => c.StoreTransactionItems)
            .Include(c => c.Template).ThenInclude(ct => ct!.ContainedItem)
            .Include(c => c.Pipe)
                .Where(c => c.Deleted)
                .Select(c => new ContainerIntermediateModel(c, 0))
                .Page(1, Constants.DefaultPageSize, Mapper.ToModels);

        readPage.Should().HaveValue(expectedPage.Value);
    }

    [Fact]
    public void ReadAll_ReturnsErrors_WhenFilteringByNonexistentPipe() {
        // arrange
        const int pipeId = 42;

        // act
        var readPage =
            _containerService.ReadAll(null, null, true, pipeId);

        // assert
        readPage.Should().HaveValue(new Dictionary<string, string[]>
        {
            { nameof(pipeId), [$"Pipe with id {pipeId} doesn't exist"] }
        });
    }

    [Fact]
    public void ReadAll_ReadsCorrectly_WhenFilteringByPipe() {
        // arrange
        var testStoreItem = new StoreItemEntity {
            Name = "Test store item"
        };
        var testPipe = new PipeEntity {
            Name = "Some pipe"
        };
        var testContainerTemplate1 = new ContainerTemplateEntity {
            Amount = 10,
            ContainedItem = testStoreItem,
            Name = "Some container",
            Instances =
            {
                new ContainerEntity { Deleted = true },
                new ContainerEntity()
            }
        };
        var testContainerTemplate2 = new ContainerTemplateEntity {
            Amount = 10,
            ContainedItem = testStoreItem,
            Name = "Some other container",
            Instances =
            {
                new ContainerEntity { Pipe = testPipe },
                new ContainerEntity()
            }
        };
        _referenceDbContext.ContainerTemplates.Add(testContainerTemplate1);
        _referenceDbContext.ContainerTemplates.Add(testContainerTemplate2);
        _referenceDbContext.SaveChanges();

        // act
        var readPage =
            _containerService.ReadAll(null, null, null, testPipe.Id);

        // assert
        var expectedPage =
            _referenceDbContext.Containers
                .Where(c => c.PipeId == testPipe.Id)
                .Select(c => new ContainerIntermediateModel(c, 0))
                .Page(1, Constants.DefaultPageSize, Mapper.ToModels);

        readPage.Should().HaveValue(expectedPage.Value);
    }

    [Fact]
    public void ReadAll_ReadsCorrectly_WhenAggregating() {
        // arrange
        var testStoreItem = new StoreItemEntity {
            Name = "Test store item"
        };
        var testPipe = new PipeEntity {
            Name = "Some pipe"
        };
        var testContainerTemplate1 = new ContainerTemplateEntity {
            Amount = 10,
            ContainedItem = testStoreItem,
            Name = "Some container",
            Instances =
            {
                new ContainerEntity
                {
                    StoreTransactionItems =
                    {
                        new StoreTransactionItemEntity
                        {
                            StoreItem = testStoreItem,
                            ItemAmount = 10,
                            StoreTransaction = new StoreTransactionEntity
                            {
                                Timestamp = DateTimeOffset.UtcNow,
                                ResponsibleUser = new UserAccountEntity { UserName = "Some user" }
                            }
                        },
                        new StoreTransactionItemEntity
                        {
                            StoreItem = testStoreItem,
                            ItemAmount = 42,
                            Cancelled = true,
                            StoreTransaction = new StoreTransactionEntity
                            {
                                Timestamp = DateTimeOffset.UtcNow,
                                ResponsibleUser = new UserAccountEntity { UserName = "Some another user" }
                            }
                        },
                        new StoreTransactionItemEntity
                        {
                            StoreItem = testStoreItem,
                            ItemAmount = -2,
                            StoreTransaction = new StoreTransactionEntity
                            {
                                Timestamp = DateTimeOffset.UtcNow,
                                ResponsibleUser = new UserAccountEntity { UserName = "Some other user" }
                            }
                        }
                    },
                    Pipe = testPipe
                }
            }
        };
        _referenceDbContext.ContainerTemplates.Add(testContainerTemplate1);
        _referenceDbContext.SaveChanges();

        // act
        var readPage =
            _containerService.ReadAll(null, null, null, null);

        // assert
        var expectedPage =
            _referenceDbContext.Containers
                .Where(c => c.PipeId == testPipe.Id)
                .Select(c => new ContainerIntermediateModel(c, 8))
                .Page(1, Constants.DefaultPageSize, Mapper.ToModels);

        readPage.Should().HaveValue(expectedPage.Value);
    }

    [Fact]
    public void Create_Creates_WhenDataIsValid() {
        // arrange
        var testStoreItem = new StoreItemEntity { Name = "Some store item" };
        var testContainerTemplate = new ContainerTemplateEntity {
            Amount = 10,
            ContainedItem = testStoreItem,
            Name = "Some container"
        };
        var testPipe = new PipeEntity { Name = "Some pipe" };
        _referenceDbContext.ContainerTemplates.Add(testContainerTemplate);
        _referenceDbContext.Pipes.Add(testPipe);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();
        var timestamp = DateTimeOffset.UtcNow;
        _timeProvider.SetUtcNow(timestamp);
        var createModel = new ContainerCreateModel(testContainerTemplate.Id, testPipe.Id);

        // act
        var creationResult = _containerService.Create(createModel, "Some user");

        // assert
        var expectedModel = new ContainerListModel(
            ((ContainerListModel)creationResult.Value).Id,
            timestamp,
            new PipeListModel(testPipe.Id, testPipe.Name),
            false,
            testContainerTemplate.ToModel(),
            testContainerTemplate.Amount
        );
        creationResult.Should().HaveValue(expectedModel);
    }

    [Fact]
    public void Create_ReturnsErrors_WhenDataIsNotValid() {
        // arrange
        var createModel = new ContainerCreateModel(42, 42);

        // act
        var creationResult = _containerService.Create(createModel, "Some user");

        // assert
        creationResult.Should().HaveValue(
            new Dictionary<string, string[]>
            {
                {
                    nameof(createModel.TemplateId),
                    [$"Container template with id {createModel.TemplateId} doesn't exist"]
                },
                {
                    nameof(createModel.PipeId),
                    [$"Pipe with id {createModel.PipeId} doesn't exist"]
                }
            }
        );
    }

    [Fact]
    public void Update_RestoresEntity_IfWasDeleted() {
        // arrange
        var testPipe = new PipeEntity {
            Name = "Some pipe"
        };
        var testPipe2 = new PipeEntity {
            Name = "Some pipe 2"
        };
        var testContainer1 = new ContainerEntity {
            Template = new ContainerTemplateEntity {
                Name = "Some template",
                Amount = 10,
                ContainedItem = new StoreItemEntity {
                    Name = "Some store item"
                }
            },
            Pipe = testPipe,
            Name = "Some template",
            Deleted = true
        };
        _referenceDbContext.Containers.Add(testContainer1);
        _referenceDbContext.Pipes.Add(testPipe2);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();
        var updateModel = new ContainerPatchModel(testPipe2.Id);

        // act
        var updateResult = _containerService.Patch(testContainer1.Id, updateModel);

        // assert
        var updatedEntity = _referenceDbContext.Containers
            .Include(ce => ce.Pipe)
            .Include(ce => ce.Template)
            .ThenInclude(ct => ct!.ContainedItem)
            .Single(ce => ce.Id == testContainer1.Id);
        testContainer1.PipeId = testPipe2.Id;
        testContainer1.Pipe = testPipe2;
        testContainer1.Deleted = false;
        updatedEntity.Should().BeEquivalentTo(testContainer1, opts => opts.IgnoringCyclicReferences());
    }

    [Fact]
    public void Update_UpdatesPipe_WhenExistingId() {
        // arrange
        var testPipe = new PipeEntity {
            Name = "Some pipe"
        };
        var testPipe2 = new PipeEntity {
            Name = "Some pipe 2"
        };
        var testContainer1 = new ContainerEntity {
            Template = new ContainerTemplateEntity {
                Name = "Some template",
                Amount = 10,
                ContainedItem = new StoreItemEntity {
                    Name = "Some store item"
                }
            },
            Pipe = testPipe,
            Name = "Some template"
        };
        _referenceDbContext.Containers.Add(testContainer1);
        _referenceDbContext.Pipes.Add(testPipe2);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();
        var updateModel = new ContainerPatchModel(testPipe2.Id);

        // act
        var updateResult = _containerService.Patch(testContainer1.Id, updateModel);

        // assert
        var updatedEntity = _referenceDbContext.Containers
            .Include(ce => ce.Pipe)
            .Include(ce => ce.Template)
            .ThenInclude(ct => ct!.ContainedItem)
            .Single(ce => ce.Id == testContainer1.Id);
        testContainer1.PipeId = testPipe2.Id;
        testContainer1.Pipe = testPipe2;
        updatedEntity.Should().BeEquivalentTo(testContainer1, opts => opts.IgnoringCyclicReferences());
    }

    [Fact]
    public void Update_ReturnsErrors_WhenNonexistentPipe() {
        // arrange
        var testPipe = new PipeEntity {
            Name = "Some pipe"
        };
        var testContainer1 = new ContainerEntity {
            Template = new ContainerTemplateEntity {
                Name = "Some template",
                Amount = 10,
                ContainedItem = new StoreItemEntity {
                    Name = "Some store item"
                }
            },
            Pipe = testPipe,
            Name = "Some template"
        };
        _referenceDbContext.Containers.Add(testContainer1);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();
        var updateModel = new ContainerPatchModel(42);

        // act
        var updateResult = _containerService.Patch(testContainer1.Id, updateModel);

        // assert
        updateResult.Should().HaveValue(
            new Dictionary<string, string[]>
            {
                { nameof(updateModel.PipeId), [$"Pipe with id {updateModel.PipeId} doesn't exist"] }
            });
    }

    [Fact]
    public void Update_ReturnsNotFound_WhenNotFoundContainer() {
        // arrange
        var updateModel = new ContainerPatchModel(42);

        // act
        var updateResult = _containerService.Patch(42, updateModel);

        // assert
        updateResult.Should().BeNotFound();
    }

    [Fact]
    public void Delete_ReturnsNotFound_WhenNotFoundContainer() {
        // act
        var deleteResult = _containerService.Delete(42, "Some user");

        // assert
        deleteResult.Should().BeNotFound();
    }

    [Fact]
    public void Delete_MakesLeftoverAmount0_WhenDeleting() {
        // arrange
        var testStoreItem = new StoreItemEntity { Name = "Test store item" };
        var testPipe = new PipeEntity { Name = "Some pipe" };
        var testContainer = new ContainerEntity {
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    StoreItem = testStoreItem,
                    ItemAmount = 10,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        Timestamp = DateTimeOffset.UtcNow,
                        TransactionReason = TransactionReason.AddingToStore,
                        ResponsibleUser = new UserAccountEntity { UserName = "Some user" }
                    }
                },
                new StoreTransactionItemEntity
                {
                    StoreItem = testStoreItem,
                    ItemAmount = 42,
                    Cancelled = true,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        Timestamp = DateTimeOffset.UtcNow,
                        TransactionReason = TransactionReason.AddingToStore,
                        ResponsibleUser = new UserAccountEntity { UserName = "Some another user" }
                    }
                },
                new StoreTransactionItemEntity
                {
                    StoreItem = testStoreItem,
                    ItemAmount = -2,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        Timestamp = DateTimeOffset.UtcNow,
                        TransactionReason = TransactionReason.Sale,
                        ResponsibleUser = new UserAccountEntity { UserName = "Some other user" }
                    }
                }
            },
            Pipe = testPipe
        };
        var testContainerTemplate = new ContainerTemplateEntity {
            Amount = 10,
            ContainedItem = testStoreItem,
            Name = "Some container",
            Instances = { testContainer }
        };
        _referenceDbContext.ContainerTemplates.Add(testContainerTemplate);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();

        // act
        var deleteResult =
            _containerService.Delete(testContainer.Id, "Some user");

        // assert
        testContainer = _referenceDbContext.Containers
            .Include(con => con.StoreTransactionItems)
            .ThenInclude(sti => sti.StoreTransaction)
            .First(con => con.Id == testContainer.Id);
        testContainer.Deleted.Should().BeTrue();
        testContainer.PipeId.Should().BeNull();
        var lastTransactionItem = testContainer.StoreTransactionItems
            .Last();
        lastTransactionItem.Should().BeEquivalentTo(new StoreTransactionItemEntity {
            StoreItemId = testStoreItem.Id,
            StoreItem = testStoreItem,
            ItemAmount = -8,
            Store = testContainer,
            StoreId = testContainer.Id,
            StoreTransaction = new StoreTransactionEntity {
                TransactionReason = TransactionReason.WriteOff
            }
        },
            opts => opts
                .Excluding(st => st.Id)
                .Excluding(st => st.StoreTransaction)
                .Excluding(st => st.StoreTransactionId)
                .Including(st => st.StoreTransaction!.TransactionReason)
        );
        testContainer.StoreTransactionItems
            .Where(sti => !sti.Cancelled)
            .Sum(sti => sti.ItemAmount).Should().Be(0);
    }
}
