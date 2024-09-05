using KisV4.BL.Common.Services;
using KisV4.Common;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

public class DiscountService(KisDbContext dbContext, IDiscountUsageService discountUsageService)
    : IDiscountService, IScopedService
{
    public IEnumerable<DiscountListModel> ReadAll(bool? deleted)
    {
        var query = dbContext.Discounts.AsQueryable();
        if (deleted.HasValue)
        {
            query = query.Where(dc => dc.Deleted == deleted.Value);
        }
        return query.ToList().ToModels();
    }

    public OneOf<DiscountDetailModel, NotFound> Read(int id)
    {
        var discount = dbContext.Discounts.AsNoTracking().SingleOrDefault(dc => dc.Id == id);
        if (discount is null)
        {
            return new NotFound();
        }

        var discountUsages = discountUsageService.ReadAll(null, null, id, null);
        var output = new DiscountIntermediateModel(discount, discountUsages.AsT0).ToModel();

        return output;
    }

    public OneOf<DiscountDetailModel, NotFound> Patch(int id)
    {
        var discount = dbContext.Discounts.Find(id);
        if (discount is null)
        {
            return new NotFound();
        }

        discount.Deleted = false;
        dbContext.SaveChanges();

        return Read(id);
    }

    public OneOf<DiscountDetailModel, NotFound> Delete(int id)
    {
        var discount = dbContext.Discounts.Find(id);
        if (discount is null)
        {
            return new NotFound();
        }

        discount.Deleted = true;
        dbContext.SaveChanges();

        return Read(id);
    }
}