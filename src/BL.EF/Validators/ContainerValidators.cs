using FluentValidation;
using KisV4.Common.Models;
using KisV4.DAL.EF;

namespace KisV4.BL.EF.Validators;

public class ContainerReadAllValidator : AbstractValidator<ContainerReadAllRequest> {
    private readonly KisDbContext _dbContext;

    public ContainerReadAllValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        Include(new PagedRequestValidator());

        RuleFor(x => x.StoreId)
            .Must(BeNullOrExistingStore)
            .WithMessage("StoreId must either be null or identify an existing store");
        RuleFor(x => x.TemplateId)
            .Must(BeNullOrExistingTemplate)
            .WithMessage("TemplateId must either be null or identify an existing template");
        RuleFor(x => x.PipeId)
            .Must(BeNullOrExistingPipe)
            .WithMessage("PipeId must either be null or identify an existing pipe");
    }

    private bool BeNullOrExistingStore(int? storeId) => storeId switch {
        null => true,
        { } val => _dbContext.Stores.Find(val) is not null
    };

    private bool BeNullOrExistingTemplate(int? templateId) => templateId switch {
        null => true,
        { } val => _dbContext.ContainerTemplates.Find(val) is not null
    };

    private bool BeNullOrExistingPipe(int? pipeId) => pipeId switch {
        null => true,
        { } val => _dbContext.Pipes.Find(val) is not null
    };
}

public class ContainerCreateValidator : AbstractValidator<ContainerCreateRequest> {
    private readonly KisDbContext _dbContext;

    public ContainerCreateValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.StoreId)
            .Must(BeExistingStore)
            .WithMessage("Specified store must exist");

        RuleFor(x => x.TemplateId)
            .Must(BeExistingTemplate)
            .WithMessage("Specified container template must exist");
    }

    private bool BeExistingStore(int storeId) =>
        _dbContext.Stores.Find(storeId) is not null;

    private bool BeExistingTemplate(int templateId) =>
        _dbContext.ContainerTemplates.Find(templateId) is not null;
}

public class ContainerUpdateValidator : AbstractValidator<ContainerUpdateRequest> {
    private readonly KisDbContext _dbContext;

    public ContainerUpdateValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.StoreId)
            .Must(BeExistingStore)
            .WithMessage("Specified store must exist");
        RuleFor(x => x.PipeId)
            .Must(BeNullOrExistingPipe)
            .WithMessage("PipeId must either be null or identify an existing pipe");
    }

    private bool BeNullOrExistingPipe(int? pipeId) => pipeId switch {
        null => true,
        { } val => _dbContext.Pipes.Find(val) is not null
    };

    private bool BeExistingStore(int storeId) =>
        _dbContext.Stores.Find(storeId) is not null;
}
