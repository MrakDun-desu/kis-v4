using BL.EF.Tests.Extensions;
using BL.EF.Tests.Fixtures;
using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.Extensions.Time.Testing;

namespace BL.EF.Tests.Services;

[Collection(DockerDatabaseTests.Name)]
public class UserServiceTests : IDisposable, IAsyncDisposable {
    private readonly KisDbContext _referenceDbContext;
    private readonly KisDbContext _normalDbContext;
    private readonly UserService _userService;
    private readonly FakeTimeProvider _timeProvider = new();

    public UserServiceTests(KisDbContextFactory dbContextFactory) {
        (_referenceDbContext, _normalDbContext) = dbContextFactory.CreateDbContextAndReference();
        _userService = new UserService(
            _normalDbContext,
            new CurrencyChangeService(_normalDbContext),
            new DiscountUsageService(_normalDbContext)
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
    public void Create_Creates_WhenUserDoesntExist() {
        // arrange
        _referenceDbContext.UserAccounts.RemoveRange(_referenceDbContext.UserAccounts);
        const string username = "Some user";

        // act
        var userId = _userService.CreateOrGetId(username);

        // assert
        var user = _referenceDbContext.UserAccounts.Find(userId);
        user.Should().NotBeNull();
        user!.UserName.Should().Be(username);
    }

    [Fact]
    public void Create_GetsId_WhenUserExists() {
        // arrange
        const string username = "Some user";
        var entity = _referenceDbContext.UserAccounts.Add(new UserAccountEntity { UserName = username });
        _referenceDbContext.SaveChanges();

        // act
        var userId = _userService.CreateOrGetId(username);

        // assert
        userId.Should().Be(entity.Entity.Id);
    }

    [Fact]
    public void ReadAll_ReadsAll_WhenNoFilters() {
        // arrange
        var testUser1 = new UserAccountEntity { UserName = "Some user" };
        var testUser2 = new UserAccountEntity { UserName = "Some user 2", Deleted = true };
        _referenceDbContext.UserAccounts.Add(testUser1);
        _referenceDbContext.UserAccounts.Add(testUser2);
        _referenceDbContext.SaveChanges();

        // act
        var readModels = _userService.ReadAll(null, null, null);

        // assert
        var mappedModels =
            _referenceDbContext.UserAccounts
            .Page(1, Constants.DefaultPageSize, Mapper.ToModels);
        readModels.Should().HaveValue(mappedModels.AsT0);
    }

    [Fact]
    public void ReadAll_DoesntReadDeleted_WhenFilteringByDeleted() {
        // arrange
        var testUser1 = new UserAccountEntity { UserName = "Some user" };
        var testUser2 = new UserAccountEntity { UserName = "Some user 2", Deleted = true };
        _referenceDbContext.UserAccounts.Add(testUser1);
        _referenceDbContext.UserAccounts.Add(testUser2);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();

        // act
        var readModels = _userService.ReadAll(null, null, false);

        // assert
        var mappedModels =
            _referenceDbContext.UserAccounts.Where(u => !u.Deleted)
            .Page(1, Constants.DefaultPageSize, Mapper.ToModels);
        readModels.Should().HaveValue(mappedModels.AsT0);
    }

    [Fact]
    public void Read_ReturnsNotFound_WhenNotFound() {
        // act
        var returnedModel = _userService.Read(42);

        // assert
        returnedModel.Should().BeNotFound();
    }

    [Fact]
    public void Read_ReadsCorrectly_WhenSimple() {
        // arrange
        var testUser = new UserAccountEntity {
            UserName = "Test User",
        };
        _referenceDbContext.UserAccounts.Add(testUser);
        _referenceDbContext.SaveChanges();
        var id = testUser.Id;

        // act
        var returnedModel = _userService.Read(id);

        // assert
        var expectedModel = new UserIntermediateModel(
            testUser,
            [],
            Page<CurrencyChangeListModel>.Empty,
            Page<DiscountUsageListModel>.Empty
        ).ToModel();
        returnedModel.Should().HaveValue(expectedModel);
    }

    [Fact]
    public void Read_ReadsCorrectly_WhenComplex() {
        // arrange
        var testUser = new UserAccountEntity { UserName = "Test user" };
        var currencyChangeTimestamp1 = DateTimeOffset.UtcNow.AddDays(-3);
        var currencyChangeTimestamp2 = DateTimeOffset.UtcNow.AddDays(-1);
        _timeProvider.SetUtcNow(DateTimeOffset.UtcNow);
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
            Currency = currencyChange1.Currency,
            Amount = 10,
            SaleTransaction = new SaleTransactionEntity {
                ResponsibleUser = new UserAccountEntity {
                    UserName = "Some other user"
                },
                Timestamp = currencyChangeTimestamp2
            }
        };
        testUser.CurrencyChanges.Add(currencyChange1);
        testUser.CurrencyChanges.Add(currencyChange2);
        var insertedEntity = _referenceDbContext.UserAccounts.Add(testUser);
        _referenceDbContext.SaveChanges();
        var id = insertedEntity.Entity.Id;

        // act
        var returnedModel = _userService.Read(id);

        // assert
        var currencyChanges = _referenceDbContext.CurrencyChanges.ToList().ToModels();
        var expectedModel = new UserIntermediateModel(
            testUser,
            [
                new TotalCurrencyChangeListModel(currencyChange1.Currency.ToModel(), 20),
            ],
            new Page<CurrencyChangeListModel>(currencyChanges, new PageMeta(1, 20, 1, 2, 2, 1)),
            Page<DiscountUsageListModel>.Empty).ToModel();
        returnedModel.Should().HaveValue(expectedModel);
    }

}
