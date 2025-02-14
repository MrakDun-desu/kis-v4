using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface IStoreService
{
    int Create(StoreCreateModel createModel);
    List<StoreListModel> ReadAll();
    bool Update(int id, StoreCreateModel updateModel);
    bool Delete(int id);
}