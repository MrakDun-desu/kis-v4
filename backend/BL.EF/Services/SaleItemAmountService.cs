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
public class SaleItemAmountService(KisDbContext dbContext)
    : ISaleItemAmountService, IScopedService {
    public OneOf<Page<SaleItemAmountListModel>, Dictionary<string, string[]>> ReadAll(
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

        if (categoryId is { } categoryIdReal && dbContext.ProductCategories
                .Find(categoryIdReal) is null) {
            errors.AddItemOrCreate(
                nameof(categoryId),
                $"Category {categoryId} doesn't exist"
            );
        }

        if (errors.Count > 0) {
            return errors;
        }

        var saleItemsQuery = dbContext.SaleItems.AsQueryable();

        if (categoryId is { }) {
            saleItemsQuery = saleItemsQuery
                .Include(si => si.Categories)
                .Where(si => si.Categories.Any(c => c.Id == categoryId.Value));
        }

        var skipped = (realPage - 1) * realPageSize;
        var saleItems = saleItemsQuery
            .Include(si => si.Composition)
            .Skip(skipped)
            .Take(realPageSize)
            .ToArray();

        var storeItemIds = saleItems
            .SelectMany((si) =>
                si.Composition.Select(comp => comp.StoreItemId))
            .Distinct()
            .ToArray();

        var storeItemAmounts = dbContext.StoreTransactionItems
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

        var saleItemAmounts = saleItems
            .Select(si => new SaleItemAmountListModel(
                storeId,
                si.ToListModel(),
                (int)si.Composition
                    .Min(comp => {
                        return storeItemAmounts.TryGetValue(comp.StoreItemId, out var amount)
                            ? amount / comp.Amount
                            : 0;
                    })
                ))
            .ToList();

        var totalCount = saleItemsQuery.Count();
        return new Page<SaleItemAmountListModel>(saleItemAmounts, new PageMeta(
            Page: realPage,
            PageSize: realPageSize,
            From: skipped + 1,
            To: skipped + storeItemIds.Length,
            Total: totalCount,
            PageCount: (totalCount / realPageSize) + 1
        ));
    }
}
