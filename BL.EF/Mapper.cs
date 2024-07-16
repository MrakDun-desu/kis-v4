using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;
using Riok.Mapperly.Abstractions;

namespace KisV4.BL.EF;

[Mapper]
public partial class Mapper {
    public partial CashBoxEntity ToEntity(CashBoxCreateModel model);
    public partial CashBoxReadModel? ToModel(CashBoxEntity? entity);
    public partial List<CashBoxReadAllModel> ToModels(List<CashBoxEntity> entities);

    public partial ProductCategoryEntity ToEntity(CategoryCreateModel model);
    public partial ProductCategoryEntity ToEntity(CategoryReadAllModel model);
    public partial List<CategoryReadAllModel> ToModels(List<ProductCategoryEntity> entities);

    public partial CurrencyEntity ToEntity(CurrencyCreateModel model);
    public partial List<CurrencyReadAllModel> ToModels(List<CurrencyEntity> entities);

    public partial PipeEntity ToEntity(PipeCreateModel model);
    public partial List<PipeReadAllModel> ToModels(List<PipeEntity> entities);

    public partial SaleItemEntity ToEntity(SaleItemCreateModel model);
    public partial SaleItemReadModel? ToModel(SaleItemEntity? entity);
    public partial List<SaleItemReadAllModel> ToModels(List<SaleItemEntity> entities);

    public partial StoreEntity ToEntity(StoreCreateModel model);
    public partial List<StoreReadAllModel> ToModels(List<StoreEntity> entities);
}
