using KisV4.BL.Common.Services;
using KisV4.Common;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Enums;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class StoreTransactionService(
    KisDbContext dbContext,
    IUserService userService,
    TimeProvider timeProvider) : IStoreTransactionService, IScopedService {
    public OneOf<Page<StoreTransactionListModel>, Dictionary<string, string[]>> ReadAll(
        int? page,
        int? pageSize,
        DateTimeOffset? startDate,
        DateTimeOffset? endDate,
        bool? cancelled) {
        var query = dbContext.StoreTransactions
            .Include(sti => sti.SaleTransaction)
            .Include(sti => sti.ResponsibleUser)
            .OrderByDescending(sti => sti.Timestamp)
            .AsQueryable();

        if (startDate.HasValue) {
            query = query.Where(sti => sti.Timestamp > startDate.Value);
        }

        if (endDate.HasValue) {
            query = query.Where(sti => sti.Timestamp < endDate.Value);
        }

        if (cancelled.HasValue) {
            query = query.Where(sti => sti.Cancelled == cancelled.Value);
        }

        return query.Page(page ?? 1, pageSize ?? Constants.DefaultPageSize, Mapper.ToModels);
    }

    public IEnumerable<StoreTransactionListModel> ReadSelfCancellable(string userName) {
        var userId = userService.CreateOrGetId(userName);
        var currentTime = timeProvider.GetUtcNow();
        var oldestCancellableTime = currentTime - Constants.SelfCancellableTime;
        return dbContext.StoreTransactions
            .Include(sti => sti.SaleTransaction)
            .Include(sti => sti.ResponsibleUser)
            .OrderByDescending(sti => sti.Timestamp)
            .Where(sti => sti.ResponsibleUserId == userId)
            .Where(sti => sti.Timestamp > oldestCancellableTime)
            .Where(sti => !sti.Cancelled)
            .ToList()
            .ToModels();
    }

    public OneOf<StoreTransactionDetailModel, NotFound> Read(int id) {
        var entity = dbContext.StoreTransactions
            .FirstOrDefault(st => st.Id == id);
        return entity is null ? new NotFound() : entity.ToModel();
    }

    public OneOf<StoreTransactionDetailModel, Dictionary<string, string[]>> Create(
        StoreTransactionCreateModel createModel,
        string userName) {
        ValidateCreateRequest(createModel, out var errors);
        if (errors.Count > 0) {
            return errors;
        }

        var newTransaction = new StoreTransactionEntity {
            Timestamp = timeProvider.GetUtcNow(),
            ResponsibleUserId = userService.CreateOrGetId(userName),
            TransactionReason = createModel.TransactionReason
        };
        foreach (var item in createModel.StoreTransactionItems) {
            switch (createModel.TransactionReason) {
                case TransactionReason.MovingStores:
                    newTransaction.StoreTransactionItems.Add(new StoreTransactionItemEntity {
                        ItemAmount = -item.Amount,
                        StoreId = createModel.StoreId,
                        StoreItemId = item.StoreItemId
                    });
                    newTransaction.StoreTransactionItems.Add(new StoreTransactionItemEntity {
                        ItemAmount = item.Amount,
                        StoreId = createModel.DestinationStoreId!.Value,
                        StoreItemId = item.StoreItemId
                    });
                    break;
                case TransactionReason.AddingToStore:
                case TransactionReason.Sale:
                case TransactionReason.WriteOff:
                    newTransaction.StoreTransactionItems.Add(new StoreTransactionItemEntity {
                        ItemAmount = item.Amount,
                        StoreId = createModel.StoreId,
                        StoreItemId = item.StoreItemId
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(createModel.TransactionReason));
            }
        }

        dbContext.StoreTransactions.Add(newTransaction);
        dbContext.SaveChanges();
        dbContext.ChangeTracker.Clear();

        return Read(newTransaction.Id).AsT0;
    }

    public OneOf<StoreTransactionDetailModel, NotFound> Delete(int id) {
        var entity = dbContext.StoreTransactions
            .Include(sti => sti.StoreTransactionItems)
            .FirstOrDefault(sti => sti.Id == id);

        if (entity is null) {
            return new NotFound();
        }

        entity.Cancelled = true;
        foreach (var item in entity.StoreTransactionItems) {
            item.Cancelled = true;
        }

        dbContext.StoreTransactions.Update(entity);
        dbContext.SaveChanges();

        return Read(id).AsT0;
    }

    private void ValidateCreateRequest(
        StoreTransactionCreateModel createModel,
        out Dictionary<string, string[]> errors) {
        errors = [];
        if (!dbContext.Stores.Any(st => st.Id == createModel.StoreId)) {
            errors.AddItemOrCreate(
                nameof(createModel.StoreId),
                $"Store with id {createModel.StoreId} doesn't exist"
            );
        } else if (dbContext.Containers.Any(st => st.Id == createModel.StoreId)) {
            errors.AddItemOrCreate(
                nameof(createModel.StoreId),
                $"Store with id {createModel.StoreId} is a container. " +
                $"It is not possible to manually create store transactions " +
                $"for containers"
            );
        }

        if (createModel.TransactionReason == TransactionReason.MovingStores) {
            if (!createModel.DestinationStoreId.HasValue) {
                errors.AddItemOrCreate(
                    nameof(createModel.DestinationStoreId),
                    "Destination store has to be specified when moving stores"
                );
            } else if (!dbContext.Stores.Any(st => st.Id == createModel.DestinationStoreId.Value)) {
                errors.AddItemOrCreate(
                    nameof(createModel.DestinationStoreId),
                    $"Store with id {createModel.DestinationStoreId.Value} doesn't exist"
                );
            } else if (dbContext.Containers.Any(st => st.Id == createModel.DestinationStoreId.Value)) {
                errors.AddItemOrCreate(
                    nameof(createModel.StoreId),
                    $"Store with id {createModel.StoreId} is a container. " +
                    $"It is not possible to manually create store transactions " +
                    $"for containers"
                );
            }
        }

        var storeItemIds = createModel.StoreTransactionItems
            .Select(item => item.StoreItemId)
            .Distinct()
            .ToList();
        var realStoreItems = dbContext.StoreItems
            .Where(si => storeItemIds.Contains(si.Id))
            .ToList();
        if (realStoreItems.Count != storeItemIds.Count) {
            errors.AddItemOrCreate(
                nameof(createModel.StoreTransactionItems),
                "Some of the specified store items do not exist"
            );
        }

        if (realStoreItems.Any(si => si.IsContainerItem)) {
            errors.AddItemOrCreate(
                nameof(createModel.StoreTransactionItems),
                "Some of the specified store items are container items. " +
                "It's not possible to manually create store transactions " +
                "for container items"
            );
        }
    }
}
