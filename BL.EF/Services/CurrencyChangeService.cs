using KisV4.BL.Common.Services;
using KisV4.Common;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace KisV4.BL.EF.Services;

public class CurrencyChangeService(KisDbContext dbContext) : ICurrencyChangeService, IScopedService
{
    public OneOf<Page<CurrencyChangeListModel>, Dictionary<string, string[]>> ReadAll(
        int? page,
        int? pageSize,
        int? accountId,
        bool? cancelled,
        DateTimeOffset? startDate,
        DateTimeOffset? endDate)
    {
        var query = dbContext.CurrencyChanges
            .Include(cc => cc.SaleTransaction)
            .Include(cc => cc.Currency)
            .OrderByDescending(cc => cc.SaleTransaction!.Timestamp)
            .AsQueryable();

        var errors = new Dictionary<string, string[]>();
        if (accountId.HasValue)
        {
            if (!dbContext.Accounts.Any(a => a.Id == accountId.Value))
            {
                errors.AddItemOrCreate(
                    nameof(accountId),
                    $"Account with id {accountId} doesn't exist");
            }
            else
            {
                query = query.Where(cc => cc.AccountId == accountId.Value);
            }
        }

        if (cancelled.HasValue)
        {
            query = query.Where(cc => cc.Cancelled == cancelled.Value);
        }

        if (startDate.HasValue)
        {
            query = query.Where(cc => cc.SaleTransaction!.Timestamp > startDate.Value);
        }
        
        if (endDate.HasValue)
        {
            query = query.Where(cc => cc.SaleTransaction!.Timestamp < endDate.Value);
        }

        var realPage = page ?? 1;
        var realPageSize = pageSize ?? Constants.DefaultPageSize;

        return query.Page(realPage, realPageSize, Mapper.ToModels);
    }
}