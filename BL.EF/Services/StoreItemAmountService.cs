using KisV4.BL.Common.Services;
using KisV4.BL.EF.Helpers;
using KisV4.Common;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class StoreItemAmountService(KisDbContext dbContext)
    : IStoreItemAmountService, IScopedService {
    public OneOf<Page<StoreItemAmountListModel>, Dictionary<string, string[]>> ReadAll(
        int storeId,
        int? page,
        int? pageSize,
        int? categoryId
    ) {
        var realPage = page ?? 1;
        var realPageSize = Math.Min(pageSize ?? Constants.DefaultPageSize, Constants.MaxPageSize);
        var errors = new Dictionary<string, string[]>();
        if (realPage < 1) {
            errors.AddItemOrCreate(
                nameof(page), $"Page is required to be higher than 0. Received value: {realPage}"
            );
        }

        if (categoryId is { } categoryIdReal) {
            if (dbContext.ProductCategories.Find(categoryIdReal) is null) {
                errors.AddItemOrCreate(
                    nameof(categoryId),
                    $"Category {categoryId} doesn't exist"
                );
            }
        }

        if (errors.Count > 0) {
            return errors;
        }

        var storeItemsQuery = dbContext.StoreItems.AsQueryable();

        if (categoryId is { }) {
            storeItemsQuery = storeItemsQuery
                .Include(si => si.Categories)
                .Where(si => si.Categories.Select(c => c.Id).Contains(categoryId.Value));
        }

        var skipped = (realPage - 1) * realPageSize;
        var storeItems = storeItemsQuery
            .Skip(skipped)
            .Take(realPageSize);

        var storeItemIds = storeItems.Select(si => si.Id).ToArray();

        var itemToAmount = dbContext.StoreTransactionItems
            .Where(sti => storeItemIds.Contains(sti.StoreItemId))
            .Where(sti => sti.StoreId == storeId)
            .GroupBy(sti => sti.StoreItemId)
            .Select(g => new {
                g.Key,
                Amount = g.Sum(sti => sti.ItemAmount)
            })
            .ToDictionary(
                item => item.Key,
                item => item.Amount
            );

        List<StoreItemAmountListModel> storeItemAmounts = [];
        foreach (var storeItem in storeItems) {
            storeItemAmounts.Add(new StoreItemAmountListModel(
                storeId,
                storeItem.ToListModel(),
                itemToAmount.TryGetValue(storeItem.Id, out var amount) ? amount : 0
            ));
        }

        var totalCount = storeItemsQuery.Count();
        return new Page<StoreItemAmountListModel>(storeItemAmounts, new PageMeta(
            Page: realPage,
            PageSize: realPageSize,
            From: skipped + 1,
            To: skipped + storeItemIds.Length,
            Total: totalCount,
            PageCount: (totalCount / realPageSize) + 1
        ));
    }
}
