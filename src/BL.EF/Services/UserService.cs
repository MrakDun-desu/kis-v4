using KisV4.BL.EF.Mapping;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Services;

public class UserService(
        KisDbContext dbContext
        ) : IScopedService {

    private readonly KisDbContext _dbContext = dbContext;

    public async Task<UserListModel> GetAsync(int id, CancellationToken token = default) {
        var entity = await _dbContext.Users.FindAsync(id, token)
            ?? throw new InvalidOperationException($"""
                User {id} should already be created when accessing in the service
                """);

        return entity.ToModel()!;
    }
}
