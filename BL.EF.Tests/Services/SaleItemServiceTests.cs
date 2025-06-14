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

public class SaleItemServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable {
    private readonly KisDbContext _referenceDbContext;
    private readonly KisDbContext _normalDbContext;
    private readonly SaleItemService _saleItemService;

    public SaleItemServiceTests(KisDbContextFactory dbContextFactory) {
        (_referenceDbContext, _normalDbContext) = dbContextFactory.CreateDbContextAndReference();
        _saleItemService = new SaleItemService(_normalDbContext);
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
        var drinkCategory = new ProductCategoryEntity { Name = "Drink" };
        var foodCategory = new ProductCategoryEntity { Name = "Food" };
        var testUser = new UserAccountEntity { UserName = "Some user" };
        var testStore1 = new StoreEntity { Name = "Kachna 1" };
        var testStore2 = new StoreEntity { Name = "Kachna 2" };
        var testSaleItem1 = new SaleItemEntity {
            Name = "Toast",
            Categories = { foodCategory },
            Composition =
            {
                new CompositionEntity
                {
                    Amount = 10,
                    StoreItem = new StoreItemEntity
                    {
                        Name = "Šunka",
                        StoreTransactionItems =
                        {
                            new StoreTransactionItemEntity
                            {
                                Store = testStore1,
                                ItemAmount = 42,
                                StoreTransaction = new StoreTransactionEntity
                                {
                                    ResponsibleUser = testUser,
                                    Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                                    TransactionReason = TransactionReason.AddingToStore
                                }
                            }
                        }
                    }
                },
                new CompositionEntity
                {
                    Amount = 1,
                    StoreItem = new StoreItemEntity
                    {
                        Name = "Chleba",
                        StoreTransactionItems =
                        {
                            new StoreTransactionItemEntity
                            {
                                Store = testStore1,
                                ItemAmount = 42,
                                StoreTransaction = new StoreTransactionEntity
                                {
                                    ResponsibleUser = testUser,
                                    Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                                    TransactionReason = TransactionReason.AddingToStore
                                }
                            }
                        }
                    }
                }
            },
            ShowOnWeb = true
        };
        var testSaleItem2 = new SaleItemEntity {
            Name = "Plzen flaska 0.5",
            Categories = { drinkCategory },
            Composition =
            {
                new CompositionEntity
                {
                    Amount = 1,
                    StoreItem = new StoreItemEntity
                    {
                        Name = "Plzen",
                        StoreTransactionItems =
                        {
                            new StoreTransactionItemEntity
                            {
                                Store = testStore2,
                                ItemAmount = 42,
                                StoreTransaction = new StoreTransactionEntity
                                {
                                    ResponsibleUser = testUser,
                                    Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                                    TransactionReason = TransactionReason.AddingToStore
                                }
                            }
                        }
                    }
                },
            },
            ShowOnWeb = false
        };

        _referenceDbContext.SaleItems.Add(testSaleItem1);
        _referenceDbContext.SaleItems.Add(testSaleItem2);
        _referenceDbContext.SaveChanges();

        // act
        var readResult = _saleItemService.ReadAll(null, null, null, null, null);

        // assert
        readResult.IsT0.Should().BeTrue();
        var resultAsT0 = readResult.AsT0;
        resultAsT0.Should().BeEquivalentTo(
            new Page<SaleItemListModel>(
                [    new(
                        testSaleItem1.Id,
                        testSaleItem1.Name,
                        testSaleItem1.Image,
                        testSaleItem1.Deleted,
                        testSaleItem1.ShowOnWeb
                    ),
                    new(
                        testSaleItem2.Id,
                        testSaleItem2.Name,
                        testSaleItem2.Image,
                        testSaleItem2.Deleted,
                        testSaleItem2.ShowOnWeb
                    ),
                ], new PageMeta(
                    1, Constants.DefaultPageSize, 1, 2, 2, 1)));
    }

