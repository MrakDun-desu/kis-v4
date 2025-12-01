using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Services;

public class CompositeAmountService(
        KisDbContext dbContext
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;

    public async Task<CompositeAmountReadAllResponse> ReadAllAsync(CompositeAmountReadAllRequest req, CancellationToken token = default) {
        return await _dbContext.CompositeAmounts
            .Where(ca => ca.StoreId == req.StoreId)
            .PaginateAsync(
                    req,
                    ca => new CompositeAmountModel {
                        StoreId = ca.StoreId,
                        Amount = ca.Amount,
                        CompositeId = ca.CompositeId
                    },
                    (data, meta) => new CompositeAmountReadAllResponse { Data = data, Meta = meta },
                    ca => ca.CompositeId,
                    token: token
                );
    }
}
