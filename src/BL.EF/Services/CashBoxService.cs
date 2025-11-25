using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

public class CashBoxService(
        KisDbContext dbContext,
        TimeProvider timeProvider,
        UserService userService,
        StockTakingService stockTakingService,
        AccountTransactionService accountTransactionService
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly UserService _userService = userService;
    private readonly StockTakingService _stockTakingService = stockTakingService;
    private readonly AccountTransactionService _accountTransactionService = accountTransactionService;

    public CashBoxReadAllResponse ReadAll() {
        var data = _dbContext.Cashboxes.Select(
            cb => new CashBoxListModel {
                Id = cb.Id,
                Name = cb.Name
            }
        );

        return new CashBoxReadAllResponse { Data = data };
    }

    public CashBoxCreateResponse Create(CashBoxCreateRequest req) {
        var entity = new Cashbox {
            Name = req.Name,
        };

        _dbContext.Cashboxes.Add(entity);
        _dbContext.SaveChanges();

        return new CashBoxCreateResponse { Id = entity.Id, Name = entity.Name };
    }

    public OneOf<StockTakingCreateResponse, NotFound> StockTaking(int id, int userId) {
        var reqTime = _timeProvider.GetUtcNow();

        var cashbox = _dbContext.Cashboxes.Find(id);
        if (cashbox is null) {
            return new NotFound();
        }

        var user = _userService.GetOrCreate(userId);

        var stockTaking = new StockTaking {
            Timestamp = reqTime,
            CashBoxId = id,
            UserId = userId
        };

        _dbContext.StockTakings.Add(stockTaking);
        _dbContext.SaveChanges();

        return new StockTakingCreateResponse {
            Timestamp = stockTaking.Timestamp,
            CashBoxId = stockTaking.CashBoxId,
            User = user
        };
    }

    public OneOf<CashBoxReadResponse, NotFound> Read(int id) {
        var cashbox = _dbContext.Cashboxes.Find(id);
        if (cashbox is null) {
            return new NotFound();
        }

        var stockTakings = _stockTakingService.ReadAll(new StockTakingReadAllRequest {
            CashBoxId = id,
        });

        var accountTransactionsFrom = stockTakings.Data.FirstOrDefault()?.Timestamp;

        var donationsTransactions = _accountTransactionService.ReadAll(new() {
            AccountId = cashbox.DonationsAccountId,
            From = accountTransactionsFrom
        });

        var salesTransacions = _accountTransactionService.ReadAll(new() {
            AccountId = cashbox.SalesAccountId,
            From = accountTransactionsFrom
        });

        return new CashBoxReadResponse {
            Id = cashbox.Id,
            Name = cashbox.Name,
            StockTakings = stockTakings,
            DonationsTransactions = donationsTransactions,
            SalesTransactions = salesTransacions
        };
    }

    public OneOf<CashBoxUpdateResponse, NotFound> Update(int id, CashBoxUpdateRequest req) {
        var cashbox = _dbContext.Cashboxes.Find(id);
        if (cashbox is null) {
            return new NotFound();
        }

        cashbox.Name = req.Name;

        _dbContext.SaveChanges();

        return new CashBoxUpdateResponse { Id = cashbox.Id, Name = cashbox.Name, };
    }

    public bool Delete(int id) {
        var cashbox = _dbContext.Cashboxes.Find(id);
        if (cashbox == null) {
            return false;
        }

        cashbox.Deleted = true;
        // no need to mark account transactions as cancelled since they won't show up anywhere
        // anyways. Also deleting a cashbox shouldn't mean that its transactions just disappear

        _dbContext.SaveChanges();

        return true;
    }
}
