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

namespace BL.EF.Tests.Services;

public class CurrencyChangeServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable
{
    private readonly CurrencyChangeService _currencyChangeService;
    private readonly KisDbContext _referenceDbContext;
    private readonly KisDbContext _normalDbContext;

    public CurrencyChangeServiceTests(KisDbContextFactory dbContextFactory)
    {
        (_referenceDbContext, _normalDbContext) = dbContextFactory.CreateDbContextAndReference();
        _currencyChangeService = new CurrencyChangeService(_normalDbContext);
    }

    public async ValueTask DisposeAsync()
    {
        await _referenceDbContext.DisposeAsync();
        await _normalDbContext.DisposeAsync();
    }

    public void Dispose()
    {
        _referenceDbContext.Dispose();
        _normalDbContext.Dispose();
    }

    [Fact]
    public void ReadAll_ReadsAll_WhenNoFilters()
    {
        // arrange
        var testAccount1 = new UserAccountEntity { UserName = "Some user" };
        var testAccount2 = new UserAccountEntity { UserName = "Some other user" };
        var testCurrency1 = new CurrencyEntity { Name = "Czech crowns" };
        var testCurrencyChange1 = new CurrencyChangeEntity
        {
            Currency = testCurrency1,
            Amount = 10,
            Account = testAccount1,
            SaleTransaction = new SaleTransactionEntity
            {
                ResponsibleUser = testAccount2,
                Timestamp = DateTimeOffset.UtcNow.AddDays(-10)
            }
        };
        var testCurrencyChange2 = new CurrencyChangeEntity
        {
            Currency = testCurrency1,
            Amount = 10,
            Account = testAccount2,
            Cancelled = true,
            SaleTransaction = new SaleTransactionEntity
            {
                ResponsibleUser = testAccount2,
                Timestamp = DateTimeOffset.UtcNow.AddDays(-5)
            }
        };
        _referenceDbContext.CurrencyChanges.Add(testCurrencyChange1);
        _referenceDbContext.CurrencyChanges.Add(testCurrencyChange2);

        // act
        var readResult = _currencyChangeService.ReadAll(null, null, null, null, null, null);

        // assert
        var expectedPage = _referenceDbContext.CurrencyChanges
            .Include(cc => cc.SaleTransaction)
            .Include(cc => cc.Currency)
            .OrderByDescending(cc => cc.SaleTransaction!.Timestamp)
            .Page(1, Constants.DefaultPageSize, Mapper.ToModels);
        readResult.IsT0.Should().BeTrue();
        readResult.Should().HaveValue(expectedPage.Value);
    }

    [Fact]
    public void ReadAll_ReadsCorrectly_WhenFilteringByAccount()
    {
        // arrange
        var testAccount1 = new UserAccountEntity { UserName = "Some user" };
        var testAccount2 = new UserAccountEntity { UserName = "Some other user" };
        var testCurrency1 = new CurrencyEntity { Name = "Czech crowns" };
        var testCurrencyChange1 = new CurrencyChangeEntity
        {
            Currency = testCurrency1,
            Amount = 10,
            Account = testAccount1,
            SaleTransaction = new SaleTransactionEntity
            {
                ResponsibleUser = testAccount2,
                Timestamp = DateTimeOffset.UtcNow.AddDays(-10)
            }
        };
        var testCurrencyChange2 = new CurrencyChangeEntity
        {
            Currency = testCurrency1,
            Amount = 10,
            Account = testAccount2,
            Cancelled = true,
            SaleTransaction = new SaleTransactionEntity
            {
                ResponsibleUser = testAccount2,
                Timestamp = DateTimeOffset.UtcNow.AddDays(-5)
            }
        };
        _referenceDbContext.CurrencyChanges.Add(testCurrencyChange1);
        _referenceDbContext.CurrencyChanges.Add(testCurrencyChange2);
        _referenceDbContext.SaveChanges();

        // act
        var readResult = _currencyChangeService.ReadAll(null, null, testAccount1.Id, null, null, null);

        // assert
        var expectedPage = _referenceDbContext.CurrencyChanges
            .Include(cc => cc.SaleTransaction)
            .Include(cc => cc.Currency)
            .OrderByDescending(cc => cc.SaleTransaction!.Timestamp)
            .Where(cc => cc.AccountId == testAccount1.Id)
            .Page(1, Constants.DefaultPageSize, Mapper.ToModels);
        readResult.IsT0.Should().BeTrue();
        readResult.Should().HaveValue(expectedPage.Value);
    }

