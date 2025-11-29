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
