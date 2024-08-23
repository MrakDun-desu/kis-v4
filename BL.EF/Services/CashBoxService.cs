using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class CashBoxService(KisDbContext dbContext, Mapper mapper, TimeProvider timeProvider)
    : ICashBoxService, IScopedService {
    public int Create(CashBoxCreateModel createModel) {
        var entity = mapper.ToEntity(createModel);
        var insertedEntity = dbContext.CashBoxes.Add(entity);

        dbContext.SaveChanges();

        return insertedEntity.Entity.Id;
    }

    public List<CashBoxReadAllModel> ReadAll() {
        return mapper.ToModels(dbContext.CashBoxes.Where(cb => !cb.Deleted).ToList());
    }

    public CashBoxReadModel? Read(int id) {
        var lastStockTaking = dbContext.StockTakings
            .Where(st => st.CashBoxId == id)
            .OrderByDescending(st => st.Timestamp).FirstOrDefault();

        if (lastStockTaking is null) {
            return mapper.ToModel(dbContext.CashBoxes.Find(id));
        }

        var lastTimestamp = lastStockTaking.Timestamp;
        // if I remove the AsNoTracking, for some reason, all the currency changes get included
        // even when not wanted. Maybe there's a better way to do this, but I don't know it
        var cashBox = dbContext.CashBoxes.AsNoTracking().SingleOrDefault(cb => cb.Id == id);
        if (cashBox is null) {
            return null;
        }

        // need to use explicit querying here because query is too complex to utilize lazy loading
        var currencyChanges = dbContext.Entry(cashBox)
            .Collection(cb => cb.CurrencyChanges).Query()
            .Where(cc => cc.SaleTransaction!.Timestamp > lastTimestamp)
            .Include(cc => cc.Currency);

        cashBox.CurrencyChanges.Clear();
        foreach (var currencyChange in currencyChanges) {
            cashBox.CurrencyChanges.Add(currencyChange);
        }

        return mapper.ToModel(cashBox);
    }

    public bool Update(int id, CashBoxUpdateModel updateModel) {
        var entity = dbContext.CashBoxes.Find(id);
        if (entity is null)
            return false;

        if (updateModel.Name is not null) {
            entity.Name = updateModel.Name;
        }

        dbContext.CashBoxes.Update(entity);
        dbContext.SaveChanges();

        return true;
    }

    public bool Delete(int id) {
        var entity = dbContext.CashBoxes.Find(id);
        if (entity is null) {
            return false;
        }

        entity.Deleted = true;
        dbContext.SaveChanges();

        return true;
    }

    public bool AddStockTaking(int id) {
        var entity = dbContext.CashBoxes.Find(id);
        if (entity is null) {
            return false;
        }

        var newStockTaking = new StockTakingEntity { CashBoxId = id, Timestamp = timeProvider.GetUtcNow() };

        dbContext.StockTakings.Add(newStockTaking);
        dbContext.SaveChanges();

        return true;
    }
}
