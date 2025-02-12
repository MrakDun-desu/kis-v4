using BL.EF.Tests.Fixtures;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.Common.Models;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Shouldly;

namespace BL.EF.Tests.Services;

public class ModifierServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable
{
    private readonly KisDbContext _referenceDbContext;
    private readonly KisDbContext _normalDbContext;
    private readonly ModifierService _modifierService;

    public ModifierServiceTests(KisDbContextFactory dbContextFactory)
    {
        (_referenceDbContext, _normalDbContext) = dbContextFactory.CreateDbContextAndReference();
        _modifierService = new ModifierService(_normalDbContext);
    }

    public async ValueTask DisposeAsync()
    {
        await _referenceDbContext.DisposeAsync();
        await _normalDbContext.DisposeAsync();
    }

    public void Dispose()
    {
        _referenceDbContext.Dispose();
        _normalDbContext.Dispose();
    }

    [Fact]
    public void Create_CreatesModifier_WhenDataIsValid()
    {
        var targetItem = _referenceDbContext.SaleItems.Add(new SaleItemEntity { Name = "Test sale item" });
        _referenceDbContext.SaveChanges();
        var createModel = new ModifierCreateModel("Some modifier", string.Empty, true, targetItem.Entity.Id);
        var createdId = _modifierService.Create(createModel).AsT0.Id;
    
        var createdEntity = _referenceDbContext.Modifiers.Find(createdId);
        var expectedEntity = new ModifierEntity
        {
            ModificationTargetId = createModel.ModificationTargetId,
            ModificationTarget = targetItem.Entity,
            Id = createdId,
            Name = createModel.Name,
            ShowOnWeb = createModel.ShowOnWeb
        };
        createdEntity.ShouldBeEquivalentTo(expectedEntity);
    }
    
    [Fact]
    public void Update_UpdatesName_WhenExistingId()
    {
        const string oldName = "Some modifier";
        const string newName = "Some modifier 2";
        var saleItem = new SaleItemEntity { Name = "Test sale item" };
        var testModifier1 = new ModifierEntity { Name = oldName, ModificationTarget = saleItem };
        var insertedEntity = _referenceDbContext.Modifiers.Add(testModifier1);
        _referenceDbContext.SaveChanges();
        var updateModel = new ModifierCreateModel(newName, testModifier1.Image, testModifier1.ShowOnWeb, testModifier1.ModificationTargetId);
    
        var updateResult = _modifierService.Update(insertedEntity.Entity.Id, updateModel);
    
        updateResult.IsT0.ShouldBeTrue();
        var expectedEntity = insertedEntity.Entity with { Name = newName };
        updateResult.AsT0.ShouldBeEquivalentTo(expectedEntity.ToModel());
    }
    
    [Fact]
    public void Update_UpdatesImage_WhenExistingId()
    {
        const string oldImage = "Some modifier";
        const string newImage = "Some modifier 2";
        var saleItem = new SaleItemEntity { Name = "Test sale item" };
        var testModifier1 = new ModifierEntity
            { Name = "Test sale item", Image = oldImage, ModificationTarget = saleItem };
        var insertedEntity = _referenceDbContext.Modifiers.Add(testModifier1);
        _referenceDbContext.SaveChanges();
        var updateModel = new ModifierCreateModel(testModifier1.Name, newImage, testModifier1.ShowOnWeb, testModifier1.ModificationTargetId);
    
        var updateResult = _modifierService.Update(insertedEntity.Entity.Id, updateModel);

        updateResult.IsT0.ShouldBeTrue();
        var expectedEntity = insertedEntity.Entity with { Image = newImage };
        updateResult.AsT0.ShouldBeEquivalentTo(expectedEntity.ToModel());
    }
    
    [Fact]
    public void Update_UpdatesShowOnWeb_WhenExistingId()
    {
        var saleItem = new SaleItemEntity { Name = "Test sale item" };
        var testModifier1 = new ModifierEntity { Name = "Test name", ModificationTarget = saleItem };
        var insertedEntity = _referenceDbContext.Modifiers.Add(testModifier1);
        _referenceDbContext.SaveChanges();
        var updateModel = new ModifierCreateModel(testModifier1.Name, testModifier1.Image, true, testModifier1.ModificationTargetId);
    
        var updateResult = _modifierService.Update(insertedEntity.Entity.Id, updateModel);

        updateResult.IsT0.ShouldBeTrue();
        var expectedEntity = insertedEntity.Entity with { ShowOnWeb = true };
        updateResult.AsT0.ShouldBeEquivalentTo(expectedEntity.ToModel());
    }
    
    [Fact]
    public void Update_ReturnsFalse_WhenNotFound()
    {
        var updateModel = new ModifierCreateModel("Something", "Some image", false, 42);
    
        var updateResult = _modifierService.Update(42, updateModel);
    
        updateResult.IsT1.ShouldBeTrue();
    }
    
    [Fact]
    public void Delete_Deletes_WhenExistingId()
    {
        var saleItem = new SaleItemEntity { Name = "Test sale item" };
        var testModifier1 = new ModifierEntity { Name = "Some modifier", ModificationTarget = saleItem };
        var insertedEntity = _referenceDbContext.Modifiers.Add(testModifier1);
        _referenceDbContext.SaveChanges();
        _referenceDbContext.ChangeTracker.Clear();
    
        var deleteResult = _modifierService.Delete(insertedEntity.Entity.Id);
    
        deleteResult.IsT0.ShouldBeTrue();
        deleteResult.AsT0.Deleted.ShouldBeTrue();
        var deletedEntity = _referenceDbContext.Modifiers.Find(insertedEntity.Entity.Id);
        deletedEntity!.Deleted.ShouldBeTrue();
    }
    
    [Fact]
    public void Delete_ReturnsFalse_WhenNotFound()
    {
        var deleteResult = _modifierService.Delete(42);

        deleteResult.IsT1.ShouldBeTrue();
    }
}