using BL.EF.Tests.Extensions;
using BL.EF.Tests.Fixtures;
using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace BL.EF.Tests.Services;

[Collection(DockerDatabaseTests.Name)]
public class CurrencyServiceTests : IDisposable, IAsyncDisposable {
    private readonly CurrencyService _currencyService;
    private readonly KisDbContext _referenceDbContext;
    private readonly KisDbContext _normalDbContext;

    public CurrencyServiceTests(KisDbContextFactory dbContextFactory) {
        (_referenceDbContext, _normalDbContext) = dbContextFactory.CreateDbContextAndReference();
        _currencyService = new CurrencyService(_normalDbContext);
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
    public void Create_CreatesCurrency_WhenDataIsValid() {
        // arrange
        var createModel = new CurrencyCreateModel("Czech Crowns", "CZK");

        // act
        var createdModel = _currencyService.Create(createModel);

        // assert
        var createdEntity = _referenceDbContext.Currencies.Find(createdModel.Id);
        var expectedEntity = new CurrencyEntity {
            Name = createModel.Name,
            ShortName = createModel.ShortName
        };
        createdEntity.Should().BeEquivalentTo(expectedEntity, opts =>
            opts.Excluding(c => c.Id)
        );
    }

    [Fact]
    public void ReadAll_ReadsAll() {
        // arrange
        var testCurrency1 = new CurrencyEntity { Name = "Some currency" };
        var testCurrency2 = new CurrencyEntity { Name = "Some currency 2" };
        _referenceDbContext.Currencies.Add(testCurrency1);
        _referenceDbContext.Currencies.Add(testCurrency2);
        _referenceDbContext.SaveChanges();

        // act
        var readModels = _currencyService.ReadAll();

        // assert
        var expectedModels = _referenceDbContext.Currencies.ToList().ToModels();

        readModels.Should().BeEquivalentTo(expectedModels);
    }

    [Fact]
    public void Update_UpdatesName_WhenExistingId() {
        // arrange
        var testCurrency = new CurrencyEntity { Name = "Some currency" };
        _referenceDbContext.Currencies.Add(testCurrency);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();
        var updateModel = new CurrencyCreateModel(
            "Some new name",
            "SNN"
        );

        // act
        var updateResult = _currencyService.Update(testCurrency.Id, updateModel);

        // assert
        var updatedEntity = _referenceDbContext.Currencies.Find(testCurrency.Id);
        var expectedEntity = testCurrency with {
            Name = updateModel.Name,
            ShortName = updateModel.ShortName
        };
        updateResult.Should().HaveValue(expectedEntity.ToModel());
        updatedEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void Update_ReturnsNotFound_WhenNotFound() {
        var updateModel = new CurrencyCreateModel("Some currency", "STH");

        var updateResult = _currencyService.Update(42, updateModel);

        updateResult.Should().BeNotFound();
    }
}
