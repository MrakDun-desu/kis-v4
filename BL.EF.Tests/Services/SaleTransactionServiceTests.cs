using BL.EF.Tests.Fixtures;
using FluentAssertions;
using KisV4.BL.EF.Services;
using KisV4.Common.Enums;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.Extensions.Time.Testing;

namespace BL.EF.Tests.Services;

public class SaleTransactionServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable {
    private readonly SaleTransactionService _saleTransactionService;
    private readonly UserService _userService;
    private readonly FakeTimeProvider _timeProvider = new();
    private readonly KisDbContext _referenceDbContext;
    private readonly KisDbContext _normalDbContext;

    public SaleTransactionServiceTests(KisDbContextFactory dbContextFactory) {
        (_referenceDbContext, _normalDbContext) = dbContextFactory.CreateDbContextAndReference();
        _userService = new UserService(
                _normalDbContext,
                new CurrencyChangeService(_normalDbContext),
                new DiscountUsageService(_normalDbContext)
            );
        _saleTransactionService = new SaleTransactionService(_normalDbContext, _userService, _timeProvider);
        AssertionOptions.AssertEquivalencyUsing(static options =>
            options.Using<DateTimeOffset>(static ctx =>
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
    public void Validate_ChecksIf_ItemsHaveCorrectAmount() {
        // arrange
        var testSaleItem = new SaleItemEntity {
            Name = "Some sale item",
            Composition = [
                new () {
                    Amount = 1,
                    StoreItem = new () { Name = "Some store item" },
                }
            ],
        };
        var testStore = new StoreEntity { Name = "Some store" };
        _referenceDbContext.SaleItems.Add(testSaleItem);
        _referenceDbContext.Stores.Add(testStore);
        _referenceDbContext.SaveChanges();

        // act
        var validationResult = _saleTransactionService
            .ValidateCreateModel(new(
                [new(testSaleItem.Id, [], -42, null)],
                testStore.Id,
                "Some user"
                ),
             out var _);

        // assert
        validationResult.Should().BeEquivalentTo(new Dictionary<string, string[]> {
                {
                "ItemAmount",
                [$"Sale item {testSaleItem.Id} specified invalid amount -42. Amount must be more than 0"]
                }});
    }

    [Fact]
    public void Validate_ChecksIf_DefaultStoreIsntContainer() {
        // arrange
        var testSaleItem = new SaleItemEntity {
            Name = "Some sale item",
            Composition = [
                new () {
                    Amount = 1,
                    StoreItem = new () {
                        Name = "Some store item",
                        // IsContainerItem = true
                    },
                }
            ],
        };
        var testContainer = new ContainerEntity {
            Name = "Some container",
            Template = new ContainerTemplateEntity {
                ContainedItem = testSaleItem.Composition.First().StoreItem
            }
        };
        _referenceDbContext.SaleItems.Add(testSaleItem);
        _referenceDbContext.Containers.Add(testContainer);
        _referenceDbContext.SaveChanges();

        // act
        var validationResult = _saleTransactionService
            .ValidateCreateModel(new(
                [new(testSaleItem.Id, [], 42, null)],
                testContainer.Id,
                "Some user"
                ),
             out var _);

        // assert
        validationResult.Should().BeEquivalentTo(new Dictionary<string, string[]> {
                {
                "StoreId",
                [$"Default sale transaction store {testContainer.Id} cannot be a container"]
                }});
    }

    [Fact]
    public void Validate_ChecksIf_ContainerStoresContainCorrectItems() {
        // arrange
        var testStoreItem = new StoreItemEntity {
            Name = "Some store item",
            IsContainerItem = false,
        };
        var testSaleItem = new SaleItemEntity {
            Name = "Some sale item",
            Composition = [
                new () {
                    Amount = 1,
                    StoreItem = testStoreItem
                }
            ],
        };
        var testStore = new StoreEntity {
            Name = "Some store"
        };
        var testContainer = new ContainerEntity {
            Name = "Some container",
            Template = new ContainerTemplateEntity {
                ContainedItem = testStoreItem
            }
        };
        _referenceDbContext.SaleItems.Add(testSaleItem);
        _referenceDbContext.Stores.Add(testStore);
        _referenceDbContext.Containers.Add(testContainer);
        _referenceDbContext.SaveChanges();

        // act
        var validationResult = _saleTransactionService
            .ValidateCreateModel(new(
                [new(testSaleItem.Id, [], 42, testContainer.Id)],
                testStore.Id,
                "Some user"
                ),
             out var _);

        // assert
        validationResult.Should().BeEquivalentTo(new Dictionary<string, string[]> {
                {
                    "StoreId",
                    [$"Container {testContainer.Id} can't store a non-container item {testStoreItem.Id}"]
                }
            });
    }

    [Fact]
    public void Create_ReturnsCorrectOutput() {
        // arrange
        _timeProvider.SetUtcNow(DateTimeOffset.UtcNow);
        var testStore1 = new StoreEntity { Name = "Test store 1" };

        var testCurrency1 = new CurrencyEntity {
            ShortName = "czk"
        };
        var hamStoreItem = new StoreItemEntity { Name = "Ham", UnitName = "g" };
        var testSaleItem1 = new SaleItemEntity {
            Name = "Toast",
            Composition = [
                new CompositionEntity {
                    StoreItem = new StoreItemEntity {Name = "Toast bread", UnitName = "ks"},
                    Amount = 2,
                },
                new CompositionEntity {
                    StoreItem = new StoreItemEntity {Name = "Butter", UnitName = "g"},
                    Amount = 10
                },
                new CompositionEntity {
                    StoreItem = new StoreItemEntity {Name = "Cheese", UnitName = "g"},
                    Amount = 10
                },
                new CompositionEntity {
                    StoreItem = hamStoreItem,
                    Amount = 10
                },
            ],
            Costs = [
                new CurrencyCostEntity {
                    Currency = testCurrency1,
                    Amount = 20,
                    ValidSince = _timeProvider.GetUtcNow().AddDays(-2)
                }
            ]
        };
        var testModifier1 = new ModifierEntity {
            Name = "Vegetarian",
            ModificationTarget = testSaleItem1,
            Costs = [
                new CurrencyCostEntity {
                    Currency = testCurrency1,
                    Amount = -2,
                    ValidSince = _timeProvider.GetUtcNow().AddDays(-2)
                }
            ],
            Composition = [
                new CompositionEntity {
                    StoreItem = hamStoreItem,
                    Amount = -10
                }
            ]
        };

        _referenceDbContext.Currencies.Add(testCurrency1);
        _referenceDbContext.StoreItems.Add(hamStoreItem);
        _referenceDbContext.Stores.Add(testStore1);
        _referenceDbContext.SaleItems.Add(testSaleItem1);
        _referenceDbContext.Modifiers.Add(testModifier1);
        _referenceDbContext.SaveChanges();

        // act
        var createResult = _saleTransactionService.Create(
                new SaleTransactionCreateModel(
                    [
                        new SaleTransactionItemCreateModel(
                            testSaleItem1.Id,
                            [new ModifierAmountCreateModel(testModifier1.Id, 1)],
                            10,
                            null
                        )
                    ],
                    testStore1.Id,
                    "Some client"
                    ),
                "Some user"
                );

        // assert

        createResult.IsT0.Should().BeTrue();
        createResult.AsT0.Should().BeEquivalentTo(new SaleTransactionDetailModel(
                    1,
                    new UserListModel(0, "Some user", false),
                    _timeProvider.GetUtcNow(),
                    false,
                    [
                        new SaleTransactionItemListModel(
                            1,
                            new SaleItemListModel(
                                testSaleItem1.Id,
                                testSaleItem1.Name,
                                string.Empty,
                                false,
                                false
                                ),
                            [
                                new ModifierAmountListModel(
                                    new ModifierListModel(
                                        testModifier1.Id,
                                        testModifier1.Name,
                                        string.Empty,
                                        false,
                                        testSaleItem1.Id,
                                        false
                                        ),
                                    1,
                                    1
                                    )
                            ],
                            [
                                new TransactionPriceListModel(
                                    new CurrencyListModel(1, string.Empty, "czk"),
                                    1,
                                    180,
                                    false
                                )
                            ],
                            10,
                            false
                            )
                    ],
                    [
                        new StoreTransactionListModel(
                                1,
                                new UserListModel(1, "Some user", false),
                                _timeProvider.GetUtcNow(),
                                false,
                                TransactionReason.Sale,
                                1
                            )
                    ],
                    []
                    ), static opts => {
                        opts = opts.Excluding(static sti => sti.ResponsibleUser.Id);
                        return opts;
                    }
                );
    }

}
