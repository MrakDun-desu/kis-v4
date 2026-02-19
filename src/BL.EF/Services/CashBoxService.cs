using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task<CashBoxReadAllResponse> ReadAllAsync(CancellationToken token = default) {
        var data = await _dbContext.Cashboxes.Select(
            cb => new CashBoxListModel {
                Id = cb.Id,
                Name = cb.Name
            }
        ).ToArrayAsync();

        return new CashBoxReadAllResponse { Data = data };
    }

    public async Task<CashBoxCreateResponse> CreateAsync(
            CashBoxCreateRequest req,
            CancellationToken token = default
            ) {
        var entity = new Cashbox {
            Name = req.Name,
        };

        _dbContext.Cashboxes.Add(entity);
        await _dbContext.SaveChangesAsync(token);

        return new CashBoxCreateResponse { Id = entity.Id, Name = entity.Name };
    }

    public async Task<CashBoxReadResponse?> ReadAsync(int id, CancellationToken token = default) {
        var entity = await _dbContext.Cashboxes.FindAsync(id, token);
        if (entity is null) {
            return null;
        }

        var stockTakings = await _stockTakingService.ReadAllAsync(new StockTakingReadAllRequest {
            CashBoxId = id,
        }, token);

        var accountTransactionsFrom = stockTakings.Data.FirstOrDefault()?.Timestamp;

        var donationsTransactions = await _accountTransactionService.ReadAllAsync(new() {
            AccountId = entity.DonationsAccountId,
            From = accountTransactionsFrom
        }, token);

        var salesTransacions = await _accountTransactionService.ReadAllAsync(new() {
            AccountId = entity.SalesAccountId,
            From = accountTransactionsFrom
        }, token);

        return new CashBoxReadResponse {
            Id = entity.Id,
            Name = entity.Name,
            StockTakings = stockTakings,
            DonationsTransactions = donationsTransactions,
            SalesTransactions = salesTransacions
        };
    }

    public async Task<CashBoxUpdateResponse?> UpdateAsync(int id, CashBoxUpdateRequest req, CancellationToken token = default) {
        var entity = await _dbContext.Cashboxes.FindAsync(id, token);
        if (entity is null) {
            return null;
        }

        entity.Name = req.Name;
        entity.Deleted = false;

        _dbContext.Cashboxes.Update(entity);
        await _dbContext.SaveChangesAsync(token);

        return new CashBoxUpdateResponse { Id = entity.Id, Name = entity.Name, };
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken token = default) {
        var entity = await _dbContext.Cashboxes.FindAsync(id, token);
        if (entity == null) {
            return false;
        }

        entity.Deleted = true;
        // no need to mark account transactions as cancelled since they won't show up anywhere
        // anyways. Also deleting a cashbox shouldn't mean that its transactions just disappear

        await _dbContext.SaveChangesAsync(token);

        return true;
    }
}
