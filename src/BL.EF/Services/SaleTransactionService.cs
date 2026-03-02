using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Services;

public class SaleTransactionService(
    KisDbContext dbContext,
    TimeProvider timeProvider,
    UserService userService
) : IScopedService {
    private readonly KisDbContext _dbContext = dbContext;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly UserService _userService = userService;

    public async Task<SaleTransactionReadAllResponse> ReadAllAsync(
        SaleTransactionReadAllRequest req,
        int userId,
        CancellationToken token = default
    ) {
        throw new NotImplementedException();
    }

    public async Task<SaleTransactionReadResponse?> ReadAsync(
        int id,
        CancellationToken token = default
    ) {
        throw new NotImplementedException();
    }

    public async Task<SaleTransactionCreateResponse> CreateAsync(
        SaleTransactionCreateRequest req,
        int userId,
        CancellationToken token = default
    ) {

        throw new NotImplementedException();
    }

    public async Task<SaleTransactionCheckPriceResponse> CheckPriceAsync(
        SaleTransactionCheckPriceRequest req,
        CancellationToken token = default
    ) {

        throw new NotImplementedException();
    }

    public async Task<SaleTransactionOpenResponse> OpenAsync(
        SaleTransactionOpenRequest req,
        int userId,
        CancellationToken token = default
    ) {

        throw new NotImplementedException();
    }

    public async Task<SaleTransactionUpdateResponse?> UpdateAsync(
        SaleTransactionUpdateRequest cmd,
        CancellationToken token = default
    ) {

        throw new NotImplementedException();
    }

    public async Task<SaleTransactionCloseResponse?> CloseAsync(
        SaleTransactionCloseRequest cmd,
        CancellationToken token = default
    ) {

        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(
        SaleTransactionDeleteRequest cmd,
        CancellationToken token = default
    ) {

        throw new NotImplementedException();
    }
}
