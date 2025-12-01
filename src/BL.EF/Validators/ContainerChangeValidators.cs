using FluentValidation;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Validators;

public class ContainerChangeReadAllValidator : AbstractValidator<ContainerChangeReadAllRequest> {
    private readonly KisDbContext _dbContext;

    public ContainerChangeReadAllValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.ContainerId)
            .MustAsync(ContainerExists)
            .WithMessage("Specified container must exist");
    }

    private async Task<bool> ContainerExists(int containerId, CancellationToken token = default) =>
        await _dbContext.Containers.FindAsync(containerId, token) is not null;
}

public class ContainerChangeCreateValidator : AbstractValidator<ContainerChangeCreateRequest> {
    private readonly KisDbContext _dbContext;

    public ContainerChangeCreateValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.ContainerId)
            .MustAsync(ContainerExists)
            .WithMessage("Specified container must exist");
        RuleFor(x => x.NewAmount)
            .GreaterThanOrEqualTo(0);
        RuleFor(x => x)
            .MustAsync(AmountLowerOrEqualToCurrent)
            .WithMessage("New container amount must be lower or equal to the current one");
    }

    private async Task<bool> ContainerExists(int containerId, CancellationToken token = default) =>
        await _dbContext.Containers.FindAsync(containerId, token) is not null;

    private async Task<bool> AmountLowerOrEqualToCurrent(ContainerChangeCreateRequest req, CancellationToken token = default) {
        var container = await _dbContext.Containers.FindAsync(req.ContainerId, token);
        if (container is null) {
            return true;
        }

        return container.Amount >= req.NewAmount;
    }
}
