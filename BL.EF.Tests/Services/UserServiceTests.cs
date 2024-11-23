using BL.EF.Tests.Fixtures;
using FluentAssertions;
using KisV4.BL.EF.Services;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace BL.EF.Tests.Services;

public class UserServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable
{
    private readonly KisDbContext _referenceDbContext;
    private readonly KisDbContext _normalDbContext;
    private readonly UserService _userService;

    public UserServiceTests(KisDbContextFactory dbContextFactory)
    {
        (_referenceDbContext, _normalDbContext) = dbContextFactory.CreateDbContextAndReference();
        _userService = new UserService(_normalDbContext);
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
    public void Create_Creates_WhenUserDoesntExist()
    {
        // arrange
        _referenceDbContext.UserAccounts.RemoveRange(_referenceDbContext.UserAccounts);
        const string username = "Some user";

        // act
        var userId = _userService.CreateOrGetId(username);

        // assert
        var user = _referenceDbContext.UserAccounts.Find(userId);
        user.Should().NotBeNull();
        user!.UserName.Should().Be(username);
    }

    [Fact]
    public void Create_GetsId_WhenUserExists()
    {
        // arrange
        const string username = "Some user";
        var entity = _referenceDbContext.UserAccounts.Add(new UserAccountEntity { UserName = username });
        _referenceDbContext.SaveChanges();

        // act
        var userId = _userService.CreateOrGetId(username);

        // assert
        userId.Should().Be(entity.Entity.Id);
    }
}