    [Fact]
    public void ReadAll_ReadsAll_WhenFilteringByCategory() {
        // arrange
        var drinkCategory = new ProductCategoryEntity { Name = "Drink" };
        var foodCategory = new ProductCategoryEntity { Name = "Food" };
        var testUser = new UserAccountEntity { UserName = "Some user" };
        var testStore1 = new StoreEntity { Name = "Kachna 1" };
        var testStore2 = new StoreEntity { Name = "Kachna 2" };
        var testSaleItem1 = new SaleItemEntity {
            Name = "Toast",
            Categories = { foodCategory },
            Composition =
            {
                new CompositionEntity
                {
                    Amount = 10,
                    StoreItem = new StoreItemEntity
                    {
                        Name = "Šunka",
                        StoreTransactionItems =
                        {
                            new StoreTransactionItemEntity
                            {
                                Store = testStore1,
                                ItemAmount = 42,
                                StoreTransaction = new StoreTransactionEntity
                                {
                                    ResponsibleUser = testUser,
                                    Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                                    TransactionReason = TransactionReason.AddingToStore
                                }
                            }
                        }
                    }
                },
                new CompositionEntity
                {
                    Amount = 1,
                    StoreItem = new StoreItemEntity
                    {
                        Name = "Chleba",
                        StoreTransactionItems =
                        {
                            new StoreTransactionItemEntity
                            {
                                Store = testStore1,
                                ItemAmount = 42,
                                StoreTransaction = new StoreTransactionEntity
                                {
                                    ResponsibleUser = testUser,
                                    Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                                    TransactionReason = TransactionReason.AddingToStore
                                }
                            }
                        }
                    }
                }
            },
            ShowOnWeb = true
        };
        var testSaleItem2 = new SaleItemEntity {
            Name = "Plzen flaska 0.5",
            Categories = { drinkCategory },
            Composition =
            {
                new CompositionEntity
                {
                    Amount = 1,
                    StoreItem = new StoreItemEntity
                    {
                        Name = "Plzen",
                        StoreTransactionItems =
                        {
                            new StoreTransactionItemEntity
                            {
                                Store = testStore2,
                                ItemAmount = 42,
                                StoreTransaction = new StoreTransactionEntity
                                {
                                    ResponsibleUser = testUser,
                                    Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                                    TransactionReason = TransactionReason.AddingToStore
                                }
                            }
                        }
                    }
                },
            },
            ShowOnWeb = false
        };

        _referenceDbContext.SaleItems.Add(testSaleItem1);
        _referenceDbContext.SaleItems.Add(testSaleItem2);
        _referenceDbContext.SaveChanges();

        // act
        var readResult = _saleItemService.ReadAll(null, null, null, foodCategory.Id, null);

        // assert
        readResult.IsT0.Should().BeTrue();
        var resultAsT0 = readResult.AsT0;
        resultAsT0.Should().BeEquivalentTo(
            new Page<SaleItemListModel>(
                [
                    new(
                        testSaleItem1.Id,
                        testSaleItem1.Name,
                        testSaleItem1.Image,
                        testSaleItem1.Deleted,
                        testSaleItem1.ShowOnWeb
                    ),
                ], new PageMeta(
                    1, Constants.DefaultPageSize, 1, 1, 1, 1)));
    }

