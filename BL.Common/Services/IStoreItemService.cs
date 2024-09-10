using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface IStoreItemService
{
    public OneOf<Page<StoreItemListModel>, Dictionary<string, string[]>> ReadAll(
        int? page, 
        int? pageSize, 
        bool? deleted, 
        int? categoryId, 
        int? storeId);
    public OneOf<StoreItemDetailModel, Dictionary<string, string[]>> Create(StoreItemCreateModel createModel);
    public OneOf<StoreItemDetailModel, NotFound> Read(int id);
    public OneOf<StoreItemDetailModel, NotFound, Dictionary<string, string[]>> Update(int id, StoreItemCreateModel updateModel);
    public OneOf<StoreItemDetailModel, NotFound> Delete(int id);
}