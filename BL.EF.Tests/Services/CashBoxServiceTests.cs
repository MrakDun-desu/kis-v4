using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.Extensions.Time.Testing;
using Xunit.Abstractions;

namespace BL.EF.Tests.Services;

public class
    CashBoxServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable {
    private readonly CashBoxService _cashBoxService;
    private readonly KisDbContext _dbContext;
    private readonly Mapper _mapper;
    private readonly FakeTimeProvider _timeProvider = new();
    private readonly ITestOutputHelper _output;

    public CashBoxServiceTests(KisDbContextFactory dbContextFactory, ITestOutputHelper output) {
        _dbContext = dbContextFactory.CreateDbContext();
        _mapper = new Mapper();
        _cashBoxService = new CashBoxService(_dbContext, _mapper, _timeProvider);
        _output = output;
    }

    public void Dispose() {
        _dbContext.Dispose();
    }

    public async ValueTask DisposeAsync() {
        await _dbContext.DisposeAsync();
    }

    [Fact]
    public void ReadAll_ReadsAll() {
        var testCashbox1 = new CashBoxEntity { Name = "Some cash box" };
        var testCashbox2 = new CashBoxEntity { Name = "Some cash box 2" };
        _dbContext.CashBoxes.Add(testCashbox1);
        _dbContext.CashBoxes.Add(testCashbox2);
        _dbContext.SaveChanges();

        var readModels = _cashBoxService.ReadAll();
        var mappedModels =
            _mapper.ToModels(_dbContext.CashBoxes.Where(cb => !cb.Deleted).ToList());

        readModels.Should().BeEquivalentTo(mappedModels);
    }

    [Fact]
    public void ReadAll_DoesntRead_WhenDeleted() {
        var testCashbox1 = new CashBoxEntity { Name = "Some cash box" };
        var testCashbox2 = new CashBoxEntity { Name = "Some cash box 2", Deleted = true };
        _dbContext.CashBoxes.Add(testCashbox1);
        _dbContext.CashBoxes.Add(testCashbox2);
        _dbContext.SaveChanges();

        var readModels = _cashBoxService.ReadAll();
        var mappedModels = _mapper.ToModels(_dbContext.CashBoxes.Where(cb => !cb.Deleted).ToList());

        readModels.Should().BeEquivalentTo(mappedModels);
    }

    [Fact]
    public void Create_Creates_WhenDataIsValid() {
        const string name = "Some cash box";
        var createModel = new CashBoxCreateModel(name);
        var createdId = _cashBoxService.Create(createModel);

        var createdEntity = _dbContext.CashBoxes.Find(createdId);
        var expectedEntity = new CashBoxEntity { Id = createdId, Name = name };

        createdEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void Update_UpdatesName_WhenExistingId() {
        const string oldName = "Some cash box";
        const string newName = "Some cash box 2";
        var testEntity = new CashBoxEntity { Name = oldName };
        var insertedEntity = _dbContext.CashBoxes.Add(testEntity);
        _dbContext.SaveChanges();
        var updateModel = new CashBoxUpdateModel(newName);

        var updateSuccess = _cashBoxService.Update(insertedEntity.Entity.Id, updateModel);

        updateSuccess.Should().BeTrue();
        var updatedEntity = _dbContext.CashBoxes.Find(insertedEntity.Entity.Id);
        var expectedEntity = insertedEntity.Entity with { Name = newName };
        updatedEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void Update_ReturnsFalse_WhenNotFound() {
        var updateModel = new CashBoxUpdateModel("Some cash box");

        var updateSuccess = _cashBoxService.Update(42, updateModel);

        updateSuccess.Should().BeFalse();
    }

    [Fact]
    public void Delete_Deletes_WhenExistingId() {
        var testCashBox1 = new CashBoxEntity { Name = "Some category" };
        var insertedEntity = _dbContext.CashBoxes.Add(testCashBox1);
        _dbContext.SaveChanges();

        var deleteSuccess = _cashBoxService.Delete(insertedEntity.Entity.Id);

        deleteSuccess.Should().BeTrue();
        var deletedEntity = _dbContext.CashBoxes.Find(insertedEntity.Entity.Id);
        var expectedEntity = insertedEntity.Entity with { Deleted = true };
        deletedEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void Delete_ReturnsFalse_WhenNotFound() {
        var deleteSuccess = _cashBoxService.Delete(42);

        deleteSuccess.Should().BeFalse();
    }

    [Fact]
    public void AddStockTaking_AddsStockTaking_WhenExistingId() {
        var testCashBox1 = new CashBoxEntity { Name = "Some category" };
        var insertedEntity = _dbContext.CashBoxes.Add(testCashBox1);
        _dbContext.SaveChanges();
        var returnedDateTime = DateTimeOffset.UtcNow;
        _timeProvider.SetUtcNow(returnedDateTime);
        _timeProvider.SetLocalTimeZone(TimeZoneInfo.Utc);

        var stockTakingSuccess = _cashBoxService.AddStockTaking(insertedEntity.Entity.Id);

        stockTakingSuccess.Should().BeTrue();
        var createdEntity =
            _dbContext.StockTakings
                .First(st => st.CashboxId == insertedEntity.Entity.Id);
        createdEntity.CashboxId.Should().Be(insertedEntity.Entity.Id);
        createdEntity.Timestamp.Should().Be(returnedDateTime);
    }

    [Fact]
    public void AddStockTaking_ReturnsFalse_WhenNotFound() {
        var stockTakingSuccess = _cashBoxService.AddStockTaking(42);

        stockTakingSuccess.Should().BeFalse();
    }

    [Fact]
    public void Read_ReturnsNull_WhenNotFound() {
        var returnedModel = _cashBoxService.Read(42);

        returnedModel.Should().BeNull();
    }

    [Fact]
    public void Read_ReadsCorrectly_WhenSimple() {
        var testCashBox = new CashBoxEntity { Name = "Test cash box" };
        var insertedEntity = _dbContext.CashBoxes.Add(testCashBox);
        _dbContext.SaveChanges();
        var id = insertedEntity.Entity.Id;

        var returnedModel = _cashBoxService.Read(id);

        var expectedModel = _mapper.ToModel(insertedEntity.Entity);
        returnedModel.Should().BeEquivalentTo(expectedModel);
    }

    [Fact]
    public void Read_ReadsCorrectly_WhenComplex() {
        // add a stock-taking and two transactions, one of which is before the stock-taking and
        // second which is after the stock-taking. Read should only get the currency changes after
        // the stock-taking
        var testCashBox = new CashBoxEntity { Name = "Test cash box" };
        var stockTakingTimestamp = DateTimeOffset.UtcNow.AddDays(-2);
        var currencyChangeTimestamp1 = DateTimeOffset.UtcNow.AddDays(-3);
        var currencyChangeTimestamp2 = DateTimeOffset.UtcNow.AddDays(-1);
        testCashBox.StockTakings.Add(new StockTakingEntity { Timestamp = stockTakingTimestamp });
        testCashBox.CurrencyChanges.Add(new CurrencyChangeEntity {
            Currency = new CurrencyEntity { Name = "Some currency" },
            Amount = 10,
            SaleTransaction = new SaleTransactionEntity {
                ResponsibleUser = new UserAccountEntity {
                    Name = "Some user"
                },
                Timestamp = currencyChangeTimestamp1
            }
        });
        testCashBox.CurrencyChanges.Add(new CurrencyChangeEntity {
            Currency = new CurrencyEntity { Name = "Some currency" },
            Amount = 10,
            SaleTransaction = new SaleTransactionEntity {
                ResponsibleUser = new UserAccountEntity {
                    Name = "Some user"
                },
                Timestamp = currencyChangeTimestamp2
            }
        });
        var insertedEntity = _dbContext.CashBoxes.Add(testCashBox);
        _dbContext.SaveChanges();
        _output.WriteLine(insertedEntity.ToString());
        var id = insertedEntity.Entity.Id;

        var returnedModel = _cashBoxService.Read(id);

        var expectedModel = _mapper.ToModel(insertedEntity.Entity);
        expectedModel!.CurrencyChanges.Remove(expectedModel.CurrencyChanges.First());
        returnedModel.Should().BeEquivalentTo(expectedModel);
    }
}
