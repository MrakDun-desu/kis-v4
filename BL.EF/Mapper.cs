using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;
using Riok.Mapperly.Abstractions;

namespace KisV4.BL.EF;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper {
    public static partial CashBoxEntity ToEntity(this CashBoxCreateModel model);
    public static partial void UpdateEntity(this CashBoxCreateModel model, CashBoxEntity source);

    public static CashBoxDetailModel ToModel(this CashBoxEntity entity) {
        return new CashBoxIntermediateModel(
                entity,
                Page<CurrencyChangeListModel>.Empty,
                [])
            .ToDetailModel();
    }

    public static CashBoxDetailModel ToDetailModel(this CashBoxIntermediateModel model) {
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
    public static partial void UpdateEntity(this PipeCreateModel model, PipeEntity entity);
    public static partial PipeListModel ToModel(this PipeEntity entity);
    public static partial List<PipeListModel> ToModels(this List<PipeEntity> entities);

    public static partial SaleItemEntity ToEntity(this SaleItemCreateModel model);
    public static partial void UpdateEntity(this SaleItemCreateModel model, SaleItemEntity entity);

    public static SaleItemDetailModel ToModel(this SaleItemIntermediateModel model) {
        return new SaleItemDetailModel(
            model.Entity.Id,
            model.Entity.Name,
            model.Entity.Image,
            model.Entity.Deleted,
            model.Entity.ShowOnWeb,
            model.Entity.Categories.ToList().ToModels(),
            model.Entity.Composition.ToList().ToModels(),
            model.Entity.AvailableModifiers.Where(static mod => !mod.Deleted).ToList().ToModels(),
            model.Entity.Costs.ToList().ToModels(),
            model.CurrentCosts,
            model.StoreAmounts
        );
    }

    public static partial List<SaleItemListModel> ToModels(this List<SaleItemEntity> entities);

    public static partial StoreEntity ToEntity(this StoreCreateModel model);
    public static partial List<StoreListModel> ToModels(this List<StoreEntity> entities);
    public static partial StoreListModel ToListModel(this StoreEntity entity);

    public static partial CompositionEntity ToEntity(this CompositionCreateModel model);

    public static partial CompositionListModel ToModel(this CompositionEntity entity);
    public static partial List<CompositionListModel> ToModels(this List<CompositionEntity> entities);
    public static partial ContainerEntity ToEntity(this ContainerCreateModel model);

    public static ContainerListModel ToModel(this ContainerIntermediateModel model) {
        return new ContainerListModel(
            model.Entity.Id,
            model.Entity.OpenSince,
            model.Entity.Pipe?.ToModel(),
            model.Entity.Deleted,
            model.Entity.Template!.ToModel(),
            model.CurrentAmount
        );
    }

    public static List<ContainerListModel> ToModels(this List<ContainerIntermediateModel> models) {
        return [.. models.Select(m => m.ToModel())];
    }

    public static partial ContainerTemplateListModel ToModel(this ContainerTemplateEntity entity);
    public static partial List<ContainerTemplateListModel> ToModels(this List<ContainerTemplateEntity> entities);
    public static partial ContainerTemplateEntity ToEntity(this ContainerTemplateCreateModel model);
    public static partial void UpdateEntity(this ContainerTemplateCreateModel model, ContainerTemplateEntity entity);
    public static partial CurrencyCostEntity ToEntity(this CostCreateModel createModel);
    public static partial CostListModel ToModel(this CurrencyCostEntity entity);
    public static partial List<CostListModel> ToModels(this List<CurrencyCostEntity> entities);

    public static partial CurrencyListModel ToModel(this CurrencyEntity entity);

    public static partial List<DiscountListModel> ToModels(this List<DiscountEntity> entities);

    public static DiscountDetailModel ToModel(this DiscountIntermediateModel model) {
        return new DiscountDetailModel(
            model.Entity.Id,
            model.Entity.Name,
            model.Entity.Deleted,
            model.DiscountUsages
        );
    }

    public static partial List<DiscountUsageListModel> ToModels(this List<DiscountUsageEntity> entities);

    public static partial ModifierEntity ToEntity(this ModifierCreateModel model);
    public static partial void UpdateEntity(this ModifierCreateModel model, ModifierEntity entity);
    public static partial ModifierDetailModel ToModel(this ModifierEntity entity);
    public static partial List<ModifierListModel> ToModels(this List<ModifierEntity> entities);

    [MapperIgnoreSource(nameof(StoreItemCreateModel.CategoryIds))]
    public static partial StoreItemEntity ToEntity(this StoreItemCreateModel model);

    [MapperIgnoreSource(nameof(StoreItemCreateModel.CategoryIds))]
    public static partial void UpdateEntity(this StoreItemCreateModel model, StoreItemEntity entity);

    public static partial StoreItemListModel ToListModel(this StoreItemEntity entity);
    public static StoreItemDetailModel ToModel(this StoreItemIntermediateModel model) {
        return new StoreItemDetailModel(
            model.Entity.Id,
            model.Entity.Name,
            model.Entity.Image,
            model.Entity.Deleted,
            model.Entity.UnitName,
            model.Entity.BarmanCanStock,
            model.Entity.IsContainerItem,
            model.Entity.Categories.ToList().ToModels(),
            model.Entity.Costs.ToList().ToModels(),
            model.CurrentCosts,
            model.StoreAmounts
        );
    }

    public static partial List<StoreItemListModel> ToModels(this List<StoreItemEntity> entities);

    public static partial DiscountUsageDetailModel? ToModel(this DiscountUsageEntity? entity);

    public static partial List<StoreTransactionListModel> ToModels(this List<StoreTransactionEntity> entities);
    public static partial StoreTransactionDetailModel ToModel(this StoreTransactionEntity entity);

    public static partial List<StoreTransactionItemListModel> ToModels(this List<StoreTransactionItemEntity> entities);

    public static partial UserListModel ToListModel(this UserAccountEntity entity);

    public static UserDetailModel ToModel(this UserIntermediateModel model) {
        return new UserDetailModel(
            model.Entity.Id,
            model.Entity.UserName,
            model.Entity.Deleted,
            model.CurrencyAmounts,
            model.CurrencyChanges,
            model.DiscountUsages
        );
    }
    public static partial List<UserListModel> ToModels(this List<UserAccountEntity> entity);

    public static partial List<SaleTransactionListModel> ToModels(this List<SaleTransactionEntity> entities);
    public static partial SaleTransactionDetailModel ToModel(this SaleTransactionEntity entity);
    public static partial SaleTransactionListModel ToListModel(this SaleTransactionEntity entity);
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

public record DiscountIntermediateModel(
    DiscountEntity Entity,
    Page<DiscountUsageListModel> DiscountUsages
);

public record StoreItemIntermediateModel(
    StoreItemEntity Entity,
    List<CostListModel> CurrentCosts,
    List<StoreAmountStoreItemListModel> StoreAmounts
);

public record SaleItemIntermediateModel(
    SaleItemEntity Entity,
    List<CostListModel> CurrentCosts,
    List<StoreAmountSaleItemListModel> StoreAmounts
);

public record UserIntermediateModel(
    UserAccountEntity Entity,
    List<TotalCurrencyChangeListModel> CurrencyAmounts,
    Page<CurrencyChangeListModel> CurrencyChanges,
    Page<DiscountUsageListModel> DiscountUsages
);
