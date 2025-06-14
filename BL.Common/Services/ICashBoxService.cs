using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface ICashBoxService {
    List<CashBoxListModel> ReadAll(bool? deleted);
    CashBoxDetailModel Create(CashBoxCreateModel createModel);
    OneOf<CashBoxDetailModel, NotFound> Update(int id, CashBoxCreateModel updateModel);

    OneOf<CashBoxDetailModel, NotFound, Dictionary<string, string[]>> Read(
       int id,
       DateTimeOffset? startDate = null,
       DateTimeOffset? endDate = null);

    OneOf<CashBoxDetailModel, NotFound> Delete(int id);
    OneOf<Success, NotFound> AddStockTaking(int id);
}
