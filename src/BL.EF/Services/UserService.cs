using KisV4.BL.EF.Mapping;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace KisV4.BL.EF.Services;

public class UserService(
        KisDbContext dbContext
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;

    public async Task<UserListModel> GetOrCreateAsync(int id, CancellationToken token = default) {
        var entity = await _dbContext.Users.FindAsync(id, token);

        if (entity is null) {
            entity = new User { Id = id };
            _dbContext.Users.Add(entity);
            await _dbContext.SaveChangesAsync(token);
        }

        return entity.ToModel()!;
    }
}
