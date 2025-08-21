using BL.EF.Tests.Extensions;
using BL.EF.Tests.Fixtures;
using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common;
using KisV4.Common.Enums;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.Extensions.Time.Testing;

namespace BL.EF.Tests.Services;

[Collection(DockerDatabaseTests.Name)]
public class StoreTransactionServiceTests : IDisposable, IAsyncDisposable {
    private readonly StoreTransactionService _storeTransactionService;
    private readonly UserService _userService;
    private readonly FakeTimeProvider _timeProvider = new();
    private readonly KisDbContext _referenceDbContext;
    private readonly KisDbContext _normalDbContext;

    public StoreTransactionServiceTests(KisDbContextFactory dbContextFactory) {
        (_referenceDbContext, _normalDbContext) = dbContextFactory.CreateDbContextAndReference();
        _userService = new UserService(
                _normalDbContext,
                new CurrencyChangeService(_normalDbContext),
                new DiscountUsageService(_normalDbContext)
            );
        _storeTransactionService = new StoreTransactionService(_normalDbContext, _userService, _timeProvider);
        AssertionOptions.AssertEquivalencyUsing(options =>
            options.Using<DateTimeOffset>(ctx =>
                ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1))).WhenTypeIs<DateTimeOffset>()
        );
    }

    public void Dispose() {
        GC.SuppressFinalize(this);
        _referenceDbContext.Dispose();
        _normalDbContext.Dispose();
    }

    public async ValueTask DisposeAsync() {
        GC.SuppressFinalize(this);
        await _referenceDbContext.DisposeAsync();
        await _normalDbContext.DisposeAsync();
    }

    [Fact]
    public void ReadAll_ReadsAll_WhenNoFilters() {
        // arrange
        var testUser1 = new UserAccountEntity { UserName = "Some user" };
        var testUser2 = new UserAccountEntity { UserName = "Some other user" };
        var testStoreItem = new StoreItemEntity { Name = "Beer" };
        var testStore = new StoreEntity { Name = "Kachna" };
        var testTransaction1 = new StoreTransactionEntity {
            Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
            Cancelled = false,
            ResponsibleUser = testUser1,
            TransactionReason = TransactionReason.Sale,
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    StoreItem = testStoreItem,
                    ItemAmount = -10,
                    Cancelled = false,
                    Store = testStore
                }
            }
        };
        var testTransaction2 = new StoreTransactionEntity {
            Timestamp = DateTimeOffset.UtcNow.AddDays(-5),
            Cancelled = false,
            ResponsibleUser = testUser2,
            TransactionReason = TransactionReason.Sale,
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    StoreItem = testStoreItem,
                    ItemAmount = -10,
                    Cancelled = false,
                    Store = testStore
                }
            }
        };
        var testTransaction3 = new StoreTransactionEntity {
            Timestamp = DateTimeOffset.UtcNow.AddDays(-1),
            Cancelled = true,
            ResponsibleUser = testUser1,
            TransactionReason = TransactionReason.Sale,
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    StoreItem = testStoreItem,
                    ItemAmount = -10,
                    Cancelled = true,
                    Store = testStore
                }
            }
        };

        _referenceDbContext.StoreTransactions.Add(testTransaction1);
        _referenceDbContext.StoreTransactions.Add(testTransaction2);
        _referenceDbContext.StoreTransactions.Add(testTransaction3);
        _referenceDbContext.SaveChanges();

        // act
        var readResult = _storeTransactionService.ReadAll(null, null, null, null, null);

        // assert
        readResult.IsT0.Should().BeTrue();
        readResult.AsT0.Should().BeEquivalentTo(new Page<StoreTransactionListModel>(
                new List<StoreTransactionEntity>
                {
                    testTransaction3,
                    testTransaction2,
                    testTransaction1,
                }.ToModels(),
                new PageMeta(1, Constants.DefaultPageSize, 1, 3, 3, 1)),
            opts =>
                opts.WithStrictOrderingFor(page => page.Data));
    }

    [Fact]
    public void ReadAll_ReadsAll_WhenFilteringByDate() {
        // arrange
        var testUser1 = new UserAccountEntity { UserName = "Some user" };
        var testUser2 = new UserAccountEntity { UserName = "Some other user" };
        var testStoreItem = new StoreItemEntity { Name = "Beer" };
        var testStore = new StoreEntity { Name = "Kachna" };
        var testTransaction1 = new StoreTransactionEntity {
            Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
            Cancelled = false,
            ResponsibleUser = testUser1,
            TransactionReason = TransactionReason.Sale,
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    StoreItem = testStoreItem,
                    ItemAmount = -10,
                    Cancelled = false,
                    Store = testStore
                }
            }
        };
        var testTransaction2 = new StoreTransactionEntity {
            Timestamp = DateTimeOffset.UtcNow.AddDays(-5),
            Cancelled = false,
            ResponsibleUser = testUser2,
            TransactionReason = TransactionReason.Sale,
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    StoreItem = testStoreItem,
                    ItemAmount = -10,
                    Cancelled = false,
                    Store = testStore
                }
            }
        };
        var testTransaction3 = new StoreTransactionEntity {
            Timestamp = DateTimeOffset.UtcNow.AddDays(-1),
            Cancelled = true,
            ResponsibleUser = testUser1,
            TransactionReason = TransactionReason.Sale,
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    StoreItem = testStoreItem,
                    ItemAmount = -10,
                    Cancelled = true,
                    Store = testStore
                }
            }
        };

        _referenceDbContext.StoreTransactions.Add(testTransaction1);
        _referenceDbContext.StoreTransactions.Add(testTransaction2);
        _referenceDbContext.StoreTransactions.Add(testTransaction3);
        _referenceDbContext.SaveChanges();

        // act
        var readResult = _storeTransactionService.ReadAll(
            null, null,
            DateTimeOffset.UtcNow.AddDays(-6),
            DateTimeOffset.UtcNow.AddDays(-3),
            null);

        // assert
        readResult.IsT0.Should().BeTrue();
        readResult.AsT0.Should().BeEquivalentTo(new Page<StoreTransactionListModel>(
                new List<StoreTransactionEntity>
                {
                    testTransaction2,
                }.ToModels(),
                new PageMeta(1, Constants.DefaultPageSize, 1, 1, 1, 1)),
            opts =>
                opts.WithStrictOrderingFor(page => page.Data));
    }

    [Fact]
    public void ReadAll_ReadsAll_WhenFilteringByCancelled() {
        // arrange
        var testUser1 = new UserAccountEntity { UserName = "Some user" };
        var testUser2 = new UserAccountEntity { UserName = "Some other user" };
        var testStoreItem = new StoreItemEntity { Name = "Beer" };
        var testStore = new StoreEntity { Name = "Kachna" };
        var testTransaction1 = new StoreTransactionEntity {
            Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
            Cancelled = false,
            ResponsibleUser = testUser1,
            TransactionReason = TransactionReason.Sale,
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    StoreItem = testStoreItem,
                    ItemAmount = -10,
                    Cancelled = false,
                    Store = testStore
                }
            }
        };
        var testTransaction2 = new StoreTransactionEntity {
            Timestamp = DateTimeOffset.UtcNow.AddDays(-5),
            Cancelled = false,
            ResponsibleUser = testUser2,
            TransactionReason = TransactionReason.Sale,
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    StoreItem = testStoreItem,
                    ItemAmount = -10,
                    Cancelled = false,
                    Store = testStore
                }
            }
        };
        var testTransaction3 = new StoreTransactionEntity {
            Timestamp = DateTimeOffset.UtcNow.AddDays(-1),
            Cancelled = true,
            ResponsibleUser = testUser1,
            TransactionReason = TransactionReason.Sale,
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    StoreItem = testStoreItem,
                    ItemAmount = -10,
                    Cancelled = true,
                    Store = testStore
                }
            }
        };

        _referenceDbContext.StoreTransactions.Add(testTransaction1);
        _referenceDbContext.StoreTransactions.Add(testTransaction2);
        _referenceDbContext.StoreTransactions.Add(testTransaction3);
        _referenceDbContext.SaveChanges();

        // act
        var readResult = _storeTransactionService.ReadAll(null, null, null, null, false);

        // assert
        readResult.IsT0.Should().BeTrue();
        readResult.AsT0.Should().BeEquivalentTo(new Page<StoreTransactionListModel>(
                new List<StoreTransactionEntity>
                {
                    testTransaction2,
                    testTransaction1,
                }.ToModels(),
                new PageMeta(1, Constants.DefaultPageSize, 1, 2, 2, 1)),
            opts =>
                opts.WithStrictOrderingFor(page => page.Data));
    }

    [Fact]
    public void ReadSelfCancellable_ReadsCorrectly() {
        // arrange
        var testUser1 = new UserAccountEntity { UserName = "Some user" };
        var testUser2 = new UserAccountEntity { UserName = "Some other user" };
        var testStoreItem = new StoreItemEntity { Name = "Beer" };
        var testStore = new StoreEntity { Name = "Kachna" };
        var testTransaction1 = new StoreTransactionEntity {
            Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
            Cancelled = false,
            ResponsibleUser = testUser1,
            TransactionReason = TransactionReason.Sale,
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    StoreItem = testStoreItem,
                    ItemAmount = -10,
                    Cancelled = false,
                    Store = testStore
                }
            }
        };
        var testTransaction2 = new StoreTransactionEntity {
            Timestamp = DateTimeOffset.UtcNow.AddMinutes(-10),
            Cancelled = false,
            ResponsibleUser = testUser2,
            TransactionReason = TransactionReason.Sale,
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    StoreItem = testStoreItem,
                    ItemAmount = -10,
                    Cancelled = false,
                    Store = testStore
                }
            }
        };
        var testTransaction3 = new StoreTransactionEntity {
            Timestamp = DateTimeOffset.UtcNow.AddMinutes(-10),
            Cancelled = false,
            ResponsibleUser = testUser1,
            TransactionReason = TransactionReason.Sale,
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    StoreItem = testStoreItem,
                    ItemAmount = -10,
                    Cancelled = false,
                    Store = testStore
                }
            }
        };

        _referenceDbContext.StoreTransactions.Add(testTransaction1);
        _referenceDbContext.StoreTransactions.Add(testTransaction2);
        _referenceDbContext.StoreTransactions.Add(testTransaction3);
        _referenceDbContext.SaveChanges();
        _timeProvider.SetUtcNow(DateTimeOffset.UtcNow);

        // act
        var readResult = _storeTransactionService.ReadSelfCancellable(testUser1.UserName);

        // assert
        readResult.Should().BeEquivalentTo(
            new List<StoreTransactionEntity>
            {
                testTransaction3,
            }.ToModels(),
            opts =>
                opts.WithStrictOrdering());
    }

    [Fact]
    public void Read_ReadsCorrectly() {
        // arrange
        var testUser1 = new UserAccountEntity { UserName = "Some user" };
        var testStoreItem = new StoreItemEntity { Name = "Beer" };
        var testStore = new StoreEntity { Name = "Kachna" };
        var testTransaction1 = new StoreTransactionEntity {
            Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
            Cancelled = false,
            ResponsibleUser = testUser1,
            TransactionReason = TransactionReason.Sale,
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    StoreItem = testStoreItem,
                    ItemAmount = -10,
                    Cancelled = false,
                    Store = testStore
                }
            }
        };
        _referenceDbContext.StoreTransactions.Add(testTransaction1);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();
        _timeProvider.SetUtcNow(DateTimeOffset.UtcNow);

        // act
        var readResult = _storeTransactionService.Read(testTransaction1.Id);

        // assert
        readResult.Should().HaveValue(
            new StoreTransactionDetailModel(
                testTransaction1.Id,
                testTransaction1.ResponsibleUser.ToListModel(),
                testTransaction1.Timestamp,
                testTransaction1.Cancelled,
                testTransaction1.TransactionReason,
                null,
                testTransaction1.StoreTransactionItems.ToList().ToModels()
            ));
    }

    [Fact]
    public void Create_Creates_WhenDataIsValid() {
        // arrange
        var testStore1 = new StoreEntity { Name = "Kachna" };
        var testStoreItem1 = new StoreItemEntity { Name = "Beer" };
        var testUser = new UserAccountEntity { UserName = "Some user" };
        _referenceDbContext.Stores.Add(testStore1);
        _referenceDbContext.StoreItems.Add(testStoreItem1);
        _referenceDbContext.UserAccounts.Add(testUser);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();

        var creationTime = DateTimeOffset.UtcNow;
        _timeProvider.SetUtcNow(creationTime);
        var createModel = new StoreTransactionCreateModel(
            [
                new StoreTransactionItemCreateModel(testStoreItem1.Id, 42)
            ],
            TransactionReason.AddingToStore,
            testStore1.Id,
            null
        );

        // act
        var createResult = _storeTransactionService.Create(createModel, testUser.UserName);

        // assert
        createResult.IsT0.Should().BeTrue();
        createResult.Should().HaveValue(new StoreTransactionDetailModel(
            1,
            testUser.ToListModel(),
            creationTime,
            false,
            createModel.TransactionReason,
            null,
            [
                new StoreTransactionItemListModel(
                    1,
                    testStoreItem1.ToListModel(),
                    testStore1.ToListModel(),
                    1,
                    42,
                    false
                )
            ]
        ));
    }

    [Fact]
    public void Create_ReturnsErrors_WhenDataIsNotValid() {
        // arrange
        var testStoreItem1 = new StoreItemEntity {
            Name = "Beer",
            IsContainerItem = true
        };
        _referenceDbContext.StoreItems.Add(testStoreItem1);
        _referenceDbContext.SaveChanges();

        var creationTime = DateTimeOffset.UtcNow;
        _timeProvider.SetUtcNow(creationTime);
        var createModel = new StoreTransactionCreateModel(
            [
                new StoreTransactionItemCreateModel(testStoreItem1.Id, 42),
                new StoreTransactionItemCreateModel(24, 42)
            ],
            TransactionReason.MovingStores,
            42,
            null
        );

        // act
        var createResult = _storeTransactionService.Create(createModel, "Some user");

        // assert
        createResult.IsT1.Should().BeTrue();
        createResult.Should().HaveValue(new Dictionary<string, string[]>
            {
                {
                    nameof(createModel.StoreId),
                    [$"Store with id {createModel.StoreId} doesn't exist"]
                },
                {
                    nameof(createModel.DestinationStoreId),
                    ["Destination store has to be specified when moving stores"]
                },
                {
                    nameof(createModel.StoreTransactionItems),
                    [
                        "Some of the specified store items do not exist",
                        "Some of the specified store items are container items. " +
                        "It's not possible to manually create store transactions " +
                        "for container items"
                    ]
                }
            }
        );
    }

    [Fact]
    public void Delete_DeletesCorrectly_WhenExistingId() {
        // arrange
        var testUser1 = new UserAccountEntity { UserName = "Some user" };
        var testStoreItem = new StoreItemEntity { Name = "Beer" };
        var testStore = new StoreEntity { Name = "Kachna" };
        var testTransaction1 = new StoreTransactionEntity {
            Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
            ResponsibleUser = testUser1,
            TransactionReason = TransactionReason.Sale,
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    StoreItem = testStoreItem,
                    ItemAmount = -10,
                    Store = testStore
                }
            }
        };
        _referenceDbContext.StoreTransactions.Add(testTransaction1);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();
        _timeProvider.SetUtcNow(DateTimeOffset.UtcNow);

        // act
        var deleteResult = _storeTransactionService.Delete(testTransaction1.Id);

        // assert
        var cancelledItems = testTransaction1.StoreTransactionItems
            .Select(item => item with { Cancelled = true }).ToList();
        deleteResult.Should().HaveValue(
            new StoreTransactionDetailModel(
                testTransaction1.Id,
                testTransaction1.ResponsibleUser.ToListModel(),
                testTransaction1.Timestamp,
                true,
                testTransaction1.TransactionReason,
                null,
                cancelledItems.ToModels()
            ));

    }
}
