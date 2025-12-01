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

    public UserListModel GetOrCreate(int id) {
        var entity = _dbContext.Users.Find(id);

        if (entity is null) {
            entity = new User { Id = id };
            _dbContext.Users.Add(entity);
            _dbContext.SaveChanges();
        }

        return entity.ToModel();
    }
}