    [Fact]
    public void ReadAll_ReadsCorrectly_WhenFilteringByCancelled()
    {
        // arrange
        var testAccount1 = new UserAccountEntity { UserName = "Some user" };
        var testAccount2 = new UserAccountEntity { UserName = "Some other user" };
        var testCurrency1 = new CurrencyEntity { Name = "Czech crowns" };
        var testCurrencyChange1 = new CurrencyChangeEntity
        {
            Currency = testCurrency1,
            Amount = 10,
            Account = testAccount1,
            SaleTransaction = new SaleTransactionEntity
            {
                ResponsibleUser = testAccount2,
                Timestamp = DateTimeOffset.UtcNow.AddDays(-10)
            }
        };
        var testCurrencyChange2 = new CurrencyChangeEntity
        {
            Currency = testCurrency1,
            Amount = 10,
            Account = testAccount2,
            Cancelled = true,
            SaleTransaction = new SaleTransactionEntity
            {
                ResponsibleUser = testAccount2,
                Timestamp = DateTimeOffset.UtcNow.AddDays(-5)
            }
        };
        _referenceDbContext.CurrencyChanges.Add(testCurrencyChange1);
        _referenceDbContext.CurrencyChanges.Add(testCurrencyChange2);

        // act
        var readResult = _currencyChangeService.ReadAll(null, null, null, true, null, null);

        // assert
        var expectedPage = _referenceDbContext.CurrencyChanges
            .Include(cc => cc.SaleTransaction)
            .Include(cc => cc.Currency)
            .OrderByDescending(cc => cc.SaleTransaction!.Timestamp)
            .Where(cc => cc.Cancelled)
            .Page(1, Constants.DefaultPageSize, Mapper.ToModels);
        readResult.IsT0.Should().BeTrue();
        readResult.Should().HaveValue(expectedPage.Value);
    }

    [Fact]
    public void ReadAll_ReturnsErrors_WhenFilteringByNonexistentAccount()
    {
        // arrange
        var testAccount1 = new UserAccountEntity { UserName = "Some user" };
        var testAccount2 = new UserAccountEntity { UserName = "Some other user" };
        var testCurrency1 = new CurrencyEntity { Name = "Czech crowns" };
        var testCurrencyChange1 = new CurrencyChangeEntity
        {
            Currency = testCurrency1,
            Amount = 10,
            Account = testAccount1,
            SaleTransaction = new SaleTransactionEntity
            {
                ResponsibleUser = testAccount2,
                Timestamp = DateTimeOffset.UtcNow.AddDays(-10)
            }
        };
        var testCurrencyChange2 = new CurrencyChangeEntity
        {
            Currency = testCurrency1,
            Amount = 10,
            Account = testAccount2,
            Cancelled = true,
            SaleTransaction = new SaleTransactionEntity
            {
                ResponsibleUser = testAccount2,
                Timestamp = DateTimeOffset.UtcNow.AddDays(-5)
            }
        };
        _referenceDbContext.CurrencyChanges.Add(testCurrencyChange1);
        _referenceDbContext.CurrencyChanges.Add(testCurrencyChange2);
        const int accountId = 42;

        // act
        var readResult = _currencyChangeService.ReadAll(null, null, accountId, null, null, null);

        // assert
        readResult.IsT1.Should().BeTrue();
        readResult.Should().HaveValue(new Dictionary<string, string[]>
        {
            {
                nameof(accountId),
                [$"Account with id {accountId} doesn't exist"]
            }
        });
    }
}