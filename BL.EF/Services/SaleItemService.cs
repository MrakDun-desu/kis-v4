using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class SaleItemService(KisDbContext dbContext)
    : ISaleItemService, IScopedService
{
    public int Create(SaleItemCreateModel createModel)
    {
        var entity = createModel.ToEntity();
        dbContext.SaleItems.Add(entity);

        dbContext.SaveChanges();

        return entity.Id;
    }

    public List<SaleItemListModel> ReadAll()
    {
        return dbContext.SaleItems
            .Where(e => !e.Deleted)
            // including categories to prevent lazy loading from sending ton of db queries
            .Include(e => e.Categories)
            .ToList().ToModels();
    }

    public SaleItemDetailModel? Read(int id)
    {
        return dbContext.SaleItems.Find(id).ToModel();
    }

    public bool Update(int id, SaleItemCreateModel updateModel)
    {
        if (dbContext.SaleItems.Any(si => si.Id == id))
            return false;

        var entity = updateModel.ToEntity();
        entity.Id = id;

        dbContext.SaleItems.Update(entity);
        dbContext.SaveChanges();

        return true;
    }

    public bool Delete(int id)
    {
        var entity = dbContext.SaleItems.Find(id);
        if (entity is null) return false;

        entity.Deleted = true;
        dbContext.SaveChanges();

        return true;
    }
}