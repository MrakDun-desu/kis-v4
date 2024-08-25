using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface ISaleItemService
{
    public int Create(SaleItemCreateModel createModel);
    public List<SaleItemReadAllModel> ReadAll();
    public SaleItemReadModel? Read(int id);
    public bool Update(int id, SaleItemUpdateModel updateModel);
    public bool Delete(int id);
}