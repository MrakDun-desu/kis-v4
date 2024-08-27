using KisV4.BL.Common.Services;
using KisV4.Common;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using KisV4.DAL.EF.Enums;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class ContainerService(
    KisDbContext dbContext,
    TimeProvider timeProvider,
    IUserService userService) : IContainerService, IScopedService
{
    public OneOf<Page<ContainerReadAllModel>, Dictionary<string, string[]>> ReadAll(
        int? page,
        int? pageSize,
        bool? deleted,
        int? pipeId)
    {
        var query = dbContext.Containers.AsQueryable();
        if (deleted.HasValue)
        {
            query = query.Where(c => c.Deleted == deleted.Value);
        }

        if (pipeId.HasValue)
        {
            if (!dbContext.Pipes.Any(p => p.Id == pipeId))
            {
                return new Dictionary<string, string[]>
                {
                    { nameof(pipeId), [$"Pipe with id {pipeId} doesn't exist"] }
                };
            }

            query = query.Where(c => c.PipeId == pipeId.Value);
        }

        var intermediateQuery = query
            .Include(c => c.StoreTransactionItems)
            .Include(c => c.Template)
            .Include(c => c.Pipe)
            .Select(c =>
                new ContainerIntermediateModel(c,
                    c.StoreTransactionItems
                        .Where(sti => !sti.Cancelled)
                        .Sum(sti => sti.ItemAmount)));

        var realPage = page ?? 1;
        var realPageSize = pageSize ?? Constants.DefaultPageSize;
        return intermediateQuery.Page(realPage, realPageSize, Mapper.ToModels);
    }

    public OneOf<ContainerReadAllModel, Dictionary<string, string[]>> Create(
        ContainerCreateModel createModel, string userName)
    {
        var errors = new Dictionary<string, string[]>();
        var template = dbContext.ContainerTemplates.Find(createModel.TemplateId);
        if (template is null)
        {
            errors[nameof(createModel.TemplateId)] =
                [$"Container template with id {createModel.TemplateId} doesn't exist"];
        }
        else if (template.Deleted)
        {
            errors[nameof(createModel.TemplateId)] =
                [$"Container template with id {createModel.TemplateId} is currently marked as deleted"];
        }

        if (!dbContext.Pipes.Any(p => p.Id == createModel.PipeId))
        {
            errors[nameof(createModel.PipeId)] = [$"Pipe with id {createModel.PipeId} doesn't exist"];
        }

        if (errors.Count != 0)
        {
            return errors;
        }

        var creationTime = timeProvider.GetUtcNow();
        var container = createModel.ToEntity();
        container.Name = template!.Name;
        container.OpenSince = creationTime;
        container.StoreTransactionItems.Add(
            new StoreTransactionItemEntity
            {
                ItemAmount = template.Amount,
                StoreItemId = template.ContainedItemId,
                StoreTransaction = new StoreTransactionEntity
                {
                    Timestamp = creationTime,
                    TransactionReason = TransactionReason.AddingToStore,
                    ResponsibleUserId = userService.CreateOrGetId(userName)
                }
            }
        );

        dbContext.Containers.Add(container);
        dbContext.SaveChanges();
        return new ContainerIntermediateModel(container, template.Amount).ToModel();
    }

    public OneOf<Success, NotFound, Dictionary<string, string[]>> Update(ContainerUpdateModel updateModel)
    {
        var container = dbContext.Containers.Find(updateModel.Id);
        if (container is null)
        {
            return new NotFound();
        }

        if (updateModel.PipeId.HasValue)
        {
            if (!dbContext.Pipes.Any(p => p.Id == updateModel.PipeId.Value))
                return new Dictionary<string, string[]>
                {
                    { nameof(updateModel.PipeId), [$"Pipe with id {updateModel.PipeId} doesn't exist"] }
                };
        }

        container.PipeId = updateModel.PipeId;

        dbContext.SaveChanges();

        return new Success();
    }

    public OneOf<Success, NotFound> Delete(int id, string userName)
    {
        var container = dbContext.Containers
            .Include(c => c.Template)
            .SingleOrDefault(c => c.Id == id);

        if (container is null)
        {
            return new NotFound();
        }

        var writeOffTime = timeProvider.GetUtcNow();

        var leftoverAmount = dbContext.StoreTransactionItems
            .Where(sti => sti.StoreId == container.Id)
            .Where(sti => !sti.Cancelled)
            .Sum(sti => sti.ItemAmount);

        if (leftoverAmount > 0)
        {
            dbContext.StoreTransactionItems.Add(
                new StoreTransactionItemEntity
                {
                    ItemAmount = -leftoverAmount,
                    StoreItemId = container.Template!.ContainedItemId,
                    StoreId = container.Id,
                    StoreTransaction = new StoreTransactionEntity
                    {
                        Timestamp = writeOffTime,
                        TransactionReason = TransactionReason.WriteOff,
                        ResponsibleUserId = userService.CreateOrGetId(userName)
                    }
                }
            );
        }

        container.Deleted = true;
        container.PipeId = null;
        dbContext.Containers.Update(container);
        dbContext.SaveChanges();

        return new Success();
    }
}