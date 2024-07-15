using KisV4.Common.Models.CashBox;

namespace KisV4.BL.Common;

public interface ICashboxService {
    public int Create(CashboxCreateModel createModel);
    public List<CashboxListModel> ReadAll();
    public CashBoxDetailModel? Read(int id);
    public bool Update(int id, CashboxUpdateModel updateModel);
    public bool Delete(int id);
}
