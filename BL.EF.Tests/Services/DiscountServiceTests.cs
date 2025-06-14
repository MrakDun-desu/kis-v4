using BL.EF.Tests.Extensions;
using BL.EF.Tests.Fixtures;
using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace BL.EF.Tests.Services;

public class
    DiscountServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable {
    private readonly DiscountService _discountService;
    private readonly DiscountUsageService _discountUsageService;
    private readonly KisDbContext _referenceDbContext;
    private readonly KisDbContext _normalDbContext;

    public DiscountServiceTests(KisDbContextFactory dbContextFactory) {
        (_referenceDbContext, _normalDbContext) = dbContextFactory.CreateDbContextAndReference();
        _discountUsageService = new DiscountUsageService(_normalDbContext);
        _discountService = new DiscountService(_normalDbContext, _discountUsageService);
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
        var testDiscount1 = new DiscountEntity { Name = "Some discount box" };
        var testDiscount2 = new DiscountEntity { Name = "Some discount box 2", Deleted = true };
        _referenceDbContext.Discounts.Add(testDiscount1);
        _referenceDbContext.Discounts.Add(testDiscount2);
        _referenceDbContext.SaveChanges();

        // act
        var readModels = _discountService.ReadAll(null);

        // assert
        var mappedModels =
            _referenceDbContext.Discounts.ToList().ToModels();

        readModels.Should().BeEquivalentTo(mappedModels);
    }

    [Fact]
    public void ReadAll_ReadsCorrectly_WhenFilteringByDeleted() {
        // arrange
        var testDiscount1 = new DiscountEntity { Name = "Some discount box" };
        var testDiscount2 = new DiscountEntity { Name = "Some discount box 2", Deleted = true };
        _referenceDbContext.Discounts.Add(testDiscount1);
        _referenceDbContext.Discounts.Add(testDiscount2);
        _referenceDbContext.SaveChanges();

        // act
        var readModels = _discountService.ReadAll(false);

        // assert
        var mappedModels =
            _referenceDbContext.Discounts.Where(dc => !dc.Deleted).ToList().ToModels();

        readModels.Should().BeEquivalentTo(mappedModels);
    }

    [Fact]
    public void Read_ReturnsNotFound_WhenNotFound() {
        // act
        var readResult = _discountService.Read(42);

        // assert
        readResult.Should().BeNotFound();
    }

    [Fact]
    public void Read_ReadsCorrectly_WhenComplex() {
        // arrange
        var testDiscount1 = new DiscountEntity {
            Name = "Some discount box",
            DiscountUsages =
            {
                new DiscountUsageEntity
                {
                    Timestamp = DateTimeOffset.UtcNow,
                    User = new UserAccountEntity { UserName = "Some user" }
                }
            }
        };
        _referenceDbContext.Discounts.Add(testDiscount1);
        _referenceDbContext.SaveChanges();

        // act
        var readResult = _discountService.Read(testDiscount1.Id);

        // assert
        readResult.Should()
            .HaveValue(
                new DiscountIntermediateModel(
                        testDiscount1,
                        _discountUsageService.ReadAll(null, null, testDiscount1.Id, null).AsT0)
                    .ToModel()
            );
    }

    [Fact]
    public void Patch_RestoresDiscount_FromDeletion() {
        // arrange
        var testDiscount1 = new DiscountEntity {
            Name = "Some discount",
            Deleted = true
        };
        _referenceDbContext.Discounts.Add(testDiscount1);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();

        // act
        var patchResult = _discountService.Patch(testDiscount1.Id);

        // assert
        patchResult.Should()
            .HaveValue(
                new DiscountIntermediateModel(
                        testDiscount1 with { Deleted = false },
                        _discountUsageService.ReadAll(null, null, testDiscount1.Id, null).AsT0)
                    .ToModel()
            );
        _referenceDbContext.Discounts.Find(testDiscount1.Id)!.Deleted.Should().BeFalse();
    }

    [Fact]
    public void Delete_DeletesDiscount_IfFound() {
        // arrange
        var testDiscount1 = new DiscountEntity {
            Name = "Some discount",
        };
        _referenceDbContext.Discounts.Add(testDiscount1);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();

        // act
        var deleteResult = _discountService.Delete(testDiscount1.Id);

        // assert
        deleteResult.Should()
            .HaveValue(
                new DiscountIntermediateModel(
                        testDiscount1 with { Deleted = true },
                        _discountUsageService.ReadAll(null, null, testDiscount1.Id, null).AsT0)
                    .ToModel()
            );
        _referenceDbContext.Discounts.Find(testDiscount1.Id)!.Deleted.Should().BeTrue();
    }
}
