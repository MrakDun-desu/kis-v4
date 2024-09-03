using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface ISaleItemService
{
    public int Create(SaleItemCreateModel createModel);
    public List<SaleItemListModel> ReadAll();
    public SaleItemDetailModel? Read(int id);
    public bool Update(int id, SaleItemCreateModel updateModel);
    public bool Delete(int id);
}