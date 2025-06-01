using BL.EF.Tests.Fixtures;
using FluentAssertions;
using KisV4.BL.EF.Services;
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
        AssertionOptions.AssertEquivalencyUsing(options =>
            options.Using<DateTimeOffset>(ctx =>
                ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1))).WhenTypeIs<DateTimeOffset>()
        );
    }

    public void Dispose() {
        _referenceDbContext.Dispose();
        _normalDbContext.Dispose();
    }

    public async ValueTask DisposeAsync() {
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
                testStore.Id
                ),
            out var _,
             out var _);

        // assert
        validationResult.Should().BeEquivalentTo(new Dictionary<string, string[]> {
                {
                "ItemAmount",
                [$"Sale item {testSaleItem.Id} specified invalid amount -42. Amount must be more than 0"]
                }});
    }
}
