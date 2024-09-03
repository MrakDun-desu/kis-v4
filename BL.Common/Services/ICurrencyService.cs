using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface ICurrencyService
{
    public CurrencyListModel Create(CurrencyCreateModel createModel);
    public List<CurrencyListModel> ReadAll();
    public OneOf<Success, NotFound> Update(int id, CurrencyCreateModel updateModel);
}