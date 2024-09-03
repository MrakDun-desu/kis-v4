using FluentAssertions;
using KisV4.BL.EF.Services;
using KisV4.DAL.EF;
using KisV4.DAL.EF.Entities;

namespace BL.EF.Tests.Services;

public class UserServiceTests : IClassFixture<KisDbContextFactory>, IDisposable, IAsyncDisposable
{
    private readonly KisDbContext _dbContext;
    private readonly UserService _userService;

    public UserServiceTests(KisDbContextFactory dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
        _userService = new UserService(_dbContext);
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    [Fact]
    public void Create_Creates_WhenUserDoesntExist()
    {
        // arrange
        _dbContext.UserAccounts.RemoveRange(_dbContext.UserAccounts);
        const string username = "Some user";

        // act
        var userId = _userService.CreateOrGetId(username);

        // assert
        var user = _dbContext.UserAccounts.Find(userId);
        user.Should().NotBeNull();
        user!.UserName.Should().Be(username);
    }

    [Fact]
    public void Create_GetsId_WhenUserExists()
    {
        // arrange
        const string username = "Some user";
        var entity = _dbContext.UserAccounts.Add(new UserAccountEntity { UserName = username });
        _dbContext.SaveChanges();

        // act
        var userId = _userService.CreateOrGetId(username);

        // assert
        userId.Should().Be(entity.Entity.Id);
    }
}