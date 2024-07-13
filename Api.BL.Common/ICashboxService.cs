using KisV4.Api.Common.Models.Cashbox;

namespace Api.BL.Common;

public interface ICashboxService {
    public int Create(CashboxCreateModel createModel);
    public List<CashboxListModel> ReadAll();
    public CashboxDetailModel? Read(int id);
    public bool Update(int id, CashboxUpdateModel updateModel);
    public bool Delete(int id);
}
