using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class UserService(KisDbContext dbContext) : IScopedService, IUserService
{
    public int CreateOrGetId(string userName)
    {
        var user = dbContext.UserAccounts.SingleOrDefault(ua => ua.UserName == userName);
        if (user is not null)
        {
            return user.Id;
        }
        user = new UserAccountEntity
        {
            UserName = userName,
        };
        dbContext.UserAccounts.Add(user);
        dbContext.SaveChanges();
        return user.Id;
    }
}