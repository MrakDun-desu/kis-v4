using KisV4.BL.EF.Mapping;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class StockTakingService(
        KisDbContext dbContext
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;

    public async Task<StockTakingReadAllResponse> ReadAllAsync(StockTakingReadAllRequest req, CancellationToken token = default) {
        var stockTakings = _dbContext.StockTakings.Include(st => st.User);
        return await _dbContext.StockTakings
            .Include(st => st.User)
            .AsQueryable()
            .PaginateAsync(
                req,
                st => new StockTakingModel {
                    User = st.User.ToModel()!,
                    CashBoxId = st.CashBoxId,
                    Timestamp = st.Timestamp
                },
                (data, meta) => new StockTakingReadAllResponse { Data = data, Meta = meta },
                st => st.Timestamp,
                true,
                token
            );
    }
}
