using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface ISaleItemService {
    OneOf<Page<SaleItemListModel>, Dictionary<string, string[]>> ReadAll(
        int? page,
        int? pageSize,
        bool? deleted,
        int? categoryId,
        bool? showOnWeb
    );

    OneOf<SaleItemDetailModel, Dictionary<string, string[]>> Create(
        SaleItemCreateModel createModel
    );

    OneOf<SaleItemDetailModel, NotFound> Read(int id);

    OneOf<SaleItemDetailModel, NotFound, Dictionary<string, string[]>> Update(
        int id,
        SaleItemCreateModel updateModel
    );

    OneOf<SaleItemDetailModel, NotFound> Delete(int id);
}
