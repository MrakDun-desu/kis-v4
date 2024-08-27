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
public class CashBoxService(KisDbContext dbContext, TimeProvider timeProvider)
    : ICashBoxService, IScopedService
{
    public List<CashBoxReadAllModel> ReadAll(bool? deleted)
    {
        return deleted.HasValue
            ? dbContext.CashBoxes.Where(cb => cb.Deleted == deleted).ToList().ToModels()
            : dbContext.CashBoxes.ToList().ToModels();
    }

    public CashBoxReadModel Create(CashBoxCreateModel createModel)
    {
        var entity = createModel.ToEntity();
        entity.StockTakings.Add(new StockTakingEntity
        {
            Timestamp = timeProvider.GetUtcNow()
        });
        var insertedEntity = dbContext.CashBoxes.Add(entity);

        dbContext.SaveChanges();

        return insertedEntity.Entity.ToModel()!;
    }

    public OneOf<Success, NotFound> Update(CashBoxUpdateModel updateModel)
    {
        if (!dbContext.CashBoxes.Any(cb => cb.Id == updateModel.Id))
            return new NotFound();

        var entity = updateModel.ToEntity();
        // updating automatically restores entities from deletion
        entity.Deleted = false;

        dbContext.CashBoxes.Update(entity);
        dbContext.SaveChanges();

        return new Success();
    }

    public OneOf<CashBoxReadModel, NotFound> Read(int id, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
    {
        // needs to do AsNoTracking because otherwise currency changes will get included by lazy loading
        var cashBox = dbContext.CashBoxes.AsNoTracking()
            .Include(cb => cb.StockTakings)
            .SingleOrDefault(cb => cb.Id == id);
        if (cashBox is null) return new NotFound();

        var lastTimestamp = cashBox.StockTakings.Last().Timestamp;

        // need to use explicit querying here because query is too complex to utilize lazy loading
        var realStartDate = startDate ?? lastTimestamp;
        var realEndDate = endDate ?? timeProvider.GetUtcNow();
        var currencyChanges = dbContext.CurrencyChanges
            .Include(cc => cc.SaleTransaction)
            .Include(cc => cc.Currency)
            .Where(cc => cc.AccountId == id)
            .Where(cc => cc.SaleTransaction!.Timestamp > realStartDate && cc.SaleTransaction.Timestamp < realEndDate);

        var totalCurrencyChanges = currencyChanges
            .GroupBy(cc => cc.Currency)
            .Select(s =>
                new TotalCurrencyChangeModel(
                    s.Key!.ToModel(),
                    s.Sum(cc => cc.Amount))
            );

        return new CashBoxIntermediateModel(cashBox, currencyChanges.ToList(),
            totalCurrencyChanges.ToList()).ToReadModel();
    }

    public OneOf<Success, NotFound> Delete(int id)
    {
        var entity = dbContext.CashBoxes.Find(id);
        if (entity is null) return new NotFound();

        entity.Deleted = true;
        dbContext.SaveChanges();

        return new Success();
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