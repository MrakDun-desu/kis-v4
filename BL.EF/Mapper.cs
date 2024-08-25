using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;
using Riok.Mapperly.Abstractions;

namespace KisV4.BL.EF;

[Mapper]
public static partial class Mapper
{
    public static partial CashBoxEntity ToEntity(this CashBoxCreateModel model);
    public static partial CashBoxEntity ToEntity(this CashBoxUpdateModel model);

    public static partial CashBoxReadModel? ToModel(this CashBoxEntity? entity);

    public static CashBoxReadModel ToReadModel(this CashBoxIntermediateModel model)
    {
        return new CashBoxReadModel(
            model.Entity.Id,
            model.Entity.Name,
            model.Entity.Deleted,
            model.CurrencyChanges.ToModels(),
            model.TotalCurrencyChanges,
            model.Entity.StockTakings.ToList().ToModels()
        );
    }

    public static partial CategoryReadAllModel ToModel(this ProductCategoryEntity entity);
    public static partial ProductCategoryEntity ToEntity(this CategoryUpdateModel model);
    
    private static partial List<CurrencyChangeModel> ToModels(this List<CurrencyChangeEntity> entities);
    private static partial List<StockTakingModel> ToModels(this List<StockTakingEntity> entities);

    public static partial List<CashBoxReadAllModel> ToModels(this List<CashBoxEntity> entities);

    public static partial ProductCategoryEntity ToEntity(this CategoryCreateModel model);
    public static partial ProductCategoryEntity ToEntity(this CategoryReadAllModel model);
    public static partial List<CategoryReadAllModel> ToModels(this List<ProductCategoryEntity> entities);

    public static partial CurrencyEntity ToEntity(this CurrencyCreateModel model);
    public static partial List<CurrencyReadAllModel> ToModels(this List<CurrencyEntity> entities);

    public static partial PipeEntity ToEntity(this PipeCreateModel model);
    public static partial List<PipeReadAllModel> ToModels(this List<PipeEntity> entities);

    public static partial SaleItemEntity ToEntity(this SaleItemCreateModel model);
    public static partial SaleItemReadModel? ToModel(this SaleItemEntity? entity);
    public static partial List<SaleItemReadAllModel> ToModels(this List<SaleItemEntity> entities);

    public static partial StoreEntity ToEntity(this StoreCreateModel model);
    public static partial List<StoreReadAllModel> ToModels(this List<StoreEntity> entities);

    public static partial CompositionEntity ToEntity(this CompositionCreateModel model);

    public static partial ContainerEntity ToEntity(this ContainerCreateModel model);
    public static partial List<ContainerReadAllModel> ToModels(this List<ContainerEntity> entities);

    public static partial CurrencyCostEntity ToEntity(this CostCreateModel createModel);

    public static partial CurrencyReadAllModel ToModel(this CurrencyEntity entity);

    public static partial DiscountReadModel? ToModel(this DiscountEntity? entity);
    public static partial List<DiscountReadAllModel> ToModels(this List<DiscountEntity> entities);

    public static partial ModifierEntity ToEntity(this ModifierCreateModel model);
    public static partial ModifierReadModel? ToModel(this ModifierEntity? entity);
    public static partial List<ModifierReadAllModel> ToModels(this List<ModifierEntity> entities);

    public static partial StoreItemEntity ToEntity(this StoreItemCreateModel model);
    public static partial StoreItemReadModel? ToModel(this StoreItemEntity? entity);
    public static partial List<StoreItemReadAllModel> ToModels(this List<StoreItemEntity> entities);

    public static partial DiscountUsageReadModel? ToModel(this DiscountUsageEntity? entity);
}

public record CashBoxIntermediateModel(
    CashBoxEntity Entity,
    List<CurrencyChangeEntity> CurrencyChanges,
    List<TotalCurrencyChangeModel> TotalCurrencyChanges
);