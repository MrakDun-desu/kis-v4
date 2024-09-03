using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

public class CompositionService(KisDbContext dbContext) : ICompositionService, IScopedService
{
    public OneOf<Success, Dictionary<string, string[]>> Create(CompositionCreateModel createModel)
    {
        var errors = new Dictionary<string, string[]>();
        if (!dbContext.SaleItems.Any(si => si.Id == createModel.SaleItemId))
            errors.AddItemOrCreate(
                nameof(createModel.SaleItemId),
                $"Sale item with id {createModel.SaleItemId} doesn't exist"
            );

        if (!dbContext.StoreItems.Any(si => si.Id == createModel.StoreItemId))
            errors.AddItemOrCreate(
                nameof(createModel.StoreItemId),
                $"Store item with id {createModel.StoreItemId} doesn't exist"
            );

        if (errors.Count != 0) return errors;

        var composition =
            dbContext.Compositions.Find(createModel.SaleItemId, createModel.StoreItemId);

        if (composition is null)
        {
            if (createModel.Amount == 0) return new Success();

            var entity = createModel.ToEntity();
            dbContext.Compositions.Add(entity);
        }
        else
        {
            if (createModel.Amount == 0)
            {
                dbContext.Compositions.Remove(composition);
            }
            else
            {
                composition.Amount = createModel.Amount;
                dbContext.Update(composition);
            }
        }

        dbContext.SaveChanges();
        return new Success();
    }
}