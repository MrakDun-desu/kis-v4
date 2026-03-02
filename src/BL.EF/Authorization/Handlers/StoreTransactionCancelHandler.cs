using KisV4.BL.EF.Authorization.Requirements;
using KisV4.Common;
using KisV4.Common.Authorization;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using Microsoft.AspNetCore.Authorization;

namespace KisV4.BL.EF.Authorization.Handlers;

public class StoreTransactionUpdateHandler(
    KisDbContext dbContext,
    TimeProvider timeProvider
) : AuthorizationHandler<PassThroughRequirement, StoreTransactionDeleteRequest> {
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PassThroughRequirement requirement,
        StoreTransactionDeleteRequest request
    ) {
        var reqTime = timeProvider.GetUtcNow();
        var storeTransaction = await dbContext.StoreTransactions.FindAsync(request.Id);
        // if the transaction doesn't exist, no need to bother with further authorization,
        // it will just return NotFound later anyways
        if (storeTransaction is null) {
            context.Succeed(requirement);
            return;
        }

        var userId = context.User.GetUserId();
        var isOwner = storeTransaction.StartedById == userId;
        var withinTimeLimit = storeTransaction.StartedAt + AuthorizationConstants.TransactionCancelTimeout >= reqTime;

        if (isOwner && withinTimeLimit) {
            context.Succeed(requirement);
        }
    }
}
