using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface ICashBoxService
{
    public List<CashBoxReadAllModel> ReadAll(bool? deleted);
    public CashBoxReadModel Create(CashBoxCreateModel createModel);
    public OneOf<Success, NotFound> Update(CashBoxUpdateModel updateModel);

    public OneOf<CashBoxReadModel, NotFound> Read(
        int id,
        DateTimeOffset? startDate = null,
        DateTimeOffset? endDate = null);

    public OneOf<Success, NotFound> Delete(int id);
    public OneOf<Success, NotFound> AddStockTaking(int id);
}