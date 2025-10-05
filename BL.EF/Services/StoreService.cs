using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class StoreService(
        KisDbContext dbContext,
        StoreItemAmountService storeItemAmountService,
        SaleItemAmountService saleItemAmountService
        ) : IStoreService, IScopedService {
    public StoreDetailModel Create(StoreCreateModel createModel) {
        var entity = createModel.ToEntity();

        dbContext.Stores.Add(entity);
        dbContext.SaveChanges();

        return Read(entity.Id).AsT0;
    }

    public List<StoreListModel> ReadAll() {
        return dbContext.Stores.ToList().ToModels();
    }

    public bool Update(int id, StoreCreateModel updateModel) {
        var entity = dbContext.Stores.Find(id);
        if (entity is null) {
            return false;
        }

        updateModel.UpdateEntity(entity);

        dbContext.Stores.Update(entity);
        dbContext.SaveChanges();

        return true;
    }

    public bool Delete(int id) {
        var entity = dbContext.Stores.Find(id);
        if (entity is null) {
            return false;
        }

        dbContext.Stores.Remove(entity);
        dbContext.SaveChanges();

        return true;
    }

    public OneOf<StoreDetailModel, NotFound> Read(int id) {
        var storeEntity = dbContext.Stores.Find(id);
        return storeEntity is null
            ? new NotFound()
            : new StoreIntermediateModel(
                storeEntity,
                storeItemAmountService.ReadAll(id, null, null, null).AsT0,
                saleItemAmountService.ReadAll(id, null, null, null).AsT0
            )
            .ToModel();
    }
}
