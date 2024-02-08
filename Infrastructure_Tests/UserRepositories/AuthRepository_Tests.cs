using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure_Tests.UserRepositories;

public class AuthRepository_Tests
{
    private readonly DataContext _context = new(new DbContextOptionsBuilder<DataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);

    [Fact]
    public async Task CreateAsync_ShouldCreateSaveRecordToDatabase_ReturnAuthEntity()
    {
        //Arrange
        var authRepository = new AuthRepository(_context);
        var authEntity = new UserAuthEntity
        {
            UserId = Guid.NewGuid(),
            Email = "email",
            Password = "password",
        };

        //Act
        var result = await authRepository.CreateAsync(authEntity);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldNotSaveRecordToDatabase_ReturnNull()
    {
        //Arrange
        var authRepository = new AuthRepository(_context);
        var authEntity = new UserAuthEntity
        {
            Email = "email",
            Password = "password",

        };

        //Act
        var result = await authRepository.CreateAsync(authEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldGetAllRecords_ReturnIEnumerableOfTypeAuthEntity()
    {
        //Arrange
        var authRepository = new AuthRepository(_context);
        var authEntity = new UserAuthEntity
        {
            UserId = Guid.NewGuid(),
            Email = "email",
            Password = "password",

        };
        await authRepository.CreateAsync(authEntity);

        //Act
        var result = await authRepository.GetAllAsync();

        //Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<UserAuthEntity>>(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetAsync_ShouldGetOneUserAuth_ReturnOneAuthEntity()
    {
        //Arrange
        var authRepository = new AuthRepository(_context);
        var authEntity = new UserAuthEntity
        {
            UserId = Guid.NewGuid(),
            Email = "email",
            Password = "password"
        };
        _context.UserAuths.Add(authEntity);
        _context.SaveChanges();

        Expression<Func<UserAuthEntity, bool>> validExpression = entity => entity.Email == "email";

        //Act
        var result = await authRepository.GetAsync(validExpression);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAsync_ShouldNotGetOneUserAuth_ReturnNull()
    {
        //Arrange
        var authRepository = new AuthRepository(_context);
        Expression<Func<UserAuthEntity, bool>> invalidExpression = entity => entity.Email == "hej";

        //Act
        var result = await authRepository.GetAsync(invalidExpression);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveOneAuthEntity_ReturnTrue()
    {
        //Arrange
        var authRepository = new AuthRepository(_context);
        var authEntity = new UserAuthEntity
        {
            UserId = Guid.NewGuid(),
            Email = "email",
            Password = "password"
        };
        await authRepository.CreateAsync(authEntity);

        //Act
        var result = await authRepository.DeleteAsync(x => x.Email == authEntity.Email);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotFindUserAuthAndRemoveIt_ReturnFalse()
    {
        //Arrange
        var authRepository = new AuthRepository(_context);
        var authEntity = new UserAuthEntity { Email = "Tester" };

        //Act
        var result = await authRepository.DeleteAsync(x => x.Email == authEntity.Email);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUserAuth_ReturnUpdatedEntity()
    {
        //Arrange
        var authRepository = new AuthRepository(_context);
        var authEntity = await authRepository.CreateAsync(new UserAuthEntity
        {
            UserId = Guid.NewGuid(),
            Email = "email",
            Password = "password"
        });

        //Act
        authEntity.Email = "annat";
        var updatedAuth = await authRepository.UpdateAsync(x => x.Email == "email", authEntity);

        //Assert
        Assert.NotNull(updatedAuth);
        Assert.Equal("annat", updatedAuth.Email);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdateUserAuthIfNotExists_ReturnNull()
    {
        //Arrange
        var authRepository = new AuthRepository(_context);
        var authEntity = await authRepository.CreateAsync(new UserAuthEntity
        {
            UserId = Guid.NewGuid(),
            Email = "email",
            Password = "password"
        });

        //Act
        authEntity.Email = "annat";
        var updatedAuth = await authRepository.UpdateAsync(x => x.Email == "annat", authEntity);

        //Assert
        Assert.Null(updatedAuth);
    }
}
