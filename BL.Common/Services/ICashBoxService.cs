using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface ICashBoxService {
    public List<CashBoxReadAllModel> ReadAll(bool? deleted);
    public CashBoxReadModel Create(CashBoxCreateModel createModel);
    public bool Update(CashBoxUpdateModel updateModel);
    public CashBoxReadModel? Read(int id, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null);
    public bool Delete(int id);
    public bool AddStockTaking(int id);
}
