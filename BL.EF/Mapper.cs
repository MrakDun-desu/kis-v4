using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;
using Riok.Mapperly.Abstractions;

namespace KisV4.BL.EF;

[Mapper]
public partial class Mapper
{
    public partial CashBoxEntity ToEntity(CashBoxCreateModel model);
    public partial CashBoxEntity ToEntity(CashBoxUpdateModel model);

    public partial CashBoxReadModel? ToModel(CashBoxEntity? entity);

    public CashBoxReadModel ToModel(CashBoxIntermediateModel model)
    {
        return new CashBoxReadModel(
            model.entity.Id,
            model.entity.Name,
            model.entity.Deleted,
            ToModels(model.currencyChanges),
            model.totalCurrencyChanges,
            ToModels(model.entity.StockTakings.ToList())
        );
    }

    private partial List<CurrencyChangeModel> ToModels(List<CurrencyChangeEntity> entities);
    private partial List<StockTakingModel> ToModels(List<StockTakingEntity> entities);

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

    public partial CompositionEntity ToEntity(CompositionCreateModel model);

    public partial ContainerEntity ToEntity(ContainerCreateModel model);
    public partial List<ContainerReadAllModel> ToModels(List<ContainerEntity> entities);

    public partial CurrencyCostEntity ToEntity(CostCreateModel createModel);

    public partial CurrencyReadAllModel ToModel(CurrencyEntity entity);

    public partial DiscountReadModel? ToModel(DiscountEntity? entity);
    public partial List<DiscountReadAllModel> ToModels(List<DiscountEntity> entities);

    public partial ModifierEntity ToEntity(ModifierCreateModel model);
    public partial ModifierReadModel? ToModel(ModifierEntity? entity);
    public partial List<ModifierReadAllModel> ToModels(List<ModifierEntity> entities);

    public partial StoreItemEntity ToEntity(StoreItemCreateModel model);
    public partial StoreItemReadModel? ToModel(StoreItemEntity? entity);
    public partial List<StoreItemReadAllModel> ToModels(List<StoreItemEntity> entities);

    public partial DiscountUsageReadModel? ToModel(DiscountUsageEntity? entity);
}

public record CashBoxIntermediateModel(
    CashBoxEntity entity,
    List<CurrencyChangeEntity> currencyChanges,
    List<TotalCurrencyChangeModel> totalCurrencyChanges
);