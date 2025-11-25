using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class StockTakingService(
        KisDbContext dbContext
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;

    public StockTakingReadAllResponse ReadAll(StockTakingReadAllRequest req) {
        return _dbContext.StockTakings
            .Include(st => st.User)
            .AsQueryable()
            .Paginate(
                req,
                st => new StockTakingModel {
                    User = new UserListModel {
                        Id = st.User!.Id
                    },
                    CashBoxId = st.CashBoxId,
                    Timestamp = st.Timestamp
                },
                (data, meta) => new StockTakingReadAllResponse { Data = data, Meta = meta },
                st => st.Timestamp,
                true
            );
    }
}
