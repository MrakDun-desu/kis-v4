using BL.EF.Tests.Extensions;
using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using KisV4.DAL.EF.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;

namespace BL.EF.Tests.Services;

public class ContainerServiceTests : IClassFixture<KisDbContextFactory>,
    IDisposable, IAsyncDisposable
{
    private readonly ContainerService _containerService;
    private readonly KisDbContext _dbContext;
    private readonly FakeTimeProvider _timeProvider = new();

    public ContainerServiceTests(KisDbContextFactory dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
        _containerService = new ContainerService(_dbContext, _timeProvider, new UserService(_dbContext));
        AssertionOptions.AssertEquivalencyUsing(options =>
            options.Using<DateTimeOffset>(ctx =>
                ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1))).WhenTypeIs<DateTimeOffset>()
        );
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
        var testStoreItem = new StoreItemEntity
        {
            Name = "Test store item"
        };
        var testPipe = new PipeEntity
        {
            Name = "Some pipe"
        };
        var testContainerTemplate1 = new ContainerTemplateEntity
        {
            Amount = 10,
            ContainedItem = testStoreItem,
            Name = "Some container",
            Instances =
            {
                new ContainerEntity { Deleted = true },
                new ContainerEntity()
            }
        };
        var testContainerTemplate2 = new ContainerTemplateEntity
        {
            Amount = 10,
            ContainedItem = testStoreItem,
            Name = "Some other container",
            Instances =
            {
                new ContainerEntity { Pipe = testPipe },
                new ContainerEntity()
            }
        };
        _dbContext.ContainerTemplates.Add(testContainerTemplate1);
        _dbContext.ContainerTemplates.Add(testContainerTemplate2);
        _dbContext.SaveChanges();

        // act
        var readPage =
            _containerService.ReadAll(null, null, null, null);

        // assert
        var expectedPage =
            _dbContext.Containers
                .Select(c => new ContainerIntermediateModel(c, 0))
                .Page(1, Constants.DefaultPageSize, Mapper.ToModels);

        readPage.Should().HaveValue(expectedPage.Value);
    }

    [Fact]
    public void ReadAll_ReadsCorrectly_WhenFilteringByDeleted()
    {
        // arrange
        var testStoreItem = new StoreItemEntity
        {
            Name = "Test store item"
        };
        var testPipe = new PipeEntity
        {
            Name = "Some pipe"
        };
        var testContainerTemplate1 = new ContainerTemplateEntity
        {
            Amount = 10,
            ContainedItem = testStoreItem,
            Name = "Some container",
            Instances =
            {
                new ContainerEntity { Deleted = true },
                new ContainerEntity()
            }
        };
        var testContainerTemplate2 = new ContainerTemplateEntity
        {
            Amount = 10,
            ContainedItem = testStoreItem,
            Name = "Some other container",
            Instances =
            {
                new ContainerEntity { Pipe = testPipe },
                new ContainerEntity()
            }
        };
        _dbContext.ContainerTemplates.Add(testContainerTemplate1);
        _dbContext.ContainerTemplates.Add(testContainerTemplate2);
        _dbContext.SaveChanges();

        // act
        var readPage =
            _containerService.ReadAll(null, null, true, null);

        // assert
        var expectedPage =
            _dbContext.Containers
                .Where(c => c.Deleted)
                .Select(c => new ContainerIntermediateModel(c, 0))
                .Page(1, Constants.DefaultPageSize, Mapper.ToModels);

        readPage.Should().HaveValue(expectedPage.Value);
    }

    [Fact]
    public void ReadAll_ReturnsErrors_WhenFilteringByNonexistentPipe()
    {
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
    public void ReadAll_ReadsCorrectly_WhenFilteringByPipe()
    {
        // arrange
        var testStoreItem = new StoreItemEntity
        {
            Name = "Test store item"
        };
        var testPipe = new PipeEntity
        {
            Name = "Some pipe"
        };
        var testContainerTemplate1 = new ContainerTemplateEntity
        {
            Amount = 10,
            ContainedItem = testStoreItem,
            Name = "Some container",
            Instances =
            {
                new ContainerEntity { Deleted = true },
                new ContainerEntity()
            }
        };
        var testContainerTemplate2 = new ContainerTemplateEntity
        {
            Amount = 10,
            ContainedItem = testStoreItem,
            Name = "Some other container",
            Instances =
            {
                new ContainerEntity { Pipe = testPipe },
                new ContainerEntity()
            }
        };
        _dbContext.ContainerTemplates.Add(testContainerTemplate1);
        _dbContext.ContainerTemplates.Add(testContainerTemplate2);
        _dbContext.SaveChanges();

        // act
        var readPage =
            _containerService.ReadAll(null, null, null, testPipe.Id);

        // assert
        var expectedPage =
            _dbContext.Containers
                .Where(c => c.PipeId == testPipe.Id)
                .Select(c => new ContainerIntermediateModel(c, 0))
                .Page(1, Constants.DefaultPageSize, Mapper.ToModels);

        readPage.Should().HaveValue(expectedPage.Value);
    }

    [Fact]
    public void ReadAll_ReadsCorrectly_WhenAggregating()
    {
        // arrange
        var testStoreItem = new StoreItemEntity
        {
            Name = "Test store item"
        };
        var testPipe = new PipeEntity
        {
            Name = "Some pipe"
        };
        var testContainerTemplate1 = new ContainerTemplateEntity
        {
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
        _dbContext.ContainerTemplates.Add(testContainerTemplate1);
        _dbContext.SaveChanges();

        // act
        var readPage =
            _containerService.ReadAll(null, null, null, null);

        // assert
        var expectedPage =
            _dbContext.Containers
                .Where(c => c.PipeId == testPipe.Id)
                .Select(c => new ContainerIntermediateModel(c, 8))
                .Page(1, Constants.DefaultPageSize, Mapper.ToModels);

        readPage.Should().HaveValue(expectedPage.Value);
    }

    [Fact]
    public void Create_Creates_WhenDataIsValid()
    {
        // arrange
        var testStoreItem = new StoreItemEntity { Name = "Some store item" };
        var testContainerTemplate = new ContainerTemplateEntity
        {
            Amount = 10,
            ContainedItem = testStoreItem,
            Name = "Some container"
        };
        var testPipe = new PipeEntity { Name = "Some pipe" };
        _dbContext.ContainerTemplates.Add(testContainerTemplate);
        _dbContext.Pipes.Add(testPipe);
        _dbContext.SaveChanges();
        var timestamp = DateTimeOffset.UtcNow;
        _timeProvider.SetUtcNow(timestamp);
        var createModel = new ContainerCreateModel(testContainerTemplate.Id, testPipe.Id);

        // act
        var creationResult = _containerService.Create(createModel, "Some user");

        // assert
        var expectedModel = new ContainerReadAllModel(
            ((ContainerReadAllModel)creationResult.Value).Id,
            timestamp,
            new PipeReadAllModel(testPipe.Id, testPipe.Name),
            false,
            testContainerTemplate.ToModel(),
            testContainerTemplate.Amount
        );
        creationResult.Should().HaveValue(expectedModel);
    }

    [Fact]
    public void Create_ReturnsErrors_WhenDataIsNotValid()
    {
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
    public void Update_RestoresEntity_IfWasDeleted()
    {
        // arrange
        var testPipe = new PipeEntity
        {
            Name = "Some pipe"
        };
        var testPipe2 = new PipeEntity
        {
            Name = "Some pipe 2"
        };
        var testContainer1 = new ContainerEntity
        {
            Template = new ContainerTemplateEntity
            {
                Name = "Some template",
                Amount = 10,
                ContainedItem = new StoreItemEntity
                {
                    Name = "Some store item"
                }
            },
            Pipe = testPipe,
            Name = "Some template",
            Deleted = true
        };
        _dbContext.Containers.Add(testContainer1);
        _dbContext.Pipes.Add(testPipe2);
        _dbContext.SaveChanges();
        _dbContext.ChangeTracker.Clear();
        var updateModel = new ContainerUpdateModel(testContainer1.Id, testPipe2.Id);

        // act
        var updateResult = _containerService.Update(updateModel);

        // assert
        updateResult.Should().BeSuccess();
        var updatedEntity = _dbContext.Containers
            .Include(ce => ce.Pipe)
            .Single(ce => ce.Id == testContainer1.Id);
        var expectedEntity = testContainer1 with
        {
            PipeId = testPipe2.Id,
            Pipe = testPipe2,
            Deleted = false
        };
        updatedEntity.Should().BeEquivalentTo(expectedEntity,
            opts =>
                opts.Excluding(ce => ce.Template));
    }

    [Fact]
    public void Update_UpdatesPipe_WhenExistingId()
    {
        // arrange
        var testPipe = new PipeEntity
        {
            Name = "Some pipe"
        };
        var testPipe2 = new PipeEntity
        {
            Name = "Some pipe 2"
        };
        var testContainer1 = new ContainerEntity
        {
            Template = new ContainerTemplateEntity
            {
                Name = "Some template",
                Amount = 10,
                ContainedItem = new StoreItemEntity
                {
                    Name = "Some store item"
                }
            },
            Pipe = testPipe,
            Name = "Some template"
        };
        _dbContext.Containers.Add(testContainer1);
        _dbContext.Pipes.Add(testPipe2);
        _dbContext.SaveChanges();
        _dbContext.ChangeTracker.Clear();
        var updateModel = new ContainerUpdateModel(testContainer1.Id, testPipe2.Id);

        // act
        var updateResult = _containerService.Update(updateModel);

        // assert
        updateResult.Should().BeSuccess();
        var updatedEntity = _dbContext.Containers
            .Include(ce => ce.Pipe)
            .Single(ce => ce.Id == testContainer1.Id);
        var expectedEntity = testContainer1 with
        {
            PipeId = testPipe2.Id,
            Pipe = testPipe2
        };
        updatedEntity.Should().BeEquivalentTo(expectedEntity,
            opts =>
                opts.Excluding(ce => ce.Template));
    }

    [Fact]
    public void Update_ReturnsErrors_WhenNonexistentPipe()
    {
        // arrange
        var testPipe = new PipeEntity
        {
            Name = "Some pipe"
        };
        var testContainer1 = new ContainerEntity
        {
            Template = new ContainerTemplateEntity
            {
                Name = "Some template",
                Amount = 10,
                ContainedItem = new StoreItemEntity
                {
                    Name = "Some store item"
                }
            },
            Pipe = testPipe,
            Name = "Some template"
        };
        _dbContext.Containers.Add(testContainer1);
        _dbContext.SaveChanges();
        _dbContext.ChangeTracker.Clear();
        var updateModel = new ContainerUpdateModel(testContainer1.Id, 42);

        // act
        var updateResult = _containerService.Update(updateModel);

        // assert
        updateResult.Should().HaveValue(
            new Dictionary<string, string[]>
            {
                { nameof(updateModel.PipeId), [$"Pipe with id {updateModel.PipeId} doesn't exist"] }
            });
    }

    [Fact]
    public void Update_ReturnsNotFound_WhenNotFoundContainer()
    {
        // arrange
        var updateModel = new ContainerUpdateModel(42, 42);

        // act
        var updateResult = _containerService.Update(updateModel);

        // assert
        updateResult.Should().BeNotFound();
    }

    [Fact]
    public void Delete_ReturnsNotFound_WhenNotFoundContainer()
    {
        // act
        var deleteResult = _containerService.Delete(42, "Some user");

        // assert
        deleteResult.Should().BeNotFound();
    }

    [Fact]
    public void Delete_MakesLeftoverAmount0_WhenDeleting()
    {
        // arrange
        var testStoreItem = new StoreItemEntity { Name = "Test store item" };
        var testPipe = new PipeEntity { Name = "Some pipe" };
        var testContainer = new ContainerEntity
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
        var testContainerTemplate = new ContainerTemplateEntity
        {
            Amount = 10,
            ContainedItem = testStoreItem,
            Name = "Some container",
            Instances = { testContainer }
        };
        _dbContext.ContainerTemplates.Add(testContainerTemplate);
        _dbContext.SaveChanges();

        // act
        var deleteResult =
            _containerService.Delete(testContainer.Id, "Some user");

        // assert
        deleteResult.Should().BeSuccess();
        testContainer.Deleted.Should().BeTrue();
        testContainer.PipeId.Should().BeNull();
        var lastStoreTransaction = testContainer.StoreTransactionItems.Last();
        lastStoreTransaction.Should().BeEquivalentTo(new StoreTransactionItemEntity
            {
                StoreItemId = testStoreItem.Id,
                StoreItem = testStoreItem,
                ItemAmount = -8,
                Store = testContainer,
                StoreId = testContainer.Id,
                StoreTransaction = new StoreTransactionEntity
                {
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