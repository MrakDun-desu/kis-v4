using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface ICurrencyService {
    CurrencyListModel Create(CurrencyCreateModel createModel);
    IEnumerable<CurrencyListModel> ReadAll();
    OneOf<CurrencyListModel, NotFound> Update(int id, CurrencyCreateModel updateModel);
}
