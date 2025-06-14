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
using Microsoft.EntityFrameworkCore;

namespace BL.EF.Tests.Services;

public class StoreItemServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable {
    private readonly KisDbContext _referenceDbContext;
    private readonly KisDbContext _normalDbContext;
    private readonly StoreItemService _storeItemService;

    public StoreItemServiceTests(KisDbContextFactory dbContextFactory) {
        (_referenceDbContext, _normalDbContext) = dbContextFactory.CreateDbContextAndReference();
        _storeItemService = new StoreItemService(_normalDbContext);
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
        var drinkCategory = new ProductCategoryEntity { Name = "Drink" };
        var foodCategory = new ProductCategoryEntity { Name = "Food" };
        var testCurrency = new CurrencyEntity { Name = "Czech Crowns" };
        var testStore = new StoreEntity { Name = "Kachna 1" };
        var testUser = new UserAccountEntity { UserName = "Some user" };
        var testStoreItem1 = new StoreItemEntity {
            Name = "Plzen Beer",
            UnitName = "bottle",
            Categories = { drinkCategory },
            Costs =
            {
                new CurrencyCostEntity
                {
                    Currency = testCurrency,
                    Amount = 30,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-10)
                },
                new CurrencyCostEntity
                {
                    Currency = testCurrency,
                    Amount = 25,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-5)
                }
            },
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    Store = testStore,
                    ItemAmount = -20,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        ResponsibleUser = testUser,
                        Timestamp = DateTimeOffset.UtcNow.AddDays(-5),
                        TransactionReason = TransactionReason.Sale
                    }
                },
                new StoreTransactionItemEntity
                {
                    Store = testStore,
                    ItemAmount = 42,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        ResponsibleUser = testUser,
                        Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                        TransactionReason = TransactionReason.AddingToStore
                    }
                }
            }
        };
        var testStoreItem2 = new StoreItemEntity {
            Name = "Bread",
            UnitName = "slice",
            Categories = { foodCategory },
            Costs =
            {
                new CurrencyCostEntity
                {
                    Currency = testCurrency,
                    Amount = 30,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-10)
                },
                new CurrencyCostEntity
                {
                    Currency = testCurrency,
                    Amount = 42,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-5)
                }
            },
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    Store = testStore,
                    ItemAmount = -42,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        ResponsibleUser = testUser,
                        Timestamp = DateTimeOffset.UtcNow.AddDays(-5),
                        TransactionReason = TransactionReason.Sale
                    }
                },
                new StoreTransactionItemEntity
                {
                    Store = testStore,
                    ItemAmount = 42,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        ResponsibleUser = testUser,
                        Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                        TransactionReason = TransactionReason.AddingToStore
                    }
                }
            }
        };

        _referenceDbContext.StoreItems.Add(testStoreItem1);
        _referenceDbContext.StoreItems.Add(testStoreItem2);
        _referenceDbContext.SaveChanges();

        // act
        var readResult = _storeItemService.ReadAll(null, null, null, null, null);

        // assert
        readResult.IsT0.Should().BeTrue();
        var resultAsT0 = readResult.AsT0;
        resultAsT0.Should().BeEquivalentTo(
            new Page<StoreItemListModel>(
                [
                    new(
                        testStoreItem1.Id,
                        testStoreItem1.Name,
                        testStoreItem1.Image,
                        testStoreItem1.Deleted,
                        testStoreItem1.UnitName,
                        testStoreItem1.BarmanCanStock,
                        testStoreItem1.IsContainerItem
                    ),
                    new(
                        testStoreItem2.Id,
                        testStoreItem2.Name,
                        testStoreItem2.Image,
                        testStoreItem2.Deleted,
                        testStoreItem2.UnitName,
                        testStoreItem2.BarmanCanStock,
                        testStoreItem2.IsContainerItem
                    ),
                ], new PageMeta(
                    1, Constants.DefaultPageSize, 1, 2, 2, 1)));
    }

    [Fact]
    public void ReadAll_ReadsCorrectly_WhenFilteringByCategoryAndStore() {
        // arrange
        var drinkCategory = new ProductCategoryEntity { Name = "Drink" };
        var foodCategory = new ProductCategoryEntity { Name = "Food" };
        var testCurrency = new CurrencyEntity { Name = "Czech Crowns" };
        var testStore = new StoreEntity { Name = "Kachna 1" };
        var testUser = new UserAccountEntity { UserName = "Some user" };
        var testStoreItem1 = new StoreItemEntity {
            Name = "Plzen Beer",
            UnitName = "bottle",
            Categories = { drinkCategory },
            Costs =
            {
                new CurrencyCostEntity
                {
                    Currency = testCurrency,
                    Amount = 30,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-10)
                },
                new CurrencyCostEntity
                {
                    Currency = testCurrency,
                    Amount = 25,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-5)
                }
            },
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    Store = testStore,
                    ItemAmount = -20,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        ResponsibleUser = testUser,
                        Timestamp = DateTimeOffset.UtcNow.AddDays(-5),
                        TransactionReason = TransactionReason.Sale
                    }
                },
                new StoreTransactionItemEntity
                {
                    Store = testStore,
                    ItemAmount = 42,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        ResponsibleUser = testUser,
                        Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                        TransactionReason = TransactionReason.AddingToStore
                    }
                }
            }
        };
        var testStoreItem2 = new StoreItemEntity {
            Name = "Bread",
            UnitName = "slice",
            Categories = { foodCategory },
            Costs =
            {
                new CurrencyCostEntity
                {
                    Currency = testCurrency,
                    Amount = 30,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-10)
                },
                new CurrencyCostEntity
                {
                    Currency = testCurrency,
                    Amount = 42,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-5)
                }
            },
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    Store = testStore,
                    ItemAmount = -42,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        ResponsibleUser = testUser,
                        Timestamp = DateTimeOffset.UtcNow.AddDays(-5),
                        TransactionReason = TransactionReason.Sale
                    }
                },
                new StoreTransactionItemEntity
                {
                    Store = testStore,
                    ItemAmount = 42,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        ResponsibleUser = testUser,
                        Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                        TransactionReason = TransactionReason.AddingToStore
                    }
                }
            }
        };

        _referenceDbContext.StoreItems.Add(testStoreItem1);
        _referenceDbContext.StoreItems.Add(testStoreItem2);
        _referenceDbContext.SaveChanges();

        // act
        var readResult = _storeItemService.ReadAll(null, null, null, foodCategory.Id, testStore.Id);

        // assert
        readResult.IsT0.Should().BeTrue();
        var resultAsT0 = readResult.AsT0;
        resultAsT0.Should().BeEquivalentTo(Page<StoreItemListModel>.Empty);
    }

    [Fact]
    public void ReadAll_ReadsCorrectly_WhenFilteringByStore() {
        // arrange
        var drinkCategory = new ProductCategoryEntity { Name = "Drink" };
        var foodCategory = new ProductCategoryEntity { Name = "Food" };
        var testCurrency = new CurrencyEntity { Name = "Czech Crowns" };
        var testStore = new StoreEntity { Name = "Kachna 1" };
        var testUser = new UserAccountEntity { UserName = "Some user" };
        var testStoreItem1 = new StoreItemEntity {
            Name = "Plzen Beer",
            UnitName = "bottle",
            Categories = { drinkCategory },
            Costs =
            {
                new CurrencyCostEntity
                {
                    Currency = testCurrency,
                    Amount = 30,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-10)
                },
                new CurrencyCostEntity
                {
                    Currency = testCurrency,
                    Amount = 25,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-5)
                }
            },
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    Store = testStore,
                    ItemAmount = -20,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        ResponsibleUser = testUser,
                        Timestamp = DateTimeOffset.UtcNow.AddDays(-5),
                        TransactionReason = TransactionReason.Sale
                    }
                },
                new StoreTransactionItemEntity
                {
                    Store = testStore,
                    ItemAmount = 42,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        ResponsibleUser = testUser,
                        Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                        TransactionReason = TransactionReason.AddingToStore
                    }
                }
            }
        };
        var testStoreItem2 = new StoreItemEntity {
            Name = "Bread",
            UnitName = "slice",
            Categories = { foodCategory },
            Costs =
            {
                new CurrencyCostEntity
                {
                    Currency = testCurrency,
                    Amount = 30,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-10)
                },
                new CurrencyCostEntity
                {
                    Currency = testCurrency,
                    Amount = 42,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-5)
                }
            },
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    Store = testStore,
                    ItemAmount = -42,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        ResponsibleUser = testUser,
                        Timestamp = DateTimeOffset.UtcNow.AddDays(-5),
                        TransactionReason = TransactionReason.Sale
                    }
                },
                new StoreTransactionItemEntity
                {
                    Store = testStore,
                    ItemAmount = 42,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        ResponsibleUser = testUser,
                        Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                        TransactionReason = TransactionReason.AddingToStore
                    }
                }
            }
        };

        _referenceDbContext.StoreItems.Add(testStoreItem1);
        _referenceDbContext.StoreItems.Add(testStoreItem2);
        _referenceDbContext.SaveChanges();

        // act
        var readResult = _storeItemService.ReadAll(null, null, null, null, testStore.Id);

        // assert
        readResult.IsT0.Should().BeTrue();
        var resultAsT0 = readResult.AsT0;
        resultAsT0.Should().BeEquivalentTo(
            new Page<StoreItemListModel>(
                [
                    new(
                        testStoreItem1.Id,
                        testStoreItem1.Name,
                        testStoreItem1.Image,
                        testStoreItem1.Deleted,
                        testStoreItem1.UnitName,
                        testStoreItem1.BarmanCanStock,
                        testStoreItem1.IsContainerItem
                    )
                ], new PageMeta(
                    1, Constants.DefaultPageSize, 1, 1, 1, 1)));
    }

    [Fact]
    public void ReadAll_ReturnsErrors_WhenFilteringByNonexistentIds() {
        // arrange
        const int storeId = 42;
        const int categoryId = 42;

        // act
        var readResult = _storeItemService.ReadAll(null, null, null, categoryId, storeId);

        // assert
        readResult.Should().HaveValue(new Dictionary<string, string[]>
        {
            {
                nameof(storeId),
                [$"Store with id {storeId} doesn't exist"]
            },
            {
                nameof(categoryId),
                [$"Category with id {categoryId} doesn't exist"]
            }
        });
    }

    [Fact]
    public void Create_Creates_WhenDataIsValid() {
        // arrange
        var categories = new List<ProductCategoryEntity>
        {
            new() { Name = "Drinks" },
            new() { Name = "Food" }
        };
        _referenceDbContext.ProductCategories.AddRange(categories);
        _referenceDbContext.SaveChanges();

        var createModel = new StoreItemCreateModel(
            "Beer",
            string.Empty,
            categories.Select(cat => cat.Id),
            "l",
            false,
            false
        );

        // act
        var createResult = _storeItemService.Create(createModel);

        // assert
        var createdEntity = _referenceDbContext.StoreItems
            .Include(si => si.Categories)
            .First(si => si.Id == createResult.AsT0.Id);
        var expectedEntity = new StoreItemEntity {
            Id = createResult.AsT0.Id,
            BarmanCanStock = createModel.BarmanCanStock,
            Deleted = false,
            Image = createModel.Image,
            IsContainerItem = createModel.IsContainerItem,
            Name = createModel.Name,
            UnitName = createModel.UnitName
        };
        ((List<ProductCategoryEntity>)expectedEntity.Categories).AddRange(categories);
        createdEntity.Should().BeEquivalentTo(expectedEntity);
        createResult.Should().HaveValue(new StoreItemIntermediateModel(expectedEntity, [], []).ToModel());
    }

    [Fact]
    public void Create_ReturnsErrors_WhenDataIsNotValid() {
        // arrange
        var createModel = new StoreItemCreateModel(
            "Beer",
            string.Empty,
            [42, 52],
            "l",
            false,
            false
        );

        // act
        var createResult = _storeItemService.Create(createModel);

        // assert
        createResult.Should().HaveValue(new Dictionary<string, string[]>
        {
            {
                nameof(createModel.CategoryIds),
                ["Some of the submitted categories do not exist"]
            }
        });
    }

    [Fact]
    public void Read_ReturnsNotFound_WhenNonexistentId() {
        // act
        var readResult = _storeItemService.Read(42);

        // assert
        readResult.Should().BeNotFound();
    }

    [Fact]
    public void Read_ReadsCorrectly_WhenComplex() {
        // arrange
        var drinkCategory = new ProductCategoryEntity { Name = "Drink" };
        var testCurrency = new CurrencyEntity { Name = "Czech Crowns" };
        var testStore = new StoreEntity { Name = "Kachna 1" };
        var testUser = new UserAccountEntity { UserName = "Some user" };
        var testStoreItem1 = new StoreItemEntity {
            Name = "Plzen Beer",
            UnitName = "bottle",
            Categories = { drinkCategory },
            Costs =
            {
                new CurrencyCostEntity
                {
                    Currency = testCurrency,
                    Amount = 30,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-10)
                },
                new CurrencyCostEntity
                {
                    Currency = testCurrency,
                    Amount = 25,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-5)
                }
            },
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    Store = testStore,
                    ItemAmount = -20,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        ResponsibleUser = testUser,
                        Timestamp = DateTimeOffset.UtcNow.AddDays(-5),
                        TransactionReason = TransactionReason.Sale
                    }
                },
                new StoreTransactionItemEntity
                {
                    Store = testStore,
                    ItemAmount = 42,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        ResponsibleUser = testUser,
                        Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                        TransactionReason = TransactionReason.AddingToStore
                    }
                }
            }
        };

        _referenceDbContext.StoreItems.Add(testStoreItem1);
        _referenceDbContext.SaveChanges();

        // act
        var readResult = _storeItemService.Read(testStoreItem1.Id);

        // assert
        var expectedModel = new StoreItemDetailModel(
            testStoreItem1.Id,
            testStoreItem1.Name,
            testStoreItem1.Image,
            testStoreItem1.Deleted,
            testStoreItem1.UnitName,
            testStoreItem1.BarmanCanStock,
            testStoreItem1.IsContainerItem,
            testStoreItem1.Categories.ToList().ToModels(),
            testStoreItem1.Costs.ToList().ToModels(),
            [
                testStoreItem1.Costs.ElementAt(1).ToModel()
            ],
            [
                new(
                    testStore.ToListModel(),
                    testStoreItem1.Id,
                    22
                )
            ]
        );
        readResult.Should().HaveValue(expectedModel);
    }

    [Fact]
    public void Delete_Deletes_WhenExistingId() {
        var testStoreItem = new StoreItemEntity {
            Name = "Some store item"
        };
        _referenceDbContext.StoreItems.Add(testStoreItem);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();

        // act
        var deleteResult = _storeItemService.Delete(testStoreItem.Id);

        // assert
        var updatedEntity = _referenceDbContext.StoreItems.Find(testStoreItem.Id);
        updatedEntity!.Deleted.Should().BeTrue();
        deleteResult.IsT0.Should().BeTrue();
        deleteResult.AsT0.Deleted.Should().BeTrue();
    }

    [Fact]
    public void Delete_ReturnsNotFound_WhenNonexistentId() {
        // act
        var deleteResult = _storeItemService.Delete(42);

        // assert
        deleteResult.Should().BeNotFound();
    }

    [Fact]
    public void Update_ReturnsNotFound_WhenNonexistentId() {
        // arrange
        var updateModel = new StoreItemCreateModel(
            "Beer",
            string.Empty,
            [42, 52],
            "l",
            false,
            false
        );

        // act
        var updateResult = _storeItemService.Update(42, updateModel);

        // assert
        updateResult.Should().BeNotFound();
    }

    [Fact]
    public void Update_ReturnsErrors_WhenDataIsNotValid() {
        // arrange
        var drinkCategory = new ProductCategoryEntity { Name = "Drink" };
        var foodCategory = new ProductCategoryEntity { Name = "food" };
        var testCurrency = new CurrencyEntity { Name = "Czech Crowns" };
        var testStore = new StoreEntity { Name = "Kachna 1" };
        var testUser = new UserAccountEntity { UserName = "Some user" };
        var testStoreItem1 = new StoreItemEntity {
            Name = "Plzen Beer",
            UnitName = "bottle",
            Deleted = true,
            Categories = { drinkCategory },
            Costs =
            {
                new CurrencyCostEntity
                {
                    Currency = testCurrency,
                    Amount = 30,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-10)
                },
                new CurrencyCostEntity
                {
                    Currency = testCurrency,
                    Amount = 25,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-5)
                }
            },
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    Store = testStore,
                    ItemAmount = -20,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        ResponsibleUser = testUser,
                        Timestamp = DateTimeOffset.UtcNow.AddDays(-5),
                        TransactionReason = TransactionReason.Sale
                    }
                },
                new StoreTransactionItemEntity
                {
                    Store = testStore,
                    ItemAmount = 42,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        ResponsibleUser = testUser,
                        Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                        TransactionReason = TransactionReason.AddingToStore
                    }
                }
            }
        };

        _referenceDbContext.StoreItems.Add(testStoreItem1);
        _referenceDbContext.ProductCategories.Add(foodCategory);
        _referenceDbContext.SaveChanges();

        var updateModel = new StoreItemCreateModel(
            "Beer",
            "Some image somewhere",
            [42],
            "l",
            true,
            true
        );

        // act
        var readResult = _storeItemService.Update(testStoreItem1.Id, updateModel);

        // assert
        readResult.Should().HaveValue(new Dictionary<string, string[]>
        {
            {
                nameof(updateModel.CategoryIds),
                ["Some of the submitted categories do not exist"]
            },
            {
                nameof(updateModel.IsContainerItem),
                [
                    "Cannot change whether store item is a container item - item already " +
                    "has transactions associated with it"
                ]
            }
        });
    }

    [Fact]
    public void Update_UpdatesCorrectly_WhenDataIsValid() {
        // arrange
        var drinkCategory = new ProductCategoryEntity { Name = "Drink" };
        var foodCategory = new ProductCategoryEntity { Name = "food" };
        var testCurrency = new CurrencyEntity { Name = "Czech Crowns" };
        var testStore = new StoreEntity { Name = "Kachna 1" };
        var testUser = new UserAccountEntity { UserName = "Some user" };
        var testStoreItem1 = new StoreItemEntity {
            Name = "Plzen Beer",
            UnitName = "bottle",
            Deleted = true,
            Categories = { drinkCategory },
            Costs =
            {
                new CurrencyCostEntity
                {
                    Currency = testCurrency,
                    Amount = 30,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-10)
                },
                new CurrencyCostEntity
                {
                    Currency = testCurrency,
                    Amount = 25,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-5)
                }
            },
            StoreTransactionItems =
            {
                new StoreTransactionItemEntity
                {
                    Store = testStore,
                    ItemAmount = -20,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        ResponsibleUser = testUser,
                        Timestamp = DateTimeOffset.UtcNow.AddDays(-5),
                        TransactionReason = TransactionReason.Sale
                    }
                },
                new StoreTransactionItemEntity
                {
                    Store = testStore,
                    ItemAmount = 42,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        ResponsibleUser = testUser,
                        Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                        TransactionReason = TransactionReason.AddingToStore
                    }
                }
            }
        };

        _referenceDbContext.StoreItems.Add(testStoreItem1);
        _referenceDbContext.ProductCategories.Add(foodCategory);
        _referenceDbContext.SaveChanges();

        var updateModel = new StoreItemCreateModel(
            "Beer",
            "Some image somewhere",
            [foodCategory.Id, drinkCategory.Id],
            "l",
            true,
            false
        );

        // act
        var readResult = _storeItemService.Update(testStoreItem1.Id, updateModel);

        // assert
        var expectedModel = new StoreItemDetailModel(
            testStoreItem1.Id,
            updateModel.Name,
            updateModel.Image,
            false,
            updateModel.UnitName,
            updateModel.BarmanCanStock,
            updateModel.IsContainerItem,
            new List<ProductCategoryEntity>
            {
                foodCategory, drinkCategory
            }.ToModels(),
            testStoreItem1.Costs.ToList().ToModels(),
            [
                testStoreItem1.Costs.ElementAt(1).ToModel()
            ],
            [
                new(
                    testStore.ToListModel(),
                    testStoreItem1.Id,
                    22
                )
            ]
        );
        readResult.Should().HaveValue(expectedModel);
    }
}
