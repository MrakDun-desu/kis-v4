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

    public AccountTransactionReadAllResponse ReadAll(AccountTransactionReadAllRequest req) {
        var reqTime = _timeProvider.GetUtcNow();
        var query = _dbContext.AccountTransactions
            .Where(at => at.AccountId == req.AccountId)
            .AsQueryable();

        if (req.From is not null) {
            query = query.Where(at => at.Timestamp > req.From);
        }
        if (req.To is not null) {
            query = query.Where(at => at.Timestamp < req.To);
        }

        var total = query.Sum(at => at.Amount);

        return query.Paginate(
                req,
                at => new AccountTransactionModel {
                    Timestamp = at.Timestamp,
                    Amount = at.Amount,
                    SaleTransactionId = at.SaleTransactionId
                },
                (data, meta) => new AccountTransactionReadAllResponse {
                    From = req.From ?? DateTimeOffset.MinValue,
                    To = req.To ?? reqTime,
                    AccountId = req.AccountId,
                    Data = data,
                    Meta = meta,
                    Total = total
                },
                at => at.Timestamp,
                true
            );
    }
}
