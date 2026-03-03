using KisV4.BL.EF.Authorization.Requirements;
using KisV4.Common;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using Microsoft.AspNetCore.Authorization;

namespace KisV4.BL.EF.Authorization.Handlers;

public class SaleTransactionCloseHandler(
    KisDbContext dbContext
) : AuthorizationHandler<PassThroughRequirement, SaleTransactionCloseRequest> {
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PassThroughRequirement requirement,
        SaleTransactionCloseRequest request
    ) {
        var saleTransaction = await dbContext.SaleTransactions.FindAsync(request.Id);
        // if the sale transaction doesn't exist, no need to bother with further authorization,
        // it will just return NotFound later anyways
        if (saleTransaction is null) {
            context.Succeed(requirement);
            return;
        }

        var userId = context.User.GetUserId();
        var isOwner = saleTransaction.StartedById == userId;

        if (isOwner) {
            context.Succeed(requirement);
        }
    }
}
