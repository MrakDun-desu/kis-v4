using KisV4.BL.EF.Mapping;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace KisV4.BL.EF.Services;

public class CompositionService(
        KisDbContext _dbContext
        ) : IScopedService {

    private readonly KisDbContext _dbContext = _dbContext;

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

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(token);

        try {
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

            await _dbContext.CompositeAmounts
                .Where(ca => ca.CompositeId == req.CompositeId)
                .ExecuteUpdateAsync(
                        props => props.SetProperty(
                            x => x.Amount,
                            x => _dbContext.Compositions
                                .Where(c => c.CompositeId == x.CompositeId)
                                .Select(c => (int)(_dbContext.StoreItemAmounts
                                    .Where(sia => sia.StoreItemId == c.StoreItemId
                                        && sia.StoreId == x.StoreId)
                                    .Select(sia => sia.Amount)
                                    .Sum() / c.Amount)
                                )
                                .Min()
                            )
                        );

            await transaction.CommitAsync(token);
        } catch {
            await transaction.RollbackAsync(token);
            throw;
        }

    }
}
