using CSScriptLib;
using KisV4.BL.Common.Services;
using KisV4.BL.EF.Helpers;
using KisV4.Common;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

public class DiscountUsageService(KisDbContext dbContext) : IDiscountUsageService, IScopedService {
    public OneOf<Page<DiscountUsageListModel>, Dictionary<string, string[]>> ReadAll(
        int? page,
        int? pageSize,
        int? discountId,
        int? userId
    ) {
        var query = dbContext.DiscountUsages
            .Include(du => du.User)
            .Include(du => du.Discount)
            .AsQueryable();
        var errors = new Dictionary<string, string[]>();
        if (discountId.HasValue) {
            if (!dbContext.Discounts.Any(dc => dc.Id == discountId)) {
                errors.AddItemOrCreate(
                    nameof(discountId),
                    $"Discount with id {discountId} doesn't exist"
                );
            }

            query = query.Where(du => du.DiscountId == discountId.Value);
        }

        if (userId.HasValue) {
            if (!dbContext.UserAccounts.Any(ua => ua.Id == userId.Value)) {
                errors.AddItemOrCreate(
                    nameof(userId),
                    $"User with id {userId} doesn't exist"
                );
            }

            query = query.Where(du => du.UserId == userId.Value);
        }

        return errors.Count > 0
            ? errors
            : query.Page(page ?? 1, pageSize ?? Constants.DefaultPageSize, Mapper.ToModels);
    }

    public OneOf<DiscountUsageDetailModel, Dictionary<string, string[]>> Create(
            DiscountUsageCreateModel createModel,
            string discountScriptPath
    ) {
        var discountEntity = dbContext.Discounts.Find(createModel);
        var saleTransactionEntity = dbContext.SaleTransactions.Find(createModel.SaleTransactionId);

        var errors = new Dictionary<string, string[]>();
        if (discountEntity is null) {
            errors.AddItemOrCreate(
                nameof(createModel.DiscountId),
                $"Discount with id {createModel.DiscountId} doesn't exist"
            );
        }
        if (saleTransactionEntity is null) {
            errors.AddItemOrCreate(
                nameof(createModel.SaleTransactionId),
                $"Sale transaction with id {createModel.SaleTransactionId} doesn't exist"
            );
        }

        if (errors.Count != 0) {
            return errors;
        }

        var discountScriptFile = Path.Combine(
            discountScriptPath,
            $"Discount{discountEntity!.Id}-{discountEntity.Name}.cs"
        );
        var discountScript = CSScript.Evaluator
            .LoadFile<IDiscountScript>(discountScriptFile);

        return discountScript.Run(createModel.SaleTransactionId, dbContext);
    }

    public OneOf<DiscountUsageDetailModel, NotFound> Read(int id) {
        var output = dbContext.DiscountUsages
            .Include(du => du.User)
            .Include(du => du.Discount)
            .Include(du => du.UsageItems)
            .ThenInclude(dui => dui.Currency)
            .SingleOrDefault(du => du.Id == id)
            .ToModel();
        return output is not null ? output : new NotFound();
    }
}
