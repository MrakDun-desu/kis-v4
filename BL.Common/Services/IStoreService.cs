using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface IStoreService
{
    public int Create(StoreCreateModel createModel);
    public List<StoreListModel> ReadAll();
    public bool Update(int id, StoreCreateModel updateModel);
    public bool Delete(int id);
}