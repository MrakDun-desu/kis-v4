using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Services;

public class StoreItemAmountService(
        KisDbContext dbContext
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;

    public async Task<StoreItemAmountReadAllResponse> ReadAllAsync(
            StoreItemAmountReadAllRequest req,
            CancellationToken token = default) {
        return await _dbContext.StoreItemAmounts
            .Where(sia => sia.StoreId == req.StoreId)
            .PaginateAsync(
                    req,
                    sia => new StoreItemAmountModel {
                        StoreId = sia.StoreId,
                        Amount = sia.Amount,
                        StoreItemId = sia.StoreItemId
                    },
                    (data, meta) => new StoreItemAmountReadAllResponse { Data = data, Meta = meta },
                    sia => sia.StoreItemId,
                    token: token
                );
    }
}
