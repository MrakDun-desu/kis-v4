using KisV4.BL.EF.Mapping;
using KisV4.BL.EF.Validation;
using KisV4.Common.Authorization;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Enums;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class StoreTransactionService(
        KisDbContext dbContext,
        TimeProvider timeProvider,
        UserService userService
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly UserService _userService = userService;

    public async Task<StoreTransactionReadAllResponse> ReadAllAsync(
        StoreTransactionReadAllRequest req,
        int userId,
        CancellationToken token = default
    ) {
        var reqTime = _timeProvider.GetUtcNow();
        var query = _dbContext.StoreTransactions
            .IgnoreQueryFilters()
            .Include(st => st.StartedBy)
            .Include(st => st.CancelledBy)
            .AsQueryable();

        if (req.From is { } from) {
            query = query.Where(st => st.StartedAt > from);
        }

        if (req.To is { } to) {
            query = query.Where(st => st.StartedAt < to);
        }

        var onlySelfCancellable = req.OnlySelfCancellable ?? false;
        if (onlySelfCancellable) {
            query = query.Where(st => st.StartedById == userId)
                .Where(st => st.StartedAt + AuthorizationConstants.TransactionCancelTimeout >= reqTime);
        }

        return await query.PaginateAsync(
            req,
            entity => new StoreTransactionListModel {
                Id = entity.Id,
                Note = entity.Note,
                StartedAt = entity.StartedAt,
                CancelledAt = entity.CancelledAt,
                StartedBy = entity.StartedBy.ToModel()!,
                CancelledBy = entity.CancelledBy.ToModel(),
                Reason = entity.Reason,
                SaleTransactionId = entity.SaleTransactionId
            },
            (data, meta) => new StoreTransactionReadAllResponse { Data = data, Meta = meta },
            entity => entity.StartedAt,
            true,
            token
        );
    }

    public async Task<StoreTransactionReadResponse?> ReadAsync(
        int id,
        CancellationToken token = default
    ) {
        return await _dbContext.StoreTransactions
            .IgnoreQueryFilters()
            .Include(st => st.StartedBy)
            .Include(st => st.CancelledBy)
            .Include(st => st.StoreTransactionItems)
            .ThenInclude(st => st.StoreItem)
            .Include(st => st.StoreTransactionItems)
            .ThenInclude(st => st.Store)
            .Select(st => new StoreTransactionReadResponse {
                Id = st.Id,
                Note = st.Note,
                StartedAt = st.StartedAt,
                CancelledAt = st.CancelledAt,
                StartedBy = st.StartedBy.ToModel()!,
                CancelledBy = st.CancelledBy.ToModel(),
                Reason = st.Reason,
                SaleTransactionId = st.SaleTransactionId,
                StoreTransactionItems = st.StoreTransactionItems
                    .Select(sti => sti.ToModel())
            })
            .FirstOrDefaultAsync(st => st.Id == id, token);
    }

    public async Task<StoreTransactionCreateResponse> CreateAsync(
            StoreTransactionCreateRequest req,
            int userId,
            CancellationToken token = default
            ) {
        var reqTime = _timeProvider.GetUtcNow();
        var user = await _userService.GetAsync(userId, token);

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(token);

        try {
            var entity = await CreateInternalAsync(req, userId, reqTime, _dbContext, token: token);

            var output = new StoreTransactionCreateResponse {
                Id = entity.Id,
                Note = entity.Note,
                Reason = entity.Reason,
                StartedAt = entity.StartedAt,
                StoreTransactionItems = _dbContext.StoreTransactionItems
                    .Where(sti => sti.StoreTransactionId == entity.Id)
                    .Include(sti => sti.StoreItem)
                    .Include(sti => sti.Store)
                    .Select(StoreTransactionItemMapping.ToModel),
                CancelledAt = entity.CancelledAt,
                CancelledBy = entity.CancelledBy.ToModel(),
                SaleTransactionId = entity.SaleTransactionId,
                StartedBy = user
            };

            await transaction.CommitAsync(token);
            return output;
        } catch {
            await transaction.RollbackAsync(token);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(
        StoreTransactionDeleteRequest req,
        int userId,
        CancellationToken token = default
    ) {
        var reqTime = _timeProvider.GetUtcNow();
        await using var dbTransaction = await _dbContext.Database.BeginTransactionAsync(token);
        try {
            var user = _userService.GetAsync(userId);
            var deletedCount = await _dbContext.StoreTransactions
                .Where(st => st.Id == req.Id)
                .ExecuteUpdateAsync(props => {
                    props.SetProperty(st => st.CancelledById, userId);
                    props.SetProperty(st => st.CancelledAt, reqTime);
                }, token);

            if (deletedCount == 0) {
                return false;
            }

            await _dbContext.StoreTransactionItems
                .IgnoreQueryFilters()
                .Where(sti => sti.StoreTransactionId == req.Id)
                .ExecuteUpdateAsync(props => {
                    props.SetProperty(sti => sti.Cancelled, true);
                }, token);

            await UpdateItemAmountsAsync(req.Id, true, _dbContext, token);

            var updateCosts = req.UpdateCosts ?? false;
            if (updateCosts) {
                await UpdateCostsAsync(req.Id, user.Id, reqTime, _dbContext, token);
            }

            await dbTransaction.CommitAsync(token);
            return true;
        } catch {
            await dbTransaction.RollbackAsync(token);
            throw;
        }
    }

    /// <summary>
    /// Creates a StoreTransaction entity with the specified parameters. Must always be wrapped
    /// inside of a database transaction to keep the database state consistent.
    /// </summary>
    internal static async Task<StoreTransaction> CreateInternalAsync(
            StoreTransactionCreateRequest req,
            int userId,
            DateTimeOffset reqTime,
            KisDbContext dbContext,
            SaleTransaction? saleTransaction = null,
            CancellationToken token = default
            ) {
        var transactionItemsCount = req.StoreTransactionItems.Length;
        if (req.SourceStoreId.HasValue) {
            transactionItemsCount *= 2;
        }
        var storeTransactionItems = req.StoreTransactionItems.Aggregate(
            new List<StoreTransactionItem>(transactionItemsCount), (output, sti) => {
                output.Add(new StoreTransactionItem {
                    StoreId = req.StoreId,
                    Cost = sti.Cost,
                    ItemAmount = sti.Amount,
                    StoreItemId = sti.StoreItemId
                });

                if (req.SourceStoreId is { } sourceStoreId) {
                    output.Add(new StoreTransactionItem {
                        StoreId = sourceStoreId,
                        Cost = sti.Cost,
                        ItemAmount = -sti.Amount,
                        StoreItemId = sti.StoreItemId
                    });
                }
                return output;
            }
        );

        var entity = new StoreTransaction {
            Note = req.Note,
            Reason = req.Reason,
            SaleTransaction = saleTransaction,
            StartedAt = reqTime,
            StartedById = userId,
            StoreTransactionItems = storeTransactionItems
        };

        return await CreateInternalAsync(entity, userId, dbContext, reqTime, token, req.UpdateCosts);
    }

    internal static async Task<StoreTransaction> CreateInternalAsync(
        StoreTransaction storeTransaction,
        int userId,
        KisDbContext dbContext,
        DateTimeOffset reqTime,
        CancellationToken token = default,
        bool updateCosts = false
    ) {
        dbContext.StoreTransactions.Add(storeTransaction);
        await dbContext.SaveChangesAsync(token);

        await UpdateItemAmountsAsync(storeTransaction.Id, false, dbContext, token);

        if (updateCosts) {
            await UpdateCostsAsync(storeTransaction.Id, userId, reqTime, dbContext, token);
        }

        return storeTransaction;
    }

    private static async Task UpdateItemAmountsAsync(
            int transactionId,
            bool cancelledTransaction,
            KisDbContext dbContext,
            CancellationToken token = default
        ) {
        // update all the store item amounts in one database call
        // honestly not sure if this is faster than just updating the items with normal Update calls,
        // could be interesting to optimize
        // It's a shame that ExecuteUpdate doesn't work with in-memory collections for lists of
        // updates, so it's necessary to aggregate everything here (or add a temp table, or use raw SQL)
        var multiplier = cancelledTransaction ? -1m : 1m;
        await dbContext.StoreItemAmounts
            .Where(sia => dbContext.StoreTransactionItems
                    .Any(c => c.StoreId == sia.StoreId
                        && c.StoreItemId == sia.StoreItemId
                        && c.StoreTransactionId == transactionId)
                    )
            .ExecuteUpdateAsync(
                props => props.SetProperty(
                    x => x.Amount,
                    x => x.Amount + dbContext.StoreTransactionItems
                    .IgnoreQueryFilters()
                    .Where(sti => sti.StoreId == x.StoreId
                        && sti.StoreItemId == x.StoreItemId
                        && sti.StoreTransactionId == transactionId
                    )
                    .Sum(sti => sti.ItemAmount) * multiplier
                ), token
            );

        // update all composite amounts in one database call
        await dbContext.CompositeAmounts
            .Where(ca => dbContext.Compositions
                    .Any(c => c.CompositeId == ca.CompositeId
                        && dbContext.StoreTransactionItems
                        .Any(sti => sti.StoreItemId == c.StoreItemId
                            && sti.StoreId == ca.StoreId
                            && sti.StoreTransactionId == transactionId)
                        )
                    )
            .ExecuteUpdateAsync(
                    props => props.SetProperty(
                        x => x.Amount,
                        x => dbContext.Compositions
                            .Where(c => c.CompositeId == x.CompositeId)
                            .Select(c => (int)Math.Floor(dbContext.StoreItemAmounts
                                .Where(sia => sia.StoreItemId == c.StoreItemId
                                    && sia.StoreId == x.StoreId)
                                .Select(sia => sia.Amount)
                                .Sum() / c.Amount)
                            )
                            .Min()
                        )
                    );
    }

    private static async Task UpdateCostsAsync(
        int transactionId,
        int userId,
        DateTimeOffset reqTime,
        KisDbContext dbContext,
        CancellationToken token = default
    ) {
        var storeItemsToUpdate = await dbContext.StoreTransactionItems
            .IgnoreQueryFilters()
            .Where(sti => sti.StoreTransactionId == transactionId)
            .Select(sti => sti.StoreItemId)
            .Distinct()
            .ToArrayAsync(token);

        var newCosts = await dbContext.StoreTransactionItems
            .Where(sti => storeItemsToUpdate.Contains(sti.StoreItemId))
            .Include(sti => sti.StoreTransaction)
            .Where(sti => sti.StoreTransaction!.Reason == TransactionReason.AddingToStore)
            .GroupBy(sti => sti.StoreItemId)
            .Select(g => new {
                StoreItemId = g.Key,
                NewCost = g.Sum(sti => sti.Cost) / g.Sum(sti => sti.ItemAmount)
            })
            .ToArrayAsync(token);

        dbContext.Costs.AddRange(newCosts.Select(c => new Cost {
            Amount = c.NewCost,
            Description = "Automatically recalculated",
            Timestamp = reqTime,
            StoreItemId = c.StoreItemId,
            UserId = userId
        }));
        await dbContext.SaveChangesAsync(token);

        await dbContext.StoreItems
            .Where(si => storeItemsToUpdate.Contains(si.Id))
            .ExecuteUpdateAsync(props => props.SetProperty(
                si => si.CurrentCost,
                si => si.Costs.OrderByDescending(c => c.Timestamp)
                    .Select(c => c.Amount)
                    .First()
            ), token);
    }
}
