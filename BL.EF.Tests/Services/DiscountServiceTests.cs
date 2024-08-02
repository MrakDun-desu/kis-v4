using FluentAssertions;
using KisV4.BL.EF;
using KisV4.BL.EF.Services;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;
using Xunit.Abstractions;

namespace BL.EF.Tests.Services;

public class
    DiscountServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable {
    private readonly DiscountService _cashBoxService;
    private readonly KisDbContext _dbContext;
    private readonly Mapper _mapper;

    public DiscountServiceTests(KisDbContextFactory dbContextFactory, ITestOutputHelper output) {
        _dbContext = dbContextFactory.CreateDbContext();
        _mapper = new Mapper();
        _cashBoxService = new DiscountService(_dbContext, _mapper);
        _output = output;
    }

    public void Dispose() {
        _dbContext.Dispose();
    }

    public async ValueTask DisposeAsync() {
        await _dbContext.DisposeAsync();
    }

    [Fact]
    public void ReadAll_ReadsAll() {
        var testDiscount1 = new DiscountEntity { Name = "Some discount box" };
        var testDiscount2 = new DiscountEntity { Name = "Some discount box 2" };
        _dbContext.Discounts.Add(testDiscount1);
        _dbContext.Discounts.Add(testDiscount2);
        _dbContext.SaveChanges();

        var readModels = _cashBoxService.ReadAll();
        var mappedModels =
            _mapper.ToModels(_dbContext.Discounts.ToList());

        readModels.Should().BeEquivalentTo(mappedModels);
    }

    [Fact]
    public void Read_ReturnsNull_WhenNotFound() {
        var returnedModel = _cashBoxService.Read(42);

        returnedModel.Should().BeNull();
    }

    [Fact]
    public void Read_ReadsCorrectly_WhenSimple() {
        var testDiscount = new DiscountEntity { Name = "Test discount box" };
        var insertedEntity = _dbContext.Discounts.Add(testDiscount);
        _dbContext.SaveChanges();
        var id = insertedEntity.Entity.Id;

        var returnedModel = _cashBoxService.Read(id);

        var expectedModel = _mapper.ToModel(insertedEntity.Entity);
        returnedModel.Should().BeEquivalentTo(expectedModel);
    }
}
