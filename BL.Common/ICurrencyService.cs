using KisV4.Common.Models;

namespace KisV4.BL.Common;

public interface ICurrencyService {
    public int Create(CurrencyCreateModel createModel);
    public List<CurrencyModel> ReadAll();
    public bool Update(int id, CurrencyUpdateModel updateModel);
}
