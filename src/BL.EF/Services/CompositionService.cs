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

    public CompositionReadAllResponse ReadAll(CompositionReadAllRequest req) {
        var data = _dbContext.Compositions
            .Where(c => c.CompositeId == req.CompositeId)
            .Include(c => c.StoreItem)
            .Select(c => new CompositionModel {
                CompositeId = c.CompositeId,
                Amount = c.Amount,
                StoreItem = c.StoreItem!.ToModel()
            });

        return new CompositionReadAllResponse { Data = data };
    }

    public void Put(CompositionPutRequest req) {
        var entity = _dbContext.Compositions.Find(req.StoreItemId, req.CompositeId);
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

        _dbContext.SaveChanges();
    }
}
