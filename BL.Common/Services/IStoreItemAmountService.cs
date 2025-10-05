using KisV4.Common.Models;
using OneOf;

namespace KisV4.BL.Common.Services;

public interface IStoreItemAmountService {

    OneOf<Page<StoreItemAmountListModel>, Dictionary<string, string[]>> ReadAll(
        int storeId,
        int? page,
        int? pageSize,
        int? categoryId
    );
}
