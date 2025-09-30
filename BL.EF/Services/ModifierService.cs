using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class ModifierService(KisDbContext dbContext)
    : IModifierService, IScopedService {
    public OneOf<ModifierDetailModel, Dictionary<string, string[]>> Create(ModifierCreateModel createModel) {
        if (!dbContext.SaleItems.Any(si => si.Id == createModel
                .ModificationTargetId)) {
            return new Dictionary<string, string[]>
            {
                {
                    nameof(createModel.ModificationTargetId),
                    [$"Sale item with id {createModel.ModificationTargetId} doesn't exist."]
                }
            };
        }

        var entity = createModel.ToEntity();
        var insertedEntity = dbContext.Modifiers.Add(entity);

        dbContext.SaveChanges();

        return Read(insertedEntity.Entity.Id).AsT0;
    }

    public OneOf<ModifierDetailModel, NotFound> Read(int id) {
        var entity = dbContext.Modifiers
            .Include(m => m.ModificationTarget)
            .Include(m => m.Costs)
            .Include(m => m.Composition)
            .SingleOrDefault(m => m.Id == id);

        return entity is null ? (OneOf<ModifierDetailModel, NotFound>)new NotFound() : (OneOf<ModifierDetailModel, NotFound>)entity.ToModel();
    }

    public OneOf<ModifierDetailModel, NotFound, Dictionary<string, string[]>> Update(int id, ModifierCreateModel updateModel) {
        var entity = dbContext.Modifiers.Find(id);
        if (entity is null) {
            return new NotFound();
        }

        if (!dbContext.SaleItems.Any(si => si.Id == updateModel
                .ModificationTargetId)) {
            return new Dictionary<string, string[]>
            {
                {
                    nameof(updateModel.ModificationTargetId),
                    [$"Sale item with id {updateModel.ModificationTargetId} doesn't exist."]
                }
            };
        }

        updateModel.UpdateEntity(entity);
        entity.Deleted = false;

        dbContext.Modifiers.Update(entity);
        dbContext.SaveChanges();

        return Read(id).AsT0;
    }

    public OneOf<ModifierDetailModel, NotFound> Delete(int id) {
        var entity = dbContext.Modifiers.Find(id);
        if (entity is null) {
            return new NotFound();
        }

        dbContext.Modifiers.Update(entity);
        entity.Deleted = true;
        dbContext.SaveChanges();

        return Read(id).AsT0;
    }
}
