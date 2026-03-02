using KisV4.Common;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace KisV4.Api.Middlewares;

public class UserCreationMiddleware(RequestDelegate next) {

    public async Task InvokeAsync(HttpContext context, KisDbContext dbContext) {
        var userIdOpt = context.User.TryGetUserId();
        if (userIdOpt is not { } userId) {
            await next(context);
            return;
        }
        var existingUser = await dbContext.Users.FindAsync(userId);

        if (existingUser is null) {
            dbContext.Users.Add(new User { Id = userId });
            await dbContext.SaveChangesAsync();
        }

        await next(context);
    }
}
