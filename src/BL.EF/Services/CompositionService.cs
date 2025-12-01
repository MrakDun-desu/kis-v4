using KisV4.BL.EF.Mapping;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class CompositionService(
        KisDbContext dbContext
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;

    public async Task<CompositionReadAllResponse> ReadAllAsync(CompositionReadAllRequest req, CancellationToken token = default) {
        var data = await _dbContext.Compositions
            .Where(c => c.CompositeId == req.CompositeId)
            .Include(c => c.StoreItem)
            .Select(c => new CompositionModel {
                CompositeId = c.CompositeId,
                Amount = c.Amount,
                StoreItem = c.StoreItem!.ToModel()
            })
            .ToArrayAsync(token);

        return new CompositionReadAllResponse { Data = data };
    }

    public async Task Put(CompositionPutRequest req, CancellationToken token = default) {
        var entity = await _dbContext.Compositions.FindAsync(req.StoreItemId, req.CompositeId, token);
        if (entity is not null) {
            if (req.Amount == 0) {
                _dbContext.Compositions.Remove(entity);
            } else {
                entity.Amount = req.Amount;
                _dbContext.Compositions.Update(entity);
            }
        } else if (req.Amount != 0) {
            _dbContext.Compositions.Add(new Composition {
                StoreItemId = req.StoreItemId,
                CompositeId = req.CompositeId,
                Amount = req.Amount
            });
        }

        await _dbContext.SaveChangesAsync(token);
    }
}
