using KisV4.Common.Models;
using OneOf.Types;
using OneOf;

namespace KisV4.BL.Common.Services;

public interface IStoreTransactionService {
    OneOf<Page<StoreTransactionListModel>, Dictionary<string, string[]>> ReadAll(
        int? page,
        int? pageSize,
        DateTimeOffset? startDate,
        DateTimeOffset? endDate,
        bool? cancelled);

    IEnumerable<StoreTransactionListModel> ReadSelfCancellable(
        string userName);

    OneOf<StoreTransactionDetailModel, NotFound> Read(int id);

    OneOf<StoreTransactionDetailModel, Dictionary<string, string[]>> Create(
        StoreTransactionCreateModel createModel,
        string userName);

    OneOf<StoreTransactionDetailModel, NotFound> Delete(int id);
}
