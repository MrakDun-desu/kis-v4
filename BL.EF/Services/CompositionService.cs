using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Services;

public class CompositionService(KisDbContext dbContext, Mapper mapper) : ICompositionService, IScopedService {
    public void Create(CompositionCreateModel createModel) {
        var composition =
            dbContext.Compositions.Find(createModel.SaleItemId, createModel.StoreItemId);

        if (composition is null) {
            if (createModel.Amount == 0) {
                return;
            }

            var entity = mapper.ToEntity(createModel);
            dbContext.Compositions.Add(entity);
            dbContext.SaveChanges();
            return;
        }

        if (createModel.Amount == 0) {
            dbContext.Compositions.Remove(composition);
        } else {
            composition.Amount = createModel.Amount;
            dbContext.Update(composition);
        }
        dbContext.SaveChanges();
    }
}
