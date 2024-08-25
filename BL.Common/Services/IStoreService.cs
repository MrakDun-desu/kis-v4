using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface IStoreService
{
    public int Create(StoreCreateModel createModel);
    public List<StoreReadAllModel> ReadAll();
    public bool Update(int id, StoreUpdateModel updateModel);
    public bool Delete(int id);
}