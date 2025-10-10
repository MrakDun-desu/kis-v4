
using KisV4.Common.Models;
using OneOf;

namespace KisV4.BL.Common.Services;

public interface ISaleItemAmountService {

    OneOf<Page<SaleItemAmountListModel>, Dictionary<string, string[]>> ReadAll(
        int storeId,
        int? page,
        int? pageSize,
        int? categoryId
    );
}
