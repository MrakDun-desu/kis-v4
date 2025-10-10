using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF;

public interface IDiscountScript {
    DiscountUsageDetailModel Run(int saleTransactionId, KisDbContext dbContext);
}
