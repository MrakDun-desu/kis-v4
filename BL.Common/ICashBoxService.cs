using KisV4.Common.Models;

namespace KisV4.BL.Common;

public interface ICashBoxService {
    public int Create(CashBoxCreateModel createModel);
    public bool AddStockTaking(int id);
    public List<CashBoxReadAllModel> ReadAll();
    public CashBoxReadModel? Read(int id);
    public bool Update(int id, CashBoxUpdateModel updateModel);
    public bool Delete(int id);
}
