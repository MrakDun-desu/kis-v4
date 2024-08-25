using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class CashBoxService(KisDbContext dbContext, Mapper mapper, TimeProvider timeProvider)
    : ICashBoxService, IScopedService
{
    public List<CashBoxReadAllModel> ReadAll(bool? deleted)
    {
        return mapper.ToModels(deleted.HasValue
            ? dbContext.CashBoxes.Where(cb => cb.Deleted == deleted).ToList()
            : dbContext.CashBoxes.ToList());
    }

    public CashBoxReadModel Create(CashBoxCreateModel createModel)
    {
        var entity = mapper.ToEntity(createModel);
        entity.StockTakings.Add(new StockTakingEntity
        {
            Timestamp = timeProvider.GetUtcNow()
        });
        var insertedEntity = dbContext.CashBoxes.Add(entity);

        dbContext.SaveChanges();

        return mapper.ToModel(insertedEntity.Entity)!;
    }

    public bool Update(CashBoxUpdateModel updateModel)
    {
        if (!dbContext.CashBoxes.Any(cb => cb.Id == updateModel.Id))
            return false;

        var entity = mapper.ToEntity(updateModel);

        dbContext.CashBoxes.Update(entity);
        dbContext.SaveChanges();

        return true;
    }

    public CashBoxReadModel? Read(int id, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
    {
        // needs to do AsNoTracking because otherwise currency changes will get included by lazy loading
        var cashBox = dbContext.CashBoxes.AsNoTracking()
            .Include(cb => cb.StockTakings)
            .SingleOrDefault(cb => cb.Id == id);
        if (cashBox is null)
        {
            return null;
        }

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
                    mapper.ToModel(s.Key!),
                    s.Sum(cc => cc.Amount))
            );

        return mapper.ToModel(
            new CashBoxIntermediateModel(cashBox, currencyChanges.ToList(),
                totalCurrencyChanges.ToList()));
    }

    public bool Delete(int id)
    {
        var entity = dbContext.CashBoxes.Find(id);
        if (entity is null)
        {
            return false;
        }

        entity.Deleted = true;
        dbContext.SaveChanges();

        return true;
    }

    public bool AddStockTaking(int id)
    {
        var entity = dbContext.CashBoxes.Find(id);
        if (entity is null)
        {
            return false;
        }

        entity.StockTakings.Add(new StockTakingEntity { Timestamp = timeProvider.GetUtcNow() });

        dbContext.CashBoxes.Update(entity);
        dbContext.SaveChanges();

        return true;
    }
}