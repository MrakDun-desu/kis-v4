using BL.EF.Tests.Extensions;
using BL.EF.Tests.Fixtures;
using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;

namespace BL.EF.Tests.Services;

public class
    CashBoxServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable {
    private readonly CashBoxService _cashBoxService;
    private readonly KisDbContext _referenceDbContext;
    private readonly KisDbContext _normalDbContext;
    private readonly FakeTimeProvider _timeProvider = new();

    public CashBoxServiceTests(KisDbContextFactory dbContextFactory) {
        (_referenceDbContext, _normalDbContext) = dbContextFactory.CreateDbContextAndReference();
        _cashBoxService = new CashBoxService(_normalDbContext, new CurrencyChangeService(_normalDbContext), _timeProvider);
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
        var testCashBox1 = new CashBoxEntity { Name = "Some cash box" };
        var testCashBox2 = new CashBoxEntity { Name = "Some cash box 2", Deleted = true };
        _referenceDbContext.CashBoxes.Add(testCashBox1);
        _referenceDbContext.CashBoxes.Add(testCashBox2);
        _referenceDbContext.SaveChanges();

        // act
        var readModels = _cashBoxService.ReadAll(null);

        // assert
        var mappedModels =
            _referenceDbContext.CashBoxes.ToList().ToModels();
        readModels.Should().BeEquivalentTo(mappedModels);
    }

    [Fact]
    public void ReadAll_DoesntReadDeleted() {
        // arrange
        var testCashBox1 = new CashBoxEntity { Name = "Some cash box" };
        var testCashBox2 = new CashBoxEntity { Name = "Some cash box 2", Deleted = true };
        _referenceDbContext.CashBoxes.Add(testCashBox1);
        _referenceDbContext.CashBoxes.Add(testCashBox2);
        _referenceDbContext.SaveChanges();

        // act
        var readModels = _cashBoxService.ReadAll(false);

        // assert
        var mappedModels =
            _referenceDbContext.CashBoxes.Where(cb => !cb.Deleted).ToList().ToModels();
        readModels.Should().BeEquivalentTo(mappedModels);
    }

    [Fact]
    public void Create_Creates_WhenDataIsValid() {
        // arrange
        const string name = "Some cash box";
        var createModel = new CashBoxCreateModel(name);
        var timestamp = DateTimeOffset.UtcNow;
        _timeProvider.SetUtcNow(timestamp);

        // act
        var createdCashBox = _cashBoxService.Create(createModel);

        // assert
        var createdEntity = _referenceDbContext.CashBoxes
            .Include(cb => cb.StockTakings)
            .First(cb => cb.Id == createdCashBox.Id);
        var expectedEntity = new CashBoxEntity {
            Id = createdCashBox.Id,
            Name = name,
            StockTakings =
            {
                new StockTakingEntity
                {
                    CashBox = createdEntity,
                    CashBoxId = createdCashBox.Id,
                    Timestamp = timestamp
                }
            }
        };
        var expectedModel = new CashBoxDetailModel(createdCashBox.Id, createModel.Name, false,
            Page<CurrencyChangeListModel>.Empty, [],
            [new StockTakingModel(timestamp)]);

        createdEntity.Should().BeEquivalentTo(expectedEntity);
        createdCashBox.Should().BeEquivalentTo(expectedModel);
    }

    [Fact]
    public void Update_RestoresEntity_IfWasDeleted() {
        // arrange
        const string oldName = "Some cash box";
        const string newName = "Some cash box 2";
        var stockTaking = new StockTakingEntity { Timestamp = DateTimeOffset.UtcNow };
        var testEntity = new CashBoxEntity {
            Name = oldName,
            Deleted = true,
            StockTakings = { stockTaking }
        };
        stockTaking.CashBox = testEntity;
        _referenceDbContext.CashBoxes.Add(testEntity);
        _referenceDbContext.SaveChanges();
        var id = testEntity.Id;
        var updateModel = new CashBoxCreateModel(newName);
        _referenceDbContext.ChangeTracker.Clear();

        // act
        var updateResult = _cashBoxService.Update(id, updateModel);

        // assert
        var updatedEntity = _referenceDbContext.CashBoxes
            .Include(cb => cb.StockTakings)
            .First(cb => cb.Id == id);
        testEntity.Deleted = false;
        testEntity.Name = newName;
        updatedEntity.Should().BeEquivalentTo(testEntity, opts => opts.IgnoringCyclicReferences());
        updateResult.Should().HaveValue(testEntity.ToModel());
    }

    [Fact]
    public void Update_UpdatesName_WhenExistingId() {
        // arrange
        const string oldName = "Some cash box";
        const string newName = "Some cash box 2";
        var testEntity = new CashBoxEntity { Name = oldName, StockTakings = { new StockTakingEntity { Timestamp = DateTimeOffset.UtcNow } } };
        _referenceDbContext.CashBoxes.Add(testEntity);
        _referenceDbContext.SaveChanges();
        var id = testEntity.Id;
        var updateModel = new CashBoxCreateModel(newName);
        _referenceDbContext.ChangeTracker.Clear();

        // act
        var updateResult = _cashBoxService.Update(id, updateModel);

        // assert
        var updatedEntity = _referenceDbContext.CashBoxes
            .Include(cb => cb.StockTakings)
            .First(cb => cb.Id == id);
        testEntity.Name = newName;
        updatedEntity.Should().BeEquivalentTo(testEntity, opts => opts.IgnoringCyclicReferences());
        updateResult.Should().HaveValue(testEntity.ToModel());
    }

    [Fact]
    public void Update_ReturnsNotFound_WhenNotFound() {
        // arrange
        var updateModel = new CashBoxCreateModel("Some cash box");

        // act
        var updateResult = _cashBoxService.Update(42, updateModel);

        // assert
        updateResult.Should().BeNotFound();
    }

    [Fact]
    public void Read_ReturnsNotFound_WhenNotFound() {
        // act
        var returnedModel = _cashBoxService.Read(42);

        // assert
        returnedModel.Should().BeNotFound();
    }

    [Fact]
    public void Read_ReadsCorrectly_WhenSimple() {
        // arrange
        var timestamp = DateTimeOffset.UtcNow;
        var testCashBox = new CashBoxEntity {
            Name = "Test cash box",
            StockTakings = { new StockTakingEntity { Timestamp = timestamp } }
        };
        _referenceDbContext.CashBoxes.Add(testCashBox);
        _referenceDbContext.SaveChanges();
        var id = testCashBox.Id;

        // act
        var returnedModel = _cashBoxService.Read(id);

        // assert
        var expectedModel = testCashBox.ToModel();
        returnedModel.Should().HaveValue(expectedModel);
    }

    [Fact]
    public void Read_ReadsCorrectly_WithoutFilters() {
        // arrange
        var testCashBox = new CashBoxEntity { Name = "Test cash box" };
        var stockTakingTimestamp = DateTimeOffset.UtcNow.AddDays(-2);
        var currencyChangeTimestamp1 = DateTimeOffset.UtcNow.AddDays(-3);
        var currencyChangeTimestamp2 = DateTimeOffset.UtcNow.AddDays(-1);
        _timeProvider.SetUtcNow(DateTimeOffset.UtcNow);
        testCashBox.StockTakings.Add(new StockTakingEntity { Timestamp = stockTakingTimestamp });
        var currencyChange1 = new CurrencyChangeEntity {
            Currency = new CurrencyEntity { Name = "Some currency" },
            Amount = 10,
            SaleTransaction = new SaleTransactionEntity {
                ResponsibleUser = new UserAccountEntity {
                    UserName = "Some user"
                },
                Timestamp = currencyChangeTimestamp1
            }
        };
        var currencyChange2 = new CurrencyChangeEntity {
            Currency = new CurrencyEntity { Name = "Some currency" },
            Amount = 10,
            SaleTransaction = new SaleTransactionEntity {
                ResponsibleUser = new UserAccountEntity {
                    UserName = "Some other user"
                },
                Timestamp = currencyChangeTimestamp2
            }
        };
        testCashBox.CurrencyChanges.Add(currencyChange1);
        testCashBox.CurrencyChanges.Add(currencyChange2);
        var insertedEntity = _referenceDbContext.CashBoxes.Add(testCashBox);
        _referenceDbContext.SaveChanges();
        var id = insertedEntity.Entity.Id;

        // act
        var returnedModel = _cashBoxService.Read(id);

        // assert
        var expectedModel = new CashBoxIntermediateModel(
                insertedEntity.Entity,
                new Page<CurrencyChangeListModel>(new List<CurrencyChangeEntity> { currencyChange2 }.ToModels(),
                    new PageMeta(1, Constants.DefaultPageSize, 1, 1, 1, 1)),
                [new TotalCurrencyChangeListModel(currencyChange2.Currency.ToModel(), currencyChange2.Amount)])
            .ToDetailModel();
        returnedModel.Should().HaveValue(expectedModel);
    }

    [Fact]
    public void Read_ReadsCorrectly_WithFilters() {
        // arrange
        var testCashBox = new CashBoxEntity { Name = "Test cash box" };
        var stockTakingTimestamp = DateTimeOffset.UtcNow.AddDays(-2);
        var currencyChangeTimestamp1 = DateTimeOffset.UtcNow.AddDays(-3);
        var currencyChangeTimestamp2 = DateTimeOffset.UtcNow.AddDays(-1);
        _timeProvider.SetUtcNow(DateTimeOffset.UtcNow);
        testCashBox.StockTakings.Add(new StockTakingEntity { Timestamp = stockTakingTimestamp });
        var currencyChange1 = new CurrencyChangeEntity {
            Currency = new CurrencyEntity { Name = "Some currency" },
            Amount = 10,
            SaleTransaction = new SaleTransactionEntity {
                ResponsibleUser = new UserAccountEntity {
                    UserName = "Some user"
                },
                Timestamp = currencyChangeTimestamp1
            }
        };
        var currencyChange2 = new CurrencyChangeEntity {
            Currency = new CurrencyEntity { Name = "Some currency" },
            Amount = 10,
            SaleTransaction = new SaleTransactionEntity {
                ResponsibleUser = new UserAccountEntity {
                    UserName = "Some other user"
                },
                Timestamp = currencyChangeTimestamp2
            }
        };
        testCashBox.CurrencyChanges.Add(currencyChange1);
        testCashBox.CurrencyChanges.Add(currencyChange2);
        var insertedEntity = _referenceDbContext.CashBoxes.Add(testCashBox);
        _referenceDbContext.SaveChanges();
        var id = insertedEntity.Entity.Id;

        // act
        var returnedModel = _cashBoxService.Read(id, currencyChangeTimestamp1.AddDays(-1), stockTakingTimestamp);

        // assert
        var expectedModel = new CashBoxIntermediateModel(
                insertedEntity.Entity,
                new Page<CurrencyChangeListModel>(new List<CurrencyChangeEntity> { currencyChange1 }.ToModels(),
                    new PageMeta(1, Constants.DefaultPageSize, 1, 1, 1, 1)),
                [new TotalCurrencyChangeListModel(currencyChange1.Currency.ToModel(), currencyChange1.Amount)])
            .ToDetailModel();
        returnedModel.Should().HaveValue(expectedModel);
    }

    [Fact]
    public void Read_ReadsCorrectly_WithAggregation() {
        // arrange
        var testCashBox = new CashBoxEntity { Name = "Test cash box" };
        var stockTakingTimestamp = DateTimeOffset.UtcNow.AddDays(-3);
        var currencyChangeTimestamp1 = DateTimeOffset.UtcNow.AddDays(-2);
        var currencyChangeTimestamp2 = DateTimeOffset.UtcNow.AddDays(-1);
        _timeProvider.SetUtcNow(DateTimeOffset.UtcNow);
        testCashBox.StockTakings.Add(new StockTakingEntity { Timestamp = stockTakingTimestamp });
        var currency = new CurrencyEntity { Name = "Some currency" };
        var currencyChange1 = new CurrencyChangeEntity {
            Currency = currency,
            Amount = 10,
            SaleTransaction = new SaleTransactionEntity {
                ResponsibleUser = new UserAccountEntity {
                    UserName = "Some user"
                },
                Timestamp = currencyChangeTimestamp1
            }
        };
        var currencyChange2 = new CurrencyChangeEntity {
            Currency = currency,
            Amount = 10,
            SaleTransaction = new SaleTransactionEntity {
                ResponsibleUser = new UserAccountEntity {
                    UserName = "Some other user"
                },
                Timestamp = currencyChangeTimestamp2
            }
        };
        testCashBox.CurrencyChanges.Add(currencyChange1);
        testCashBox.CurrencyChanges.Add(currencyChange2);
        var insertedEntity = _referenceDbContext.CashBoxes.Add(testCashBox);
        _referenceDbContext.SaveChanges();
        var id = insertedEntity.Entity.Id;

        // act
        var returnedModel = _cashBoxService.Read(id);

        // assert
        var expectedModel = new CashBoxIntermediateModel(
            insertedEntity.Entity,
            new Page<CurrencyChangeListModel>(
                new List<CurrencyChangeEntity> { currencyChange1, currencyChange2 }.ToModels(),
                new PageMeta(1, Constants.DefaultPageSize, 1, 2, 2, 1)),
            [
                new TotalCurrencyChangeListModel(currency.ToModel(), currencyChange1.Amount + currencyChange2.Amount)
            ]).ToDetailModel();
        returnedModel.Should().HaveValue(expectedModel);
    }

    [Fact]
    public void Delete_Deletes_WhenExistingId() {
        var testCashBox1 = new CashBoxEntity { Name = "Some category", StockTakings = { new StockTakingEntity { Timestamp = DateTimeOffset.UtcNow } } };
        _referenceDbContext.CashBoxes.Add(testCashBox1);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();

        var deleteResult = _cashBoxService.Delete(testCashBox1.Id);

        var deletedEntity = _referenceDbContext.CashBoxes
            .Include(cb => cb.StockTakings)
            .Single(cb => cb.Id == testCashBox1.Id);
        testCashBox1.Deleted = true;
        deletedEntity.Should().BeEquivalentTo(testCashBox1, opts => opts.IgnoringCyclicReferences());
        deleteResult.Should().HaveValue(testCashBox1.ToModel());
    }

    [Fact]
    public void AddStockTaking_AddsStockTaking_WhenExistingId() {
        var testCashBox1 = new CashBoxEntity { Name = "Some category" };
        var insertedEntity = _referenceDbContext.CashBoxes.Add(testCashBox1);
        _referenceDbContext.SaveChanges();
        var returnedDateTime = DateTimeOffset.UtcNow;
        _timeProvider.SetUtcNow(returnedDateTime);

        var stockTakingSuccess = _cashBoxService.AddStockTaking(insertedEntity.Entity.Id);

        stockTakingSuccess.Should().BeSuccess();
        var createdEntity =
            _referenceDbContext.StockTakings
                .First(st => st.CashBoxId == insertedEntity.Entity.Id);
        var expectedEntity = new StockTakingEntity {
            Timestamp = returnedDateTime,
            CashBox = insertedEntity.Entity,
            CashBoxId = insertedEntity.Entity.Id
        };
        createdEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public void AddStockTaking_ReturnsNotFound_WhenNotFound() {
        var stockTakingSuccess = _cashBoxService.AddStockTaking(42);

        stockTakingSuccess.Should().BeNotFound();
    }
}
