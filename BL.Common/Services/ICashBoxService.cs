using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface ICashBoxService
{
    public List<CashBoxListModel> ReadAll(bool? deleted);
    public CashBoxDetailModel Create(CashBoxCreateModel createModel);
    public OneOf<CashBoxDetailModel, NotFound> Update(int id, CashBoxCreateModel updateModel);

    public OneOf<CashBoxDetailModel, NotFound, Dictionary<string, string[]>> Read(
        int id,
        DateTimeOffset? startDate = null,
        DateTimeOffset? endDate = null);

    public void Delete(int id);
    public OneOf<Success, NotFound> AddStockTaking(int id);
}