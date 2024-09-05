using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;
using Riok.Mapperly.Abstractions;

namespace KisV4.BL.EF;

[Mapper]
public static partial class Mapper
{
    public static partial CashBoxEntity ToEntity(this CashBoxCreateModel model);
    public static partial void UpdateEntity(this CashBoxCreateModel model, CashBoxEntity source);

    public static CashBoxDetailModel ToModel(this CashBoxEntity entity)
    {
        return new CashBoxIntermediateModel(
                entity,
                Page<CurrencyChangeListModel>.Empty,
                [])
            .ToDetailModel();
    }

    public static CashBoxDetailModel ToDetailModel(this CashBoxIntermediateModel model)
    {
        return new CashBoxDetailModel(
            model.Entity.Id,
            model.Entity.Name,
            model.Entity.Deleted,
            model.CurrencyChanges,
            model.TotalCurrencyChanges,
            model.Entity.StockTakings.ToList().ToModels()
        );
    }

    public static partial CategoryListModel ToModel(this ProductCategoryEntity entity);

    public static partial List<CurrencyChangeListModel> ToModels(this List<CurrencyChangeEntity> entities);
    private static partial List<StockTakingModel> ToModels(this List<StockTakingEntity> entities);

    public static partial List<CashBoxListModel> ToModels(this List<CashBoxEntity> entities);

    public static partial ProductCategoryEntity ToEntity(this CategoryCreateModel model);
    public static partial ProductCategoryEntity ToEntity(this CategoryListModel model);
    public static partial List<CategoryListModel> ToModels(this List<ProductCategoryEntity> entities);

    public static partial CurrencyEntity ToEntity(this CurrencyCreateModel model);
    public static partial List<CurrencyListModel> ToModels(this List<CurrencyEntity> entities);

    public static partial PipeEntity ToEntity(this PipeCreateModel model);
    public static partial PipeListModel? ToModel(this PipeEntity? entity);
    public static partial List<PipeListModel> ToModels(this List<PipeEntity> entities);

    public static partial SaleItemEntity ToEntity(this SaleItemCreateModel model);
    public static partial SaleItemDetailModel? ToModel(this SaleItemEntity? entity);
    public static partial List<SaleItemListModel> ToModels(this List<SaleItemEntity> entities);

    public static partial StoreEntity ToEntity(this StoreCreateModel model);
    public static partial List<StoreListModel> ToModels(this List<StoreEntity> entities);

    public static partial CompositionEntity ToEntity(this CompositionCreateModel model);

    public static partial CompositionListModel ToModel(this CompositionEntity entity);
    public static partial ContainerEntity ToEntity(this ContainerCreateModel model);

    public static ContainerListModel ToModel(this ContainerIntermediateModel model)
    {
        return new ContainerListModel(
            model.Entity.Id,
            model.Entity.OpenSince,
            model.Entity.Pipe.ToModel(),
            model.Entity.Deleted,
            model.Entity.Template!.ToModel(),
            model.CurrentAmount
        );
    }

    public static List<ContainerListModel> ToModels(this List<ContainerIntermediateModel> models)
    {
        return models.Select(m => m.ToModel()).ToList();
    }

    public static partial ContainerTemplateListModel ToModel(this ContainerTemplateEntity entity);
    public static partial List<ContainerTemplateListModel> ToModels(this List<ContainerTemplateEntity> entities);
    public static partial ContainerTemplateEntity ToEntity(this ContainerTemplateCreateModel model);
    public static partial void UpdateEntity(this ContainerTemplateCreateModel model, ContainerTemplateEntity entity);
    public static partial CurrencyCostEntity ToEntity(this CostCreateModel createModel);
    public static partial CostListModel ToModel(this CurrencyCostEntity entity);

    public static partial CurrencyListModel ToModel(this CurrencyEntity entity);

    public static partial List<DiscountListModel> ToModels(this List<DiscountEntity> entities);

    public static partial ModifierEntity ToEntity(this ModifierCreateModel model);
    public static partial ModifierDetailModel? ToModel(this ModifierEntity? entity);
    public static partial List<ModifierListModel> ToModels(this List<ModifierEntity> entities);

    public static partial StoreItemEntity ToEntity(this StoreItemCreateModel model);
    public static partial StoreItemDetailModel? ToModel(this StoreItemEntity? entity);
    public static partial List<StoreItemListModel> ToModels(this List<StoreItemEntity> entities);

    public static partial DiscountUsageDetailModel? ToModel(this DiscountUsageEntity? entity);
}

public record CashBoxIntermediateModel(
    CashBoxEntity Entity,
    Page<CurrencyChangeListModel> CurrencyChanges,
    List<TotalCurrencyChangeListModel> TotalCurrencyChanges
);

public record ContainerIntermediateModel(
    ContainerEntity Entity,
    decimal CurrentAmount
);