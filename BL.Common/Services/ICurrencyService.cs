using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface ICurrencyService
{
    public CurrencyListModel Create(CurrencyCreateModel createModel);
    public IEnumerable<CurrencyListModel> ReadAll();
    public OneOf<CurrencyListModel, NotFound> Update(int id, CurrencyCreateModel updateModel);
}