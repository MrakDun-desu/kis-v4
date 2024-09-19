using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class CashBoxService(
    KisDbContext dbContext,
    ICurrencyChangeService currencyChangeService,
    TimeProvider timeProvider)
    : ICashBoxService, IScopedService
{
    public List<CashBoxListModel> ReadAll(bool? deleted)
    {
        return deleted.HasValue
            ? dbContext.CashBoxes.Where(cb => cb.Deleted == deleted).ToList().ToModels()
            : dbContext.CashBoxes.ToList().ToModels();
    }

    public CashBoxDetailModel Create(CashBoxCreateModel createModel)
    {
        var entity = createModel.ToEntity();
        entity.StockTakings.Add(new StockTakingEntity
        {
            Timestamp = timeProvider.GetUtcNow()
        });
        dbContext.CashBoxes.Add(entity);

        dbContext.SaveChanges();

        return entity.ToModel();
    }

    public OneOf<CashBoxDetailModel, NotFound> Update(int id, CashBoxCreateModel updateModel)
    {
        var entity = dbContext.CashBoxes.Include(cb => cb.StockTakings).SingleOrDefault(cb => cb.Id == id);
        if (entity is null)
            return new NotFound();

        // updating automatically restores entities from deletion
        updateModel.UpdateEntity(entity);
        entity.Deleted = false;

        dbContext.SaveChanges();

        return Read(id).AsT0;
    }

    public OneOf<CashBoxDetailModel, NotFound, Dictionary<string, string[]>> Read(
        int id,
        DateTimeOffset? startDate = null,
        DateTimeOffset? endDate = null)
    {
        // needs to do AsNoTracking because otherwise currency changes will get included by lazy loading
        var cashBox = dbContext.CashBoxes.AsNoTracking()
            .Include(cb => cb.StockTakings)
            .SingleOrDefault(cb => cb.Id == id);

        if (cashBox is null)
            return new NotFound();

        var lastTimestamp = cashBox.StockTakings
            .OrderBy(st => st.Timestamp)
            .First().Timestamp;

        var realStartDate = startDate ?? lastTimestamp;
        var realEndDate = endDate ?? timeProvider.GetUtcNow();

        var totalCurrencyChanges = dbContext.CurrencyChanges
            .Include(cc => cc.SaleTransaction)
            .Include(cc => cc.Currency)
            .Where(cc => cc.AccountId == id)
            .Where(cc => !cc.Cancelled)
            .Where(cc => cc.SaleTransaction!.Timestamp > realStartDate && cc.SaleTransaction.Timestamp < realEndDate)
            .GroupBy(cc => cc.Currency).Select(s =>
                new TotalCurrencyChangeListModel(
                    s.Key!.ToModel(),
                    s.Sum(cc => cc.Amount))
            );

        var currencyChangesPage =
            currencyChangeService.ReadAll(null, null, id, false, realStartDate, realEndDate);

        if (currencyChangesPage.IsT1)
            return currencyChangesPage.AsT1;

        return new CashBoxIntermediateModel(
                cashBox,
                currencyChangesPage.AsT0,
                totalCurrencyChanges.ToList())
            .ToDetailModel();
    }

    public OneOf<CashBoxDetailModel, NotFound> Delete(int id)
    {
        var entity = dbContext.CashBoxes.Find(id);
        if (entity is null) return new NotFound();
        entity.Deleted = true;
        dbContext.SaveChanges();

        return Read(id).AsT0;
    }

    public OneOf<Success, NotFound> AddStockTaking(int id)
    {
        var entity = dbContext.CashBoxes.Find(id);
        if (entity is null) return new NotFound();

        entity.StockTakings.Add(new StockTakingEntity { Timestamp = timeProvider.GetUtcNow() });

        dbContext.CashBoxes.Update(entity);
        dbContext.SaveChanges();

        return new Success();
    }
}