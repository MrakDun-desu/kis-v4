using KisV4.BL.EF.Mapping;
using KisV4.Common.Authorization;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Enums;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class SaleTransactionService(
    KisDbContext dbContext,
    TimeProvider timeProvider,
    UserService userService,
    SaleTransactionRequestState state
) : IScopedService {
    private readonly KisDbContext _dbContext = dbContext;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly UserService _userService = userService;
    private readonly SaleTransactionRequestState _state = state;

    public async Task<SaleTransactionReadAllResponse> ReadAllAsync(
        SaleTransactionReadAllRequest req,
        int userId,
        CancellationToken token = default
    ) {
        var reqTime = _timeProvider.GetUtcNow();
        var query = _dbContext.SaleTransactions
            .IgnoreQueryFilters()
            .Include(st => st.StartedBy)
            .Include(st => st.CancelledBy)
            .Include(st => st.OpenedBy)
            .AsQueryable();

        var realFrom = req.From ?? DateTimeOffset.MinValue;
        if (req.From is { } from) {
            query = query.Where(st => st.StartedAt >= from);
        }

        if (req.To is { } to) {
            query = query.Where(st => st.StartedAt <= to);
        }

        var onlySelfCancellable = req.OnlySelfCancellable ?? false;
        if (onlySelfCancellable) {
            realFrom = reqTime - AuthorizationConstants.TransactionCancelTimeout;
            query = query.Where(st => st.OpenedById == userId)
                .Where(st => st.StartedAt >= realFrom);
        }

        return await query.PaginateAsync(
            req,
            (st) => new SaleTransactionListModel {
                Id = st.Id,
                Note = st.Note,
                StartedAt = st.StartedAt,
                CancelledAt = st.CancelledAt,
                StartedBy = st.StartedBy.ToModel()!,
                CancelledBy = st.CancelledBy.ToModel(),
                OpenedBy = st.OpenedBy.ToModel(),
            },
            (data, meta) => new SaleTransactionReadAllResponse {
                From = realFrom,
                To = req.To ?? reqTime,
                Data = data,
                Meta = meta
            },
            st => st.StartedAt,
            true,
            token
        );
    }

    public async Task<SaleTransactionDetailModel?> ReadAsync(
        int id,
        CancellationToken token = default
    ) {
        return await _dbContext.SaleTransactions
            .IgnoreQueryFilters()
            .Include(st => st.AccountTransactions)
            .Include(st => st.StoreTransactions)
            .ThenInclude(st => st.StartedBy)
            .Include(st => st.StoreTransactions)
            .ThenInclude(st => st.CancelledBy)
            .Include(st => st.SaleTransactionItems)
            .ThenInclude(sti => sti.SaleItem)
            .Include(st => st.SaleTransactionItems)
            .ThenInclude(sti => sti.Modifications)
            .ThenInclude(sti => sti.Modifier)
            .Include(st => st.StartedBy)
            .Include(st => st.CancelledBy)
            .Include(st => st.OpenedBy)
            .AsSplitQuery()
            .Select(st => new SaleTransactionDetailModel {
                Id = st.Id,
                Note = st.Note,
                StartedAt = st.StartedAt,
                CancelledAt = st.CancelledAt,
                StartedBy = st.StartedBy.ToModel()!,
                CancelledBy = st.CancelledBy.ToModel(),
                OpenedBy = st.OpenedBy.ToModel(),
                AccountTransactions = st.AccountTransactions.Select(at => new AccountTransactionModel {
                    Amount = at.Amount,
                    SaleTransactionId = at.SaleTransactionId,
                    Timestamp = st.ClosedAt ?? st.StartedAt,
                    Type = at.Type
                }),
                StoreTransactions = st.StoreTransactions.Select(st => st.ToModel()),
                SaleTransactionItems = st.SaleTransactionItems.Select(sti => sti.ToModel())
            })
            .FirstOrDefaultAsync(st => st.Id == id, token);
    }

    public async Task<SaleTransactionDetailModel> CreateAsync(
        SaleTransactionCreateRequest req,
        int userId,
        CancellationToken token = default
    ) {
        var reqTime = _timeProvider.GetUtcNow();
        await using var dbTransaction = await _dbContext.Database.BeginTransactionAsync(token);
        try {
            var customer = await _dbContext.Users.FindAsync(req.CustomerId, token);
            if (customer is null) {
                var newCustomerEntry = _dbContext.Users.Add(new User { Id = req.CustomerId });
                await _dbContext.SaveChangesAsync(token);
                customer = newCustomerEntry.Entity;
            }

            var (
                entity,
                composites,
                saleTransactionItems,
                storeTransaction
            ) = await CreateBaseTransactionAsync(
                req.Note,
                req.StoreId,
                userId,
                false,
                req.SaleTransactionItems,
                reqTime,
                token
            );

            var cashBox = await _dbContext.Cashboxes.FindAsync(req.CashBoxId, token);
            var accountTransactions = AddAccountTransactions(
                entity,
                saleTransactionItems,
                composites,
                customer,
                cashBox!,
                req.PaidAmount
            );
            _dbContext.AccountTransactions.AddRange(accountTransactions);
            await _dbContext.SaveChangesAsync(token);

            var user = await _userService.GetAsync(userId, token);

            await dbTransaction.CommitAsync(token);
            return new SaleTransactionDetailModel {
                Id = entity.Id,
                Note = entity.Note,
                StartedAt = entity.StartedAt,
                CancelledAt = null,
                StartedBy = user,
                CancelledBy = null,
                OpenedBy = null,
                StoreTransactions = [storeTransaction.ToModel()],
                AccountTransactions = accountTransactions.Select(at => new AccountTransactionModel {
                    Amount = at.Amount,
                    SaleTransactionId = entity.Id,
                    Timestamp = entity.StartedAt,
                    Type = at.Type
                }),
                SaleTransactionItems = saleTransactionItems.Select(sti => sti.ToModel(composites))
            };
        } catch {
            await dbTransaction.RollbackAsync(token);
            throw;
        }
    }

    public async Task<SaleTransactionCheckPriceResponse> CheckPriceAsync(
        SaleTransactionCheckPriceRequest req,
        CancellationToken token = default
    ) {
        var compositeIds = req.SaleTransactionItems.Aggregate(
            new HashSet<int>(req.SaleTransactionItems.Length),
            (acc, curr) => {
                acc.Add(curr.SaleItemId);
                foreach (var modifier in curr.Modifications) {
                    acc.Add(modifier.ModifierId);
                }
                return acc;
            });

        var compositePrices = await _dbContext.Composites
            .Where(c => compositeIds.Contains(c.Id))
            .Include(c => c.Compositions)
            .ThenInclude(c => c.StoreItem)
            .Select(c => new {
                c.Id,
                Composite = c,
                Price = Math.Round(c.Compositions.Sum(c => c.Amount * c.StoreItem!.CurrentCost) *
                    (c.MarginPercent * 0.01m + 1m) + c.MarginStatic, 2),
                c.Name
            })
            .ToDictionaryAsync(c => c.Id, c => (c.Composite.Name, c.Price), token);

        return new SaleTransactionCheckPriceResponse {
            SaleTransactionItems = req.SaleTransactionItems.Select((sti, i) => new SaleTransactionItemModel {
                Amount = sti.Amount,
                LineNumber = i + 1,
                BasePrice = compositePrices[sti.SaleItemId].Price,
                SaleItemName = compositePrices[sti.SaleItemId].Name,
                Modifications = sti.Modifications.Select(m => new ModificationModel {
                    Amount = m.Amount,
                    ModifierName = compositePrices[m.ModifierId].Name,
                    PriceChange = compositePrices[m.ModifierId].Price,
                })
            })
        };
    }

    public async Task<SaleTransactionDetailModel> OpenAsync(
        SaleTransactionOpenRequest req,
        int userId,
        CancellationToken token = default
    ) {
        var reqTime = _timeProvider.GetUtcNow();
        await using var dbTransaction = await _dbContext.Database.BeginTransactionAsync(token);
        try {
            var (
                entity,
                composites,
                saleTransactionItems,
                storeTransaction
            ) = await CreateBaseTransactionAsync(
                req.Note,
                req.StoreId,
                userId,
                true,
                req.SaleTransactionItems,
                reqTime,
                token
            );

            var user = await _userService.GetAsync(userId);

            await dbTransaction.CommitAsync(token);

            return new SaleTransactionDetailModel {
                Id = entity.Id,
                Note = entity.Note,
                StartedAt = entity.StartedAt,
                CancelledAt = null,
                StartedBy = user,
                CancelledBy = null,
                OpenedBy = null,
                StoreTransactions = [storeTransaction.ToModel()],
                AccountTransactions = [],
                SaleTransactionItems = saleTransactionItems.Select(sti => sti.ToModel(composites))
            };
        } catch {
            await dbTransaction.RollbackAsync(token);
            throw;
        }
    }

    public async Task<SaleTransactionDetailModel?> UpdateAsync(
        SaleTransactionUpdateRequest req,
        int userId,
        CancellationToken token = default
    ) {
        var reqTime = _timeProvider.GetUtcNow();
        var entity = await _dbContext.SaleTransactions
            .IgnoreQueryFilters()
            .Include(st => st.AccountTransactions)
            .Include(st => st.StoreTransactions)
            .ThenInclude(st => st.StartedBy)
            .Include(st => st.StoreTransactions)
            .ThenInclude(st => st.CancelledBy)
            .Include(st => st.SaleTransactionItems)
            .ThenInclude(sti => sti.SaleItem)
            .Include(st => st.SaleTransactionItems)
            .ThenInclude(sti => sti.Modifications)
            .ThenInclude(sti => sti.Modifier)
            .Include(st => st.StartedBy)
            .Include(st => st.CancelledBy)
            .Include(st => st.OpenedBy)
            .AsSplitQuery()
            .FirstOrDefaultAsync(st => st.Id == req.Id, token);

        if (entity is null) {
            return null;
        }
        await using var dbTransaction = await _dbContext.Database.BeginTransactionAsync(token);
        try {
            var composites = await TryGetCompositesAsync(req.Model.SaleTransactionItems, _dbContext, _state, token);
            var newSaleTransactionItems = AddSaleTransactionItems(
                entity,
                req.Model.SaleTransactionItems,
                composites!,
                entity.SaleTransactionItems.Max(sti => sti.LineNumber)
            );
            _dbContext.SaleTransactionItems.AddRange(newSaleTransactionItems);

            var storeTransaction = AddStoreTransaction(
                entity,
                req.Model.SaleTransactionItems,
                composites!,
                req.Model.StoreId,
                reqTime,
                userId
            );

            entity.Note = req.Model.Note;
            _dbContext.SaleTransactions.Update(entity);
            await _dbContext.SaveChangesAsync(token);

            await StoreTransactionService.CreateInternalAsync(storeTransaction, userId, _dbContext, reqTime, token);

            await dbTransaction.CommitAsync(token);

            return new SaleTransactionDetailModel {
                Id = entity.Id,
                Note = entity.Note,
                StartedAt = entity.StartedAt,
                CancelledAt = entity.CancelledAt,
                StartedBy = entity.StartedBy.ToModel()!,
                CancelledBy = entity.CancelledBy.ToModel(),
                OpenedBy = entity.StartedBy.ToModel(),
                StoreTransactions = entity.StoreTransactions.Select(st => st.ToModel()),
                AccountTransactions = entity.AccountTransactions.Select(at => new AccountTransactionModel {
                    Amount = at.Amount,
                    SaleTransactionId = entity.Id,
                    Timestamp = entity.StartedAt,
                    Type = at.Type
                }),
                SaleTransactionItems = entity.SaleTransactionItems.Select(sti => sti.ToModel())
            };
        } catch {
            await dbTransaction.RollbackAsync(token);
            throw;
        }
    }

    public async Task<SaleTransactionDetailModel?> CloseAsync(
        SaleTransactionCloseRequest req,
        int userId,
        CancellationToken token = default
    ) {
        var reqTime = _timeProvider.GetUtcNow();
        var entity = await _dbContext.SaleTransactions
            .IgnoreQueryFilters()
            .Include(st => st.AccountTransactions)
            .Include(st => st.StoreTransactions)
            .ThenInclude(st => st.StartedBy)
            .Include(st => st.StoreTransactions)
            .ThenInclude(st => st.CancelledBy)
            .Include(st => st.SaleTransactionItems)
            .ThenInclude(sti => sti.SaleItem)
            .Include(st => st.StartedBy)
            .Include(st => st.CancelledBy)
            .Include(st => st.OpenedBy)
            .AsSplitQuery()
            .FirstOrDefaultAsync(st => st.Id == req.Id, token);

        _state.SaleTransactionItems ??= await _dbContext.SaleTransactionItems
                    .Where(sti => sti.SaleTransactionId == req.Id)
                    .Include(sti => sti.Modifications)
                    .ThenInclude(m => m.Modifier)
                    .ToArrayAsync(token);

        var saleTransactionItems = _state.SaleTransactionItems;

        if (entity is null) {
            return null;
        }
        await using var dbTransaction = await _dbContext.Database.BeginTransactionAsync(token);
        try {
            var customer = await _dbContext.Users.FindAsync(req.Model.CustomerId, token);
            if (customer is null) {
                var newCustomerEntry = _dbContext.Users.Add(new User { Id = req.Model.CustomerId });
                await _dbContext.SaveChangesAsync(token);
                customer = newCustomerEntry.Entity;
            }

            var composites = await TryGetCompositesAsync(
                saleTransactionItems.Select(sti => new SaleTransactionItemCreateRequest {
                    Amount = sti.Amount,
                    SaleItemId = sti.SaleItemId,
                    Modifications = sti.Modifications.Select(m => new ModificationCreateRequest {
                        Amount = m.Amount,
                        ModifierId = m.ModifierId
                    }).ToArray()
                }).ToArray(),
                _dbContext,
                _state,
                token
            );

            var cashBox = await _dbContext.Cashboxes.FindAsync(req.Model.CashBoxId, token);
            var accountTransactions = AddAccountTransactions(
                entity,
                saleTransactionItems.ToArray(),
                composites!,
                customer,
                cashBox!,
                req.Model.PaidAmount
            );
            _dbContext.AccountTransactions.AddRange(accountTransactions);

            entity.Note = req.Model.Note;
            entity.ClosedAt = reqTime;
            _dbContext.SaleTransactions.Update(entity);
            await _dbContext.SaveChangesAsync(token);
            await dbTransaction.CommitAsync(token);

            return new SaleTransactionDetailModel {
                Id = entity.Id,
                Note = entity.Note,
                StartedAt = entity.StartedAt,
                CancelledAt = entity.CancelledAt,
                StartedBy = entity.StartedBy.ToModel()!,
                CancelledBy = entity.CancelledBy.ToModel(),
                OpenedBy = entity.StartedBy.ToModel(),
                StoreTransactions = entity.StoreTransactions.Select(st => st.ToModel()),
                AccountTransactions = entity.AccountTransactions.Select(at => new AccountTransactionModel {
                    Amount = at.Amount,
                    SaleTransactionId = entity.Id,
                    Timestamp = entity.StartedAt,
                    Type = at.Type
                }),
                SaleTransactionItems = saleTransactionItems.Select(sti => sti.ToModel())
            };
        } catch {
            await dbTransaction.RollbackAsync(token);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(
        SaleTransactionDeleteRequest req,
        int userId,
        CancellationToken token = default
    ) {
        var reqTime = _timeProvider.GetUtcNow();
        await using var dbTransaction = await _dbContext.Database.BeginTransactionAsync(token);
        try {
            var updatedCount = await _dbContext.SaleTransactions
                .Where(si => si.Id == req.Id)
                .ExecuteUpdateAsync(props => {
                    props.SetProperty(si => si.CancelledAt, reqTime);
                    props.SetProperty(si => si.CancelledById, userId);
                });

            if (updatedCount == 0) {
                return false;
            }

            await _dbContext.SaleTransactionItems
                .IgnoreQueryFilters()
                .Where(sti => sti.SaleTransactionId == req.Id)
                .ExecuteUpdateAsync(props => {
                    props.SetProperty(sti => sti.Cancelled, true);
                });
            await _dbContext.StoreTransactions
                .IgnoreQueryFilters()
                .Where(st => st.SaleTransactionId == req.Id)
                .ExecuteUpdateAsync(props => {
                    props.SetProperty(si => si.CancelledAt, reqTime);
                    props.SetProperty(si => si.CancelledById, userId);
                });
            await _dbContext.StoreTransactionItems
                .IgnoreQueryFilters()
                .Include(sti => sti.StoreTransaction)
                .Where(sti => sti.StoreTransaction!.SaleTransactionId == req.Id)
                .ExecuteUpdateAsync(props => {
                    props.SetProperty(sti => sti.Cancelled, true);
                });
            await _dbContext.AccountTransactions
                .IgnoreQueryFilters()
                .Where(at => at.SaleTransactionId == req.Id)
                .ExecuteUpdateAsync(props => {
                    props.SetProperty(at => at.Cancelled, true);
                });

            await dbTransaction.CommitAsync(token);
            return true;
        } catch {
            await dbTransaction.RollbackAsync(token);
            throw;
        }
    }

    // With the base transaction, we'll need the transaction itself, the customer,
    // transaction items, and store transaction to be used further
    private async Task<(
        SaleTransaction,
        Dictionary<int, (Composite Item, decimal Price)>,
        SaleTransactionItem[],
        StoreTransaction
    )> CreateBaseTransactionAsync(
        string? note,
        int storeId,
        int userId,
        bool open,
        SaleTransactionItemCreateRequest[] itemsToCreate,
        DateTimeOffset reqTime,
        CancellationToken token
    ) {
        var composites = await TryGetCompositesAsync(itemsToCreate, _dbContext, _state, token);
        var entity = new SaleTransaction {
            Note = note,
            StartedAt = reqTime,
            StartedById = userId,
            OpenedById = open ? userId : null,
            Reason = TransactionReason.Sale,
        };
        var saleTransactionItems = AddSaleTransactionItems(entity, itemsToCreate, composites!);
        _dbContext.SaleTransactionItems.AddRange(saleTransactionItems);
        _dbContext.SaleTransactions.Add(entity);
        await _dbContext.SaveChangesAsync(token);

        var storeTransaction = AddStoreTransaction(
            entity,
            itemsToCreate,
            composites!,
            storeId,
            reqTime,
            userId
        );
        await StoreTransactionService.CreateInternalAsync(storeTransaction, userId, _dbContext, reqTime, token);
        return (entity, composites!, saleTransactionItems, storeTransaction);
    }

    /// <summary>
    /// Returns composites relevant to this transaction or transaction update.
    /// If one or more of the specified composites doesn't exist, returns null.
    /// </summary>
    internal static async Task<Dictionary<int, (Composite Item, decimal Price)>?> TryGetCompositesAsync(
        SaleTransactionItemCreateRequest[] saleTransactionItems,
        KisDbContext dbContext,
        SaleTransactionRequestState state,
        CancellationToken token = default
    ) {
        if (state.Composites is { } composites) {
            return composites;
        }
        var compositeIds = saleTransactionItems.Aggregate(
            new HashSet<int>(saleTransactionItems.Length),
            (acc, curr) => {
                acc.Add(curr.SaleItemId);
                foreach (var modifier in curr.Modifications) {
                    acc.Add(modifier.ModifierId);
                }
                return acc;
            });

        composites = await dbContext.Composites
            .Where(c => compositeIds.Contains(c.Id))
            .Include(c => c.Compositions)
            .ThenInclude(c => c.StoreItem)
            .Select(c => new {
                c.Id,
                Composite = c,
                Price = Math.Round(c.Compositions.Sum(c => c.Amount * c.StoreItem!.CurrentCost) *
                    (c.MarginPercent * 0.01m + 1m) + c.MarginStatic, 2),
                c.Name
            })
            .ToDictionaryAsync(c => c.Id, c => (Item: c.Composite, c.Price), token);

        if (composites.Count != compositeIds.Count) {
            return null;
        }

        state.Composites = composites;
        return composites;
    }

    private SaleTransactionItem[] AddSaleTransactionItems(
        SaleTransaction transaction,
        SaleTransactionItemCreateRequest[] itemsToCreate,
        Dictionary<int, (Composite Item, decimal Price)> composites,
        int transactionMaxLineNumber = 0
    ) {
        var transactionItems = itemsToCreate.Select((sti, i) => new SaleTransactionItem {
            LineNumber = i + transactionMaxLineNumber + 1,
            Amount = sti.Amount,
            BasePrice = composites[sti.SaleItemId].Price,
            SaleItemId = sti.SaleItemId,
            SaleTransaction = transaction,
            Modifications = sti.Modifications.Select(m => new Modification {
                Amount = m.Amount,
                PriceChange = composites[m.ModifierId].Price,
                ModifierId = m.ModifierId,
            }).ToArray()
        }).ToArray();

        return transactionItems;
    }

    private AccountTransaction[] AddAccountTransactions(
        SaleTransaction saleTransaction,
        SaleTransactionItem[] saleTransactionItems,
        Dictionary<int, (Composite Item, decimal Price)> composites,
        User customer,
        Cashbox cashBox,
        decimal paidAmount
    ) {
        var totalTransactionPrice = saleTransactionItems.Aggregate(0m, (acc, curr) =>
            acc + (curr.BasePrice + curr.Modifications.Sum(m => m.PriceChange * m.Amount)) * curr.Amount
        );

        var totalTransactionPrestige = saleTransactionItems.Aggregate(0m, (acc, curr) =>
            acc +
            // sale item prestige
            (composites[curr.SaleItemId].Item.PrestigeAmount +
            // modification prestige
            curr.Modifications.Sum(m => composites[m.ModifierId].Item.PrestigeAmount * m.Amount)) *
            curr.Amount
        ) + (paidAmount - totalTransactionPrice);

        AccountTransaction[] accountTransactions = [
            // Add the total prestige to the customer's account
            new AccountTransaction {
                Amount = totalTransactionPrestige,
                AccountId = customer.PrestigeAccountId,
                Type = AccountTransactionType.Prestige,
                SaleTransaction = saleTransaction
            },
            // Add amount that was paid for the actual items to the sales account of the cashbox
            new AccountTransaction {
                Amount = totalTransactionPrice,
                AccountId = cashBox.SalesAccountId,
                Type = AccountTransactionType.SaleMoney,
                SaleTransaction = saleTransaction
            },
            // Add the actual paid amount minus total price to the donations account of the cashbox
            new AccountTransaction {
                Amount = paidAmount - totalTransactionPrice,
                AccountId = cashBox.DonationsAccountId,
                Type = AccountTransactionType.DonationMoney,
                SaleTransaction = saleTransaction
            }
        ];

        return accountTransactions;
    }

    public static Dictionary<int, StoreTransactionItem> GetStoreTransactionItems(
        Dictionary<int, (Composite Item, decimal Price)> composites,
        SaleTransactionItemCreateRequest[] saleTransactionItems,
        int storeId
    ) {
        var storeTransactionItems = new Dictionary<int, StoreTransactionItem>();
        foreach (var saleTransactionItem in saleTransactionItems) {
            var saleItem = composites[saleTransactionItem.SaleItemId].Item;
            var saleItemAmount = saleTransactionItem.Amount;
            foreach (var composition in saleItem.Compositions) {
                storeTransactionItems.TryAdd(composition.StoreItemId, new StoreTransactionItem {
                    Cost = 0,
                    ItemAmount = 0,
                    StoreId = storeId,
                    StoreItemId = composition.StoreItemId
                });
                storeTransactionItems[composition.StoreItemId].ItemAmount -=
                    composition.Amount * saleItemAmount;
            }
            foreach (var modification in saleTransactionItem.Modifications) {
                var modifier = composites[modification.ModifierId].Item;
                var modificationAmount = modification.Amount;
                foreach (var composition in modifier.Compositions) {
                    storeTransactionItems.TryAdd(composition.StoreItemId, new StoreTransactionItem {
                        Cost = 0,
                        ItemAmount = 0,
                        StoreId = storeId,
                        StoreItemId = composition.StoreItemId
                    });
                    storeTransactionItems[composition.StoreItemId].ItemAmount -=
                        composition.Amount * saleItemAmount * modificationAmount;
                }
            }
        }

        return storeTransactionItems;
    }

    private StoreTransaction AddStoreTransaction(
        SaleTransaction saleTransaction,
        SaleTransactionItemCreateRequest[] saleTransactionItems,
        Dictionary<int, (Composite Item, decimal Price)> composites,
        int storeId,
        DateTimeOffset reqTime,
        int userId
    ) {
        var storeTransactionItems = GetStoreTransactionItems(composites, saleTransactionItems, storeId);

        var storeTransaction = new StoreTransaction {
            Note = null,
            Reason = TransactionReason.Sale,
            StartedAt = reqTime,
            StartedById = userId,
            StoreTransactionItems = storeTransactionItems.Values
                .Where(sti => sti.ItemAmount != 0).ToArray(),
            SaleTransaction = saleTransaction
        };

        return storeTransaction;
    }
}
