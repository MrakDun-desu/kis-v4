using KisV4.Common.ModelWrappers;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF;

public static class IQueryableExtensions {

    public static async Task<TOutPage> KeysetPaginate<TOutPage, TOut, TSource, TKey>(
            this IQueryable<TSource> source,
            KeysetPagedRequest<TKey> req,
            Func<TSource, TOut> mapping,
            Func<IEnumerable<TOut>, KeysetPageMeta<TKey>, TOutPage> factory,
            Func<TSource, TKey> order,
            bool orderDesc = false,
            CancellationToken token = default
            )
    where TOutPage : KeysetPagedResponse<TKey, TOut>
    where TKey : struct, IComparable<TKey> {
        var total = await source.CountAsync(token);

        var offsetCollection = req.PageStart switch {
            null => source,
            { } pageStart => orderDesc
                ? source.Where(s => order(s).CompareTo(pageStart) <= 0)
                : source.Where(s => order(s).CompareTo(pageStart) >= 0)
        };

        var ordered = (orderDesc
            ? offsetCollection.OrderByDescending(order)
            : offsetCollection.OrderBy(order)).AsQueryable();

        var queried = await ordered
            .Take(req.PageSize + 1)
            .AsAsyncEnumerable()
            .ToArrayAsync(token);

        TKey? nextPageStart = (queried.Length > req.PageSize)
            ? null
            : order(queried.Last());

        TKey? realPageStart = queried.FirstOrDefault() switch {
            null => null,
            var val => order(val)
        };

        return factory(
            queried[..req.PageSize].Select(mapping).ToArray(),
            new KeysetPageMeta<TKey> {
                Total = total,
                PageStart = realPageStart,
                NextPageStart = nextPageStart,
                PageSize = req.PageSize,
            }
        );
    }

    public static async Task<TOutPage> PaginateAsync<TOutPage, TOut, TSource, TOrder>(
            this IQueryable<TSource> source,
            PagedRequest req,
            Func<TSource, TOut> mapping,
            Func<TOut[], PageMeta, TOutPage> factory,
            Func<TSource, TOrder> order,
            bool orderDesc = false,
            CancellationToken token = default
            )
    where TOutPage : PagedResponse<TOut> {
        var total = await source.CountAsync(token);

        var ordered = orderDesc switch {
            true => source.OrderByDescending(order),
            false => source.OrderBy(order)
        };

        return factory(
            await ordered
                .Skip((req.Page - 1) * req.PageSize)
                .Take(req.PageSize)
                .Select(mapping)
                .ToAsyncEnumerable()
                .ToArrayAsync(token),
            new PageMeta {
                Page = req.Page,
                PageSize = req.PageSize,
                Total = total
            }
        );
    }
}
