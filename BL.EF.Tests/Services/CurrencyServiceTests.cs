using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace BL.EF.Tests.Services;

public class CurrencyServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable {
    private readonly CurrencyService _currencyService;
    private readonly KisDbContext _dbContext;
    private readonly Mapper _mapper;

    public CurrencyServiceTests(KisDbContextFactory dbContextFactory) {
        _dbContext = dbContextFactory.CreateDbContext();
        _mapper = new Mapper();
        _currencyService = new CurrencyService(_dbContext, _mapper);
    }

    [Fact]
    public void Create_CreatesCurrency_WhenDataIsValid() {
        var createModel = new CurrencyCreateModel("Some currency");
        var createdId = _currencyService.Create(createModel);

        var createdEntity = _dbContext.Currencies.Find(createdId);
        var expectedEntity = new CurrencyEntity { Id = createdId, Name = createModel.Name };
        createdEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void ReadAll_ReadsAll() {
        var testCurrency1 = new CurrencyEntity { Name = "Some currency" };
        var testCurrency2 = new CurrencyEntity { Name = "Some currency 2" };
        _dbContext.Currencies.Add(testCurrency1);
        _dbContext.Currencies.Add(testCurrency2);
        _dbContext.SaveChanges();

        var readModels = _currencyService.ReadAll();
        var mappedModels = _mapper.ToModels(_dbContext.Currencies.ToList());

        readModels.Should().BeEquivalentTo(mappedModels);
    }

    [Fact]
    public void Update_UpdatesName_WhenExistingId() {
        const string oldName = "Some currency";
        const string newName = "Some currency 2";
        var testCurrency1 = new CurrencyEntity { Name = oldName };
        var insertedEntity = _dbContext.Currencies.Add(testCurrency1);
        _dbContext.SaveChanges();
        var updateModel = new CurrencyUpdateModel(newName);

        var updateSuccess = _currencyService.Update(insertedEntity.Entity.Id, updateModel);

        updateSuccess.Should().BeTrue();
        var updatedEntity = _dbContext.Currencies.Find(insertedEntity.Entity.Id);
        var expectedEntity = insertedEntity.Entity with { Name = newName };
        updatedEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void Update_ReturnsFalse_WhenNotFound() {
        var updateModel = new CurrencyUpdateModel("Some currency");

        var updateSuccess = _currencyService.Update(42, updateModel);

        updateSuccess.Should().BeFalse();
    }

    public void Dispose() {
        _dbContext.Dispose();
    }

    public async ValueTask DisposeAsync() {
        await _dbContext.DisposeAsync();
    }
}
