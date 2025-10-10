using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface IStoreItemService {
    OneOf<Page<StoreItemListModel>, Dictionary<string, string[]>> ReadAll(
        int? page,
        int? pageSize,
        bool? deleted,
        int? categoryId,
        int? storeId);

    OneOf<StoreItemDetailModel, Dictionary<string, string[]>> Create(StoreItemCreateModel createModel);
    OneOf<StoreItemDetailModel, NotFound> Read(int id);
    OneOf<StoreItemDetailModel, NotFound, Dictionary<string, string[]>> Update(int id,
        StoreItemCreateModel updateModel);

    OneOf<StoreItemDetailModel, NotFound> Delete(int id);
}
