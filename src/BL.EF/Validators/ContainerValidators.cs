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
            .MustAsync(BeNullOrExistingStore)
            .WithMessage("StoreId must either be null or identify an existing store");
        RuleFor(x => x.TemplateId)
            .MustAsync(BeNullOrExistingTemplate)
            .WithMessage("TemplateId must either be null or identify an existing template");
        RuleFor(x => x.PipeId)
            .MustAsync(BeNullOrExistingPipe)
            .WithMessage("PipeId must either be null or identify an existing pipe");
    }

    private async Task<bool> BeNullOrExistingStore(int? storeId, CancellationToken token = default) => storeId switch {
        null => true,
        { } val => await _dbContext.Stores.FindAsync(val, token) is not null
    };

    private async Task<bool> BeNullOrExistingTemplate(int? templateId, CancellationToken token = default) => templateId switch {
        null => true,
        { } val => await _dbContext.ContainerTemplates.FindAsync(val, token) is not null
    };

    private async Task<bool> BeNullOrExistingPipe(int? pipeId, CancellationToken token = default) => pipeId switch {
        null => true,
        { } val => await _dbContext.Pipes.FindAsync(val, token) is not null
    };
}

public class ContainerCreateValidator : AbstractValidator<ContainerCreateRequest> {
    private readonly KisDbContext _dbContext;

    public ContainerCreateValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.StoreId)
            .MustAsync(BeExistingStore)
            .WithMessage("Specified store must exist");

        RuleFor(x => x.TemplateId)
            .MustAsync(BeExistingTemplate)
            .WithMessage("Specified container template must exist");
    }

    private async Task<bool> BeExistingStore(int storeId, CancellationToken token = default) =>
        await _dbContext.Stores.FindAsync(storeId, token) is not null;

    private async Task<bool> BeExistingTemplate(int templateId, CancellationToken token = default) =>
        await _dbContext.ContainerTemplates.FindAsync(templateId, token) is not null;
}

public class ContainerUpdateValidator : AbstractValidator<ContainerUpdateRequest> {
    private readonly KisDbContext _dbContext;

    public ContainerUpdateValidator(KisDbContext dbContext) {
        _dbContext = dbContext;

        RuleFor(x => x.StoreId)
            .MustAsync(BeExistingStore)
            .WithMessage("Specified store must exist");
        RuleFor(x => x.PipeId)
            .MustAsync(BeNullOrExistingPipe)
            .WithMessage("PipeId must either be null or identify an existing pipe");
    }

    private async Task<bool> BeExistingStore(int storeId, CancellationToken token = default) =>
        await _dbContext.Stores.FindAsync(storeId, token) is not null;

    private async Task<bool> BeNullOrExistingPipe(int? pipeId, CancellationToken token = default) => pipeId switch {
        null => true,
        { } val => await _dbContext.Pipes.FindAsync(val, token) is not null
    };
}
