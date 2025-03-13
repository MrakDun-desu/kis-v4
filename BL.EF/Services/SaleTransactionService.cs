using KisV4.BL.Common.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

public class SaleTransactionService(
    KisDbContext dbContext,
    IUserService userService,
    TimeProvider timeProvider
) : ISaleTransactionService
{
    public OneOf<SaleTransactionDetailModel, Dictionary<string, string[]>> Create(
        SaleTransactionCreateModel createModel,
        string userName
    )
    {
        var errors = ValidateCreateModel(createModel);
        if (errors.Count > 0)
        {
            return errors;
        }
        var newTransaction = new SaleTransactionEntity
        {
            Timestamp = timeProvider.GetUtcNow(),
            ResponsibleUserId = userService.CreateOrGetId(userName),
        };

        foreach (var item in createModel.SaleTransactionItems)
        {
            var newItem = new SaleTransactionItemEntity
            {
                ItemAmount = item.ItemAmount,
                SaleItemId = item.SaleItemId,
            };
            foreach (ModifierAmountCreateModel modifier in item.ModifierAmounts)
            {
                newItem.ModifierAmounts.Add(
                    new ModifierAmountEntity
                    {
                        ModifierId = modifier.ModifierId,
                        Amount = modifier.Amount,
                    }
                );
            }
            newTransaction.SaleTransactionItems.Add(newItem);
        }

        dbContext.SaleTransactions.Add(newTransaction);
        dbContext.SaveChanges();
        return Read(newTransaction.Id).AsT0;
    }

    private Dictionary<string, string[]> ValidateCreateModel(SaleTransactionCreateModel createModel)
    {
        var errors = new Dictionary<string, string[]>();
        var saleItemIds = new List<int>();
        foreach (var item in createModel.SaleTransactionItems)
        {
            if (item.ItemAmount <= 0)
            {
                errors.AddItemOrCreate(
                    nameof(item.ItemAmount),
                    $"Item {item.SaleItemId} specified invalid amount {item.ItemAmount}. Amount must be more than 0"
                );
            }
            saleItemIds.Add(item.SaleItemId);
        }
    }

    public OneOf<SaleTransactionDetailModel, NotFound> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public OneOf<SaleTransactionDetailModel, NotFound, Dictionary<string, string[]>> Finish(
        int id,
        IEnumerable<CurrencyChangeListModel> currencyChanges
    )
    {
        throw new NotImplementedException();
    }

    public OneOf<SaleTransactionDetailModel, Dictionary<string, string[]>> Put(
        IEnumerable<SaleTransactionItemCreateModel> items
    )
    {
        throw new NotImplementedException();
    }

    public OneOf<SaleTransactionDetailModel, NotFound> Read(int id)
    {
        throw new NotImplementedException();
    }

    public List<SaleTransactionListModel> ReadAll(
        int? page,
        int? pageSize,
        DateTimeOffset? startDate,
        DateTimeOffset? endDate,
        bool? cancelled
    )
    {
        throw new NotImplementedException();
    }

    public List<SaleTransactionListModel> ReadSelfCancellable()
    {
        throw new NotImplementedException();
    }
}