    [Fact]
    public void Read_ReadsCorrectly_WithModifiers() {
        // arrange
        var foodCategory = new ProductCategoryEntity { Name = "Food" };
        var testUser = new UserAccountEntity { UserName = "Some user" };
        var testStore1 = new StoreEntity { Name = "Kachna 1" };
        var testStore2 = new StoreEntity { Name = "Kachna 2" };
        var testCurrency = new CurrencyEntity { Name = "Czech crowns" };
        var testSaleItem1 = new SaleItemEntity {
            Name = "Toast",
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
                    Amount = 50,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-5)
                }
            },
            Composition =
            {
                new CompositionEntity
                {
                    Amount = 10,
                    StoreItem = new StoreItemEntity
                    {
                        Name = "Šunka",
                        StoreTransactionItems =
                        {
                            new StoreTransactionItemEntity
                            {
                                Store = testStore1,
                                ItemAmount = 42,
                                StoreTransaction = new StoreTransactionEntity
                                {
                                    ResponsibleUser = testUser,
                                    Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                                    TransactionReason = TransactionReason.AddingToStore
                                }
                            }
                        }
                    }
                },
                new CompositionEntity
                {
                    Amount = 1,
                    StoreItem = new StoreItemEntity
                    {
                        Name = "Chleba",
                        StoreTransactionItems =
                        {
                            new StoreTransactionItemEntity
                            {
                                Store = testStore1,
                                ItemAmount = 42,
                                StoreTransaction = new StoreTransactionEntity
                                {
                                    ResponsibleUser = testUser,
                                    Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                                    TransactionReason = TransactionReason.AddingToStore
                                }
                            }
                        }
                    }
                }
            },
            ShowOnWeb = true,
            AvailableModifiers =
            {
                new ModifierEntity { Name = "Some modifier" },
                new ModifierEntity { Name = "Some other modifier", Deleted = true }
            }
        };

        _referenceDbContext.SaleItems.Add(testSaleItem1);
        _referenceDbContext.Stores.Add(testStore2);
        _referenceDbContext.SaveChanges();

        // act
        var readResult = _saleItemService.Read(testSaleItem1.Id);

        // assert
        var expectedResult = new SaleItemDetailModel(
            testSaleItem1.Id,
            testSaleItem1.Name,
            testSaleItem1.Image,
            testSaleItem1.Deleted,
            testSaleItem1.ShowOnWeb,
            testSaleItem1.Categories.ToList().ToModels(),
            testSaleItem1.Composition.ToList().ToModels(),
            testSaleItem1.AvailableModifiers.Where(mod => !mod.Deleted).ToList().ToModels(),
            testSaleItem1.Costs.ToList().ToModels(),
            [
                testSaleItem1.Costs.ElementAt(1).ToModel()
            ],
            [
                new(
                    testStore1.ToListModel(),
                    testSaleItem1.Id,
                    4
                ),
                new(
                    testStore2.ToListModel(),
                    testSaleItem1.Id,
                    0
                )
            ]
        );
        readResult.Should().HaveValue(expectedResult);
    }

    [Fact]
    public void Read_ReadsCorrectly_WhenNoTransactionsInStore() {
        // arrange
        var foodCategory = new ProductCategoryEntity { Name = "Food" };
        var testUser = new UserAccountEntity { UserName = "Some user" };
        var testStore1 = new StoreEntity { Name = "Kachna 1" };
        var testStore2 = new StoreEntity { Name = "Kachna 2" };
        var testCurrency = new CurrencyEntity { Name = "Czech crowns" };
        var testSaleItem1 = new SaleItemEntity {
            Name = "Toast",
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
                    Amount = 50,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-5)
                }
            },
            Composition =
            {
                new CompositionEntity
                {
                    Amount = 10,
                    StoreItem = new StoreItemEntity
                    {
                        Name = "Šunka",
                        StoreTransactionItems =
                        {
                            new StoreTransactionItemEntity
                            {
                                Store = testStore1,
                                ItemAmount = 42,
                                StoreTransaction = new StoreTransactionEntity
                                {
                                    ResponsibleUser = testUser,
                                    Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                                    TransactionReason = TransactionReason.AddingToStore
                                }
                            }
                        }
                    }
                },
                new CompositionEntity
                {
                    Amount = 1,
                    StoreItem = new StoreItemEntity
                    {
                        Name = "Chleba",
                        StoreTransactionItems =
                        {
                            new StoreTransactionItemEntity
                            {
                                Store = testStore1,
                                ItemAmount = 42,
                                StoreTransaction = new StoreTransactionEntity
                                {
                                    ResponsibleUser = testUser,
                                    Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                                    TransactionReason = TransactionReason.AddingToStore
                                }
                            }
                        }
                    }
                }
            },
            ShowOnWeb = true
        };

        _referenceDbContext.SaleItems.Add(testSaleItem1);
        _referenceDbContext.Stores.Add(testStore2);
        _referenceDbContext.SaveChanges();

        // act
        var readResult = _saleItemService.Read(testSaleItem1.Id);

        // assert
        var expectedResult = new SaleItemDetailModel(
            testSaleItem1.Id,
            testSaleItem1.Name,
            testSaleItem1.Image,
            testSaleItem1.Deleted,
            testSaleItem1.ShowOnWeb,
            testSaleItem1.Categories.ToList().ToModels(),
            testSaleItem1.Composition.ToList().ToModels(),
            [],
            testSaleItem1.Costs.ToList().ToModels(),
            [
                testSaleItem1.Costs.ElementAt(1).ToModel()
            ],
            [
                new(
                    testStore1.ToListModel(),
                    testSaleItem1.Id,
                    4
                ),
                new(
                    testStore2.ToListModel(),
                    testSaleItem1.Id,
                    0
                )
            ]
        );
        readResult.Should().HaveValue(expectedResult);
    }

    [Fact]
    public void Read_ReadsCorrectly_WhenNoTransactionsForStoreItem() {
        // arrange
        var foodCategory = new ProductCategoryEntity { Name = "Food" };
        var testUser = new UserAccountEntity { UserName = "Some user" };
        var testStore1 = new StoreEntity { Name = "Kachna 1" };
        var testStore2 = new StoreEntity { Name = "Kachna 2" };
        var testCurrency = new CurrencyEntity { Name = "Czech crowns" };
        var testSaleItem1 = new SaleItemEntity {
            Name = "Toast",
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
                    Amount = 50,
                    ValidSince = DateTimeOffset.UtcNow.AddDays(-5)
                }
            },
            Composition =
            {
                new CompositionEntity
                {
                    Amount = 10,
                    StoreItem = new StoreItemEntity
                    {
                        Name = "Šunka"
                    }
                },
                new CompositionEntity
                {
                    Amount = 1,
                    StoreItem = new StoreItemEntity
                    {
                        Name = "Chleba",
                        StoreTransactionItems =
                        {
                            new StoreTransactionItemEntity
                            {
                                Store = testStore1,
                                ItemAmount = 42,
                                StoreTransaction = new StoreTransactionEntity
                                {
                                    ResponsibleUser = testUser,
                                    Timestamp = DateTimeOffset.UtcNow.AddDays(-10),
                                    TransactionReason = TransactionReason.AddingToStore
                                }
                            }
                        }
                    }
                }
            },
            ShowOnWeb = true
        };

        _referenceDbContext.SaleItems.Add(testSaleItem1);
        _referenceDbContext.Stores.Add(testStore2);
        _referenceDbContext.SaveChanges();

        // act
        var readResult = _saleItemService.Read(testSaleItem1.Id);

        // assert
        var expectedResult = new SaleItemDetailModel(
            testSaleItem1.Id,
            testSaleItem1.Name,
            testSaleItem1.Image,
            testSaleItem1.Deleted,
            testSaleItem1.ShowOnWeb,
            testSaleItem1.Categories.ToList().ToModels(),
            testSaleItem1.Composition.ToList().ToModels(),
            [],
            testSaleItem1.Costs.ToList().ToModels(),
            [
                testSaleItem1.Costs.ElementAt(1).ToModel()
            ],
            [
                new(
                    testStore1.ToListModel(),
                    testSaleItem1.Id,
                    0
                ),
                new(
                    testStore2.ToListModel(),
                    testSaleItem1.Id,
                    0
                )
            ]
        );
        readResult.Should().HaveValue(expectedResult);
    }

    [Fact]
    public void Create_Creates_WhenDataIsValid() {
        // arrange
        var testCategory = new ProductCategoryEntity { Name = "Food" };
        _referenceDbContext.ProductCategories.Add(testCategory);
        _referenceDbContext.SaveChanges();
        var createModel = new SaleItemCreateModel(
            "Some sale item",
            string.Empty,
            [testCategory.Id],
            true
        );

        // act
        var createResult = _saleItemService.Create(createModel);

        // assert
        createResult.IsT0.Should().BeTrue();
        var createdEntity = _referenceDbContext.SaleItems
            .Include(si => si.Categories)
            .First(si => si.Id == createResult.AsT0.Id);
        createdEntity.Should().BeEquivalentTo(new SaleItemEntity {
            Id = createResult.AsT0.Id,
            Categories = { testCategory },
            Name = createModel.Name,
            ShowOnWeb = true
        });
        createResult.Should().HaveValue(new SaleItemDetailModel(
            createResult.AsT0.Id,
            createModel.Name,
            createModel.Image,
            false,
            createModel.ShowOnWeb,
            [testCategory.ToModel()],
            [],
            [],
            []
        ));
    }

    [Fact]
    public void Create_ReturnsErrors_WhenDataIsNotValid() {
        // arrange
        var createModel = new SaleItemCreateModel(
            "Some sale item",
            string.Empty,
            [42],
            true
        );

        // act
        var createResult = _saleItemService.Create(createModel);

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
    public void Update_Updates_WhenDataIsValid() {
        // arrange
        var testSaleItem = new SaleItemEntity {
            Name = "Beer",
            Image = "Some image",
            Categories = { new ProductCategoryEntity { Name = "Drink" } },
            ShowOnWeb = false,
            Costs =
            {
                new CurrencyCostEntity
                {
                    Amount = 42,
                    Currency = new CurrencyEntity { ShortName = "Czk" },
                    ValidSince = DateTimeOffset.UtcNow
                }
            }
        };
        var testCategory = new ProductCategoryEntity { Name = "Food" };
        _referenceDbContext.ProductCategories.Add(testCategory);
        _referenceDbContext.SaleItems.Add(testSaleItem);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();
        var updateModel = new SaleItemCreateModel(
            "Some sale item",
            string.Empty,
            [testCategory.Id],
            true
        );

        // act
        var createResult = _saleItemService.Update(testSaleItem.Id, updateModel);

        // assert
        createResult.IsT0.Should().BeTrue();
        var createdEntity = _referenceDbContext.SaleItems
            .Include(si => si.Categories)
            .Include(si => si.Costs)
            .ThenInclude(c => c.Currency)
            .First(si => si.Id == createResult.AsT0.Id);
        var expectedEntity = new SaleItemEntity {
            Id = testSaleItem.Id,
            Categories = { testCategory },
            Costs = { testSaleItem.Costs.ElementAt(0) },
            Name = updateModel.Name,
            ShowOnWeb = true
        };
        testCategory.Products.Add(expectedEntity);
        expectedEntity.Costs.ElementAt(0).Product = expectedEntity;
        createdEntity.Should().BeEquivalentTo(expectedEntity,
            opts => opts.IgnoringCyclicReferences());
        createResult.AsT0.Should().BeEquivalentTo(new SaleItemDetailModel(
            createResult.AsT0.Id,
            updateModel.Name,
            updateModel.Image,
            false,
            updateModel.ShowOnWeb,
            [testCategory.ToModel()],
            [],
            [],
            [testSaleItem.Costs.ElementAt(0).ToModel()],
            [testSaleItem.Costs.ElementAt(0).ToModel()]
        ), opts => opts.IgnoringCyclicReferences());
    }
}
