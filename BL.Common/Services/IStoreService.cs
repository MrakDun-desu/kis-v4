using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface IStoreService {
    StoreDetailModel Create(StoreCreateModel createModel);
    OneOf<StoreDetailModel, NotFound> Read(int id);
    List<StoreListModel> ReadAll();
    bool Update(int id, StoreCreateModel updateModel);
    bool Delete(int id);
}
