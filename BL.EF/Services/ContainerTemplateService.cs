using KisV4.BL.Common.Services;
using KisV4.Common.DependencyInjection;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.EF.Services;

// ReSharper disable once UnusedType.Global
public class ContainerTemplateService(
    KisDbContext dbContext) : IContainerTemplateService, IScopedService
{
    public OneOf<ICollection<ContainerTemplateListModel>, Dictionary<string, string[]>> ReadAll(
        bool? deleted,
        int? containedItemId)
    {
        var query = dbContext.ContainerTemplates.AsQueryable();
        if (deleted.HasValue) query = query.Where(c => c.Deleted == deleted.Value);

        if (containedItemId.HasValue)
        {
            var containedItem = dbContext.StoreItems.Find(containedItemId);
            var errors = new Dictionary<string, string[]>();
            if (containedItem is null)
            {
                errors.AddItemOrCreate(
                    nameof(containedItemId),
                    $"Store item with id {containedItemId} doesn't exist"
                );
                return errors;
            }

            if (containedItem.Deleted)
                errors.AddItemOrCreate(
                    nameof(containedItemId),
                    "Store item with id {containedItemId} has been marked as deleted"
                );

            if (!containedItem.IsContainerItem)
                errors.AddItemOrCreate(
                    nameof(containedItemId),
                    $"Store item with id {containedItemId} is not a container item"
                );

            if (errors.Count != 0)
                return errors;

            query = query.Where(c => c.ContainedItemId == containedItemId.Value);
        }

        return query.Include(ct => ct.ContainedItem).ToList().ToModels();
    }

    public OneOf<ContainerTemplateListModel, Dictionary<string, string[]>> Create(
        ContainerTemplateCreateModel createModel)
    {
        var errors = new Dictionary<string, string[]>();

        if (createModel.Amount <= 0)
            errors.AddItemOrCreate(
                nameof(createModel.Amount),
                $"Amount of a container template needs to be more than 0. Received value: {createModel.Amount}"
            );

        var containedItem = dbContext.StoreItems.Find(createModel.ContainedItemId);
        if (containedItem is null)
            errors.AddItemOrCreate(
                nameof(createModel.ContainedItemId),
                $"Store item with id {createModel.ContainedItemId} doesn't exist"
            );

        // returning errors here because can't check for more errors with contained item being null
        if (errors.Count > 0) return errors;

        if (containedItem!.Deleted)
            errors.AddItemOrCreate(
                nameof(createModel.ContainedItemId),
                $"Store item with id {createModel.ContainedItemId} has been marked as deleted"
            );

        if (!containedItem.IsContainerItem)
            errors.AddItemOrCreate(
                nameof(createModel.ContainedItemId),
                $"Store item with id {createModel.ContainedItemId} is not a container item"
            );

        if (errors.Count != 0)
            return errors;

        var entity = createModel.ToEntity();
        dbContext.ContainerTemplates.Add(entity);
        dbContext.SaveChanges();

        return entity.ToModel();
    }

    public OneOf<Success, NotFound, Dictionary<string, string[]>> Update(int id,
        ContainerTemplateCreateModel updateModel)
    {
        if (!dbContext.ContainerTemplates.Any(ct => ct.Id == id)) return new NotFound();

        var containedItem = dbContext.StoreItems.Find(updateModel.ContainedItemId);
        var errors = new Dictionary<string, string[]>();
        if (containedItem is null)
        {
            errors.AddItemOrCreate(
                nameof(updateModel.ContainedItemId),
                $"Store item with id {updateModel.ContainedItemId} doesn't exist"
            );
            return errors;
        }

        if (containedItem.Deleted)
            errors.AddItemOrCreate(
                nameof(updateModel.ContainedItemId),
                $"Store item with id {updateModel.ContainedItemId} has been marked as deleted"
            );

        if (!containedItem.IsContainerItem)
            errors.AddItemOrCreate(
                nameof(updateModel.ContainedItemId),
                $"Store item with id {updateModel.ContainedItemId} is not a container item"
            );

        if (errors.Count != 0)
            return errors;

        var entity = updateModel.ToEntity();
        entity.Deleted = false;
        entity.Id = id;
        dbContext.ContainerTemplates.Update(entity);
        dbContext.SaveChanges();

        return new Success();
    }


    public OneOf<Success, NotFound> Delete(int id)
    {
        var entity = dbContext.ContainerTemplates.Find(id);
        if (entity is null) return new NotFound();

        entity.Deleted = true;
        dbContext.ContainerTemplates.Update(entity);
        dbContext.SaveChanges();

        return new Success();
    }
}