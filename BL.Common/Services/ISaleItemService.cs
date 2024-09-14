using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface ISaleItemService
{
    public OneOf<Page<SaleItemListModel>, Dictionary<string, string[]>> ReadAll(
        int? page,
        int? pageSize,
        bool? deleted,
        int? categoryId,
        bool? showOnWeb
    );

    public OneOf<SaleItemDetailModel, Dictionary<string, string[]>> Create(
        SaleItemCreateModel createModel
    );

    public OneOf<SaleItemDetailModel, NotFound> Read(int id);

    public OneOf<SaleItemDetailModel, NotFound, Dictionary<string, string[]>> Update(
        int id,
        SaleItemCreateModel updateModel
    );

    public OneOf<SaleItemDetailModel, NotFound> Delete(int id);
}