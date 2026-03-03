using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class AccountTransactionService(
        KisDbContext dbContext,
        TimeProvider timeProvider
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;
    private readonly TimeProvider _timeProvider = timeProvider;

    public async Task<AccountTransactionReadAllResponse> ReadAllAsync(AccountTransactionReadAllRequest req, CancellationToken token = default) {
        var reqTime = _timeProvider.GetUtcNow();
        var query = _dbContext.AccountTransactions
            .Where(at => at.AccountId == req.AccountId)
            .Include(at => at.SaleTransaction)
            .AsQueryable();

        if (req.From is not null) {
            query = query.Where(at =>
                (at.SaleTransaction!.ClosedAt ?? at.SaleTransaction!.StartedAt)
                >= req.From
            );
        }
        if (req.To is not null) {
            query = query.Where(at =>
                (at.SaleTransaction!.ClosedAt ?? at.SaleTransaction!.StartedAt)
                <= req.To
            );
        }

        var total = await query.SumAsync(at => at.Amount, token);

        return await query.PaginateAsync(
                req,
                at => new AccountTransactionModel {
                    Amount = at.Amount,
                    SaleTransactionId = at.SaleTransactionId,
                    Timestamp = at.SaleTransaction!.ClosedAt ?? at.SaleTransaction!.StartedAt,
                    Type = at.Type
                },
                (data, meta) => new AccountTransactionReadAllResponse {
                    From = req.From ?? DateTimeOffset.MinValue,
                    To = req.To ?? reqTime,
                    AccountId = req.AccountId,
                    Data = data,
                    Meta = meta,
                    Total = total
                },
                at => at.SaleTransactionId,
                true,
                token
            );
    }
}
