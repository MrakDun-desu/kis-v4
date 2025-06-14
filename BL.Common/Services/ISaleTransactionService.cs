using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface ISaleTransactionService {
    OneOf<Page<SaleTransactionListModel>, Dictionary<string, string[]>> ReadAll(
        int? page,
        int? pageSize,
        DateTimeOffset? startDate,
        DateTimeOffset? endDate,
        bool? cancelled
    );

    IEnumerable<SaleTransactionListModel> ReadSelfCancellable(string userName);

    OneOf<SaleTransactionDetailModel, Dictionary<string, string[]>> Create(
        SaleTransactionCreateModel createModel,
        string userName
    );

    OneOf<SaleTransactionDetailModel, NotFound, Dictionary<string, string[]>> Patch(
        int id,
        SaleTransactionCreateModel updateModel,
        string userName
    );

    OneOf<SaleTransactionDetailModel, NotFound> Read(int id);

    OneOf<SaleTransactionDetailModel, NotFound, Dictionary<string, string[]>> Finish(
        int id,
        IEnumerable<CurrencyChangeListModel> currencyChanges
    );

    OneOf<SaleTransactionDetailModel, NotFound> Delete(int id);
}
