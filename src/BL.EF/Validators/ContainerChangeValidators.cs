using FluentValidation;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Validators;

public class ContainerChangeReadAllValidator : AbstractValidator<ContainerChangeReadAllRequest> {
    private readonly KisDbContext _dbContext;

    public ContainerChangeReadAllValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.ContainerId)
            .Must(ContainerExists)
            .WithMessage("Specified container must exist");
    }

    private bool ContainerExists(int containerId) =>
        _dbContext.Containers.Find(containerId) is not null;
}

public class ContainerChangeCreateValidator : AbstractValidator<ContainerChangeCreateRequest> {
    private readonly KisDbContext _dbContext;

    public ContainerChangeCreateValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.ContainerId)
            .Must(ContainerExists)
            .WithMessage("Specified container must exist");
        RuleFor(x => x)
            .Must(AmountLowerOrEqualToCurrent)
            .WithMessage("New container amount must be lower or equal to the current one");
    }

    private bool ContainerExists(int containerId) =>
        _dbContext.Containers.Find(containerId) is not null;

    private bool AmountLowerOrEqualToCurrent(ContainerChangeCreateRequest req) {
        var container = _dbContext.Containers.Find(req.ContainerId);
        if (container is null) {
            return true;
        }

        return container.Amount >= req.NewAmount;
    }
}
