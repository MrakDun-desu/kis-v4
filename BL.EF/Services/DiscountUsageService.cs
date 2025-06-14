using KisV4.BL.Common.Services;
using KisV4.Common;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

public class DiscountUsageService(KisDbContext dbContext) : IDiscountUsageService, IScopedService {
    public OneOf<Page<DiscountUsageListModel>, Dictionary<string, string[]>> ReadAll(
        int? page,
        int? pageSize,
        int? discountId,
        int? userId) {
        var query = dbContext.DiscountUsages.AsQueryable();
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

    public OneOf<DiscountUsageDetailModel, Dictionary<string, string[]>> Create(DiscountUsageCreateModel createModel) {
        // will be implemented with cs-script, executing custom scripts
        // on the database. Not very important for start
        throw new NotImplementedException();
    }

    public OneOf<DiscountUsageDetailModel, NotFound> Read(int id) {
        var output = dbContext.DiscountUsages.Find(id).ToModel();
        return output is not null ? (OneOf<DiscountUsageDetailModel, NotFound>)output : (OneOf<DiscountUsageDetailModel, NotFound>)new NotFound();
    }
}
