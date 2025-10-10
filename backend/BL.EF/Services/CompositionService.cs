using KisV4.BL.Common.Services;
using KisV4.BL.EF.Helpers;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

public class CompositionService(KisDbContext dbContext) : ICompositionService, IScopedService {
    public OneOf<Success, CompositionListModel, Dictionary<string, string[]>> CreateOrUpdate(
        CompositionCreateModel createModel) {
        var errors = new Dictionary<string, string[]>();
        if (!dbContext.SaleItems.Any(si => si.Id == createModel.SaleItemId)) {
            errors.AddItemOrCreate(
                nameof(createModel.SaleItemId),
                $"Sale item with id {createModel.SaleItemId} doesn't exist"
            );
        }

        if (!dbContext.StoreItems.Any(si => si.Id == createModel.StoreItemId)) {
            errors.AddItemOrCreate(
                nameof(createModel.StoreItemId),
                $"Store item with id {createModel.StoreItemId} doesn't exist"
            );
        }

        if (errors.Count != 0) {
            return errors;
        }

        var composition =
            dbContext.Compositions.Find(createModel.SaleItemId, createModel.StoreItemId);

        if (composition is not null) {
            if (createModel.Amount == 0) {
                dbContext.Compositions.Remove(composition);
                dbContext.SaveChanges();
                return new Success();
            }

            composition.Amount = createModel.Amount;
            dbContext.Update(composition);
        } else {
            if (createModel.Amount == 0) {
                return new Success();
            }

            composition = createModel.ToEntity();
            dbContext.Compositions.Add(composition);
        }
        dbContext.SaveChanges();
        return dbContext.Compositions
            .Include(comp => comp.StoreItem)
            .Include(comp => comp.SaleItem)
            .First(comp => comp.SaleItemId == createModel.SaleItemId &&
                           comp.StoreItemId == createModel.StoreItemId)
            .ToModel();
    }
}
