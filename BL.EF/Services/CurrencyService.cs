using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class CurrencyService(KisDbContext dbContext) : ICurrencyService, IScopedService
{
    public CurrencyListModel Create(CurrencyCreateModel createModel)
    {
        var entity = createModel.ToEntity();
        dbContext.Currencies.Add(entity);

        dbContext.SaveChanges();

        return entity.ToModel();
    }

    public List<CurrencyListModel> ReadAll()
    {
        return dbContext.Currencies.ToList().ToModels();
    }

    public OneOf<Success, NotFound> Update(int id, CurrencyCreateModel updateModel)
    {
        if (!dbContext.Currencies.Any(c => c.Id == id)) 
            return new NotFound();

        var entity = updateModel.ToEntity();
        entity.Id = id;

        dbContext.Currencies.Update(entity);
        dbContext.SaveChanges();

        return new Success();
    }
}