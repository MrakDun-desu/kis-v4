using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class AccountTransactionService(
        KisDbContext dbContext,
        TimeProvider timeProvider
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;
    private readonly TimeProvider _timeProvider = timeProvider;

    public async Task<AccountTransactionReadAllResponse> ReadAllAsync(
            AccountTransactionReadAllRequest req,
            CancellationToken token = default
            ) {
        var reqTime = _timeProvider.GetUtcNow();
        var query = _dbContext.AccountTransactions
                .Where(at => at.AccountId == req.AccountId)
                .Where(at => !at.Cancelled)
                .Include(at => at.SaleTransaction)
                .Include(at => at.Account)
                .ThenInclude(a => (a as UserAccount)!.User)
                .Include(at => at.Account)
                .ThenInclude(a => (a as CashBoxAccount)!.Cashbox)
                .AsQueryable();

        var total = await query
            .SumAsync(at => at.Amount, token);

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

        return await query.PaginateAsync(
                req,
                at => new AccountTransactionModel {
                    Amount = at.Amount,
                    SaleTransactionId = at.SaleTransactionId,
                    Timestamp = at.SaleTransaction!.ClosedAt ?? at.SaleTransaction!.StartedAt,
                    Type = at.Type,
                    Account = at.Account switch {
                        CashBoxAccount cba => new CashBoxAccountModel {
                            Id = cba.Id,
                            CashBox = new CashBoxListModel {
                                Id = cba.Cashbox!.Id,
                                Name = cba.Cashbox.Name,
                            },
                        },
                        UserAccount ua => new UserAccountModel {
                            Id = ua.Id,
                            User = new UserListModel {
                                Id = ua.User!.Id,
                                Nick = ua.User.Nick
                            },
                        },
                        _ => throw new ArgumentOutOfRangeException("Nonexistent account type")
                    }
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
                orderDesc: true,
                materializeBeforeMapping: true,
                token: token
            );
    }
}
