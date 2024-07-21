using KisV4.BL.Common;
using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class CashBoxService(KisDbContext dbContext, Mapper mapper)
    : ICashBoxService, IScopedService {
    public int Create(CashBoxCreateModel createModel) {
        var entity = mapper.ToEntity(createModel);
        var insertedEntity = dbContext.CashBoxes.Add(entity);

        dbContext.SaveChanges();

        return insertedEntity.Entity.Id;
    }

    public List<CashBoxReadAllModel> ReadAll() {
        return mapper.ToModels(dbContext.CashBoxes.ToList());
    }

    public CashBoxReadModel? Read(int id) {
        var lastStockTaking = dbContext.StockTakings
            .Where(st => st.CashboxId == id)
            .OrderByDescending(st => st.Timestamp).FirstOrDefault();

        if (lastStockTaking is null) {
            return mapper.ToModel(dbContext.CashBoxes.Find(id));
        }

        var lastTimestamp = lastStockTaking.Timestamp;
        var currencyChanges = dbContext.CurrencyChanges
            .Include(cc => cc.SaleTransaction)
            .Where(cc => cc.AccountId == id && cc.SaleTransaction!.Timestamp > lastTimestamp);
        var cashBox = dbContext.CashBoxes.Find(id);
        if (cashBox is null) {
            return null;
        }

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

        var newStockTaking = new StockTakingEntity { CashboxId = id, Timestamp = DateTime.Now };

        dbContext.StockTakings.Add(newStockTaking);
        dbContext.SaveChanges();

        return true;
    }
}
