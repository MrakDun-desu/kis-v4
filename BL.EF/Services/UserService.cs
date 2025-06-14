using KisV4.BL.Common.Services;
using KisV4.Common;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class UserService(
    KisDbContext dbContext,
    ICurrencyChangeService currencyChangeService,
    IDiscountUsageService discountUsageService) : IScopedService, IUserService {
    public int CreateOrGetId(string userName) {
        var user = dbContext.UserAccounts.SingleOrDefault(ua => ua.UserName == userName);
        if (user is not null) return user.Id;
        user = new UserAccountEntity {
            UserName = userName
        };
        dbContext.UserAccounts.Add(user);
        dbContext.SaveChanges();
        return user.Id;
    }

    public OneOf<Page<UserListModel>, Dictionary<string, string[]>> ReadAll(int? page, int? pageSize, bool? deleted) {
        var query = dbContext.UserAccounts.AsQueryable();
        if (deleted is { } realDeleted) {
            query = query.Where(u => u.Deleted == realDeleted);
        }

        return query.Page(page ?? 1, pageSize ?? Constants.DefaultPageSize, Mapper.ToModels);
    }

    public OneOf<UserDetailModel, NotFound, Dictionary<string, string[]>> Read(int id) {
        // needs to do AsNoTracking because otherwise currency changes will get included by lazy loading
        var user = dbContext.UserAccounts.AsNoTracking()
            .SingleOrDefault(u => u.Id == id);

        if (user is null)
            return new NotFound();

        var totalCurrencyChanges = dbContext.CurrencyChanges
            .Include(cc => cc.Currency)
            .Where(cc => cc.AccountId == id)
            .Where(cc => !cc.Cancelled)
            .GroupBy(cc => cc.Currency).Select(s =>
                new TotalCurrencyChangeListModel(
                    s.Key!.ToModel(),
                    s.Sum(cc => cc.Amount))
            );

        var currencyChangesPage =
            currencyChangeService.ReadAll(null, null, id, false, null, null);

        if (currencyChangesPage.IsT1)
            return currencyChangesPage.AsT1;

        var discountUsagesPage =
            discountUsageService.ReadAll(null, null, null, id);

        if (discountUsagesPage.IsT1)
            return currencyChangesPage.AsT1;

        return new UserIntermediateModel(
            user,
            [.. totalCurrencyChanges],
            currencyChangesPage.AsT0,
            discountUsagesPage.AsT0
        ).ToModel();
    }
}
