using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface ICurrencyService {
    public int Create(CurrencyCreateModel createModel);
    public List<CurrencyReadAllModel> ReadAll();
    public bool Update(int id, CurrencyUpdateModel updateModel);
}
