using KisV4.Common.Enums;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace KisV4.Api.Middlewares;

public class UserCreationMiddleware(RequestDelegate next) {

    public async Task InvokeAsync(HttpContext context, KisDbContext dbContext) {
        var user = context.User;
        var userIdOpt = user.Identity?.Name;
        if (userIdOpt is not { } userId) {
            await next(context);
            return;
        }
        var existingUser = await dbContext.Users.FindAsync(userId);

        if (existingUser is null) {
            dbContext.Users.Add(new User {
                Id = userId,
                Nick = user?.Claims.FirstOrDefault(c => c.Type == "nick")?.Value,
                GamificationAllowed = user?.Claims
                    .FirstOrDefault(c => c.Type == "gam")
                    ?.Value.ToLower() == "true",
                Accounts = [
                    new UserAccount {
                        Type = AccountType.Prestige
                    }
                ]
            });
            await dbContext.SaveChangesAsync();
        }

        await next(context);
    }
}
