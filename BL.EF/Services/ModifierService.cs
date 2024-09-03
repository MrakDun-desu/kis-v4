using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class ModifierService(KisDbContext dbContext)
    : IModifierService, IScopedService
{
    public int Create(ModifierCreateModel createModel)
    {
        var entity = createModel.ToEntity();
        var insertedEntity = dbContext.Modifiers.Add(entity);

        dbContext.SaveChanges();

        return insertedEntity.Entity.Id;
    }

    public ModifierDetailModel? Read(int id)
    {
        return Mapper.ToModel(dbContext.Modifiers.Find(id));
    }

    public bool Update(int id, ModifierCreateModel updateModel)
    {
        if (dbContext.Modifiers.Any(m => m.Id == id))
            return false;

        var entity = updateModel.ToEntity();
        entity.Id = id;

        dbContext.Modifiers.Update(entity);
        dbContext.SaveChanges();

        return true;
    }

    public bool Delete(int id)
    {
        var entity = dbContext.Modifiers.Find(id);
        if (entity is null) return false;

        entity.Deleted = true;
        dbContext.SaveChanges();

        return true;
    }

    public List<ModifierListModel> ReadAll()
    {
        return dbContext.Modifiers
            .Where(e => !e.Deleted)
            // including categories to prevent lazy loading from sending ton of db queries
            .Include(e => e.Categories)
            .ToList().ToModels();
    }
}