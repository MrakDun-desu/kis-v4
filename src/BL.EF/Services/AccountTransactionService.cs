using KisV4.Common.DependencyInjection;
using KisV4.Common.Enums;
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
                .Include(at => at.Account)
                .ThenInclude(a => (a as UserAccount)!.User)
                .Include(at => at.Account)
                .ThenInclude(a => (a as CashBoxAccount)!.Cashbox)
                .AsQueryable();

        var total = await query
            .SumAsync(at => at.Amount, token);

        if (req.From is not null) {
            query = query.Where(at => at.Timestamp >= req.From);
        }
        if (req.To is not null) {
            query = query.Where(at => at.Timestamp <= req.To);
        }

        return await query.PaginateAsync(
                req,
                at => new AccountTransactionModel {
                    Amount = at.Amount,
                    SaleTransactionId = at.SaleTransactionId,
                    Timestamp = at.Timestamp,
                    Type = at.Type,
                    Account = at.Account switch {
                        CashBoxAccount cba => new CashBoxAccountModel {
                            Id = cba.Id,
                            CashBox = new CashBoxListModel {
                                Id = cba.Cashbox!.Id,
                                Name = cba.Cashbox.Name,
                                AccountId = at.AccountId
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
                at => at.Timestamp,
                orderDesc: true,
                materializeBeforeMapping: true,
                token: token
            );
    }

    public async Task<AccountTransactionCreateResponse> CreateAsync(
            AccountTransactionCreateRequest req,
            CancellationToken token = default
            ) {
        var reqTime = _timeProvider.GetUtcNow();

        AccountTransaction newTransaction;

        switch (req.Type) {
            case AccountTransactionType.StockTaking:
                var currentAmount = await _dbContext.AccountTransactions
                    .Where(at => at.AccountId == req.AccountId)
                    .SumAsync(at => at.Amount, token);
                newTransaction = new() {
                    Timestamp = reqTime,
                    AccountId = req.AccountId,
                    Amount = req.Amount - currentAmount,
                    Type = req.Type
                };
                break;
            case AccountTransactionType.Deposit:
                newTransaction = new() {
                    Timestamp = reqTime,
                    AccountId = req.AccountId,
                    Amount = req.Amount,
                    Type = req.Type
                };
                break;
            case AccountTransactionType.Withdrawal:
                newTransaction = new() {
                    Timestamp = reqTime,
                    AccountId = req.AccountId,
                    Amount = -req.Amount,
                    Type = req.Type
                };
                break;
            case AccountTransactionType.Transfer:
                newTransaction = new() {
                    Timestamp = reqTime,
                    AccountId = req.AccountId,
                    Amount = -req.Amount,
                    Type = req.Type
                };

                _dbContext.AccountTransactions.Add(new() {
                    Timestamp = reqTime,
                    AccountId = req.TargetAccountId!.Value,
                    Amount = req.Amount,
                    Type = req.Type
                });
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(req.Type));
        }

        _dbContext.AccountTransactions.Add(newTransaction);

        await _dbContext.SaveChangesAsync(token);
        var output = await _dbContext.AccountTransactions
            .Include(at => at.Account)
            .ThenInclude(a => (a as UserAccount)!.User)
            .Include(at => at.Account)
            .ThenInclude(a => (a as CashBoxAccount)!.Cashbox)
            .FirstAsync(at => at.Id == newTransaction.Id);

        return new AccountTransactionCreateResponse {
            Amount = output.Amount,
            SaleTransactionId = output.SaleTransactionId,
            Timestamp = output.Timestamp,
            Type = output.Type,
            Account = output.Account switch {
                CashBoxAccount cba => new CashBoxAccountModel {
                    Id = cba.Id,
                    CashBox = new CashBoxListModel {
                        Id = cba.Cashbox!.Id,
                        Name = cba.Cashbox.Name,
                        AccountId = output.AccountId
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
        };
    }
}
