using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface IStoreItemService
{
    public int Create(StoreItemCreateModel createModel);
    public List<StoreItemListModel> ReadAll();
    public StoreItemDetailModel? Read(int id);
    public bool Update(int id, StoreItemCreateModel updateModel);
    public bool Delete(int id);
}