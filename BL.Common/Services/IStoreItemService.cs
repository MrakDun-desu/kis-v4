using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface IStoreItemService {
    public int Create(StoreItemCreateModel createModel);
    public List<StoreItemReadAllModel> ReadAll();
    public StoreItemReadModel? Read(int id);
    public bool Update(int id, StoreItemUpdateModel updateModel);
    public bool Delete(int id);
}
