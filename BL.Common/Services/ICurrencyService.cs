using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface ICurrencyService
{
    public CurrencyReadAllModel Create(CurrencyCreateModel createModel);
    public List<CurrencyReadAllModel> ReadAll();
    public OneOf<Success, NotFound> Update(CurrencyUpdateModel updateModel);
}