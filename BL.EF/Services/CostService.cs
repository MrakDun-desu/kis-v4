using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using OneOf;

namespace KisV4.BL.EF.Services;

public class CostService(KisDbContext dbContext)
    : ICostService, IScopedService
{
    public OneOf<CostListModel, Dictionary<string, string[]>> Create(CostCreateModel createModel)
    {
        var errors = new Dictionary<string, string[]>();
        if (!dbContext.Products.Any(p => p.Id == createModel.ProductId))
        {
            errors.AddItemOrCreate(
                nameof(createModel.ProductId),
                $"Product with id {createModel.ProductId} doesn't exist"
                );
        }
        if (!dbContext.Currencies.Any(p => p.Id == createModel.CurrencyId))
        {
            errors.AddItemOrCreate(
                nameof(createModel.CurrencyId),
                $"Currency with id {createModel.CurrencyId} doesn't exist"
                );
        }

        if (errors.Count > 0)
        {
            return errors;
        }
        
        var entity = (createModel with
        {
            ValidSince = createModel.ValidSince.ToUniversalTime()
        }).ToEntity();

        dbContext.CurrencyCosts.Add(entity);

        dbContext.SaveChanges();

        return entity.ToModel();
    }
